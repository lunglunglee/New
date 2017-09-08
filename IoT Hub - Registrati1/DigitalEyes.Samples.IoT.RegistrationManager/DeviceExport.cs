using Microsoft.Azure.Devices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalEyes.Samples.IoT.IotHub.RegistrationManager
{
    public class DeviceExport
    {
        string iotHubConnectionString;
        string blobConnectionString;

        public DeviceExport(string iotHubConnectionString, string blobConnectionString)
        {
            this.iotHubConnectionString = iotHubConnectionString;
            this.blobConnectionString = blobConnectionString;
        }

        public async Task ExportDevicesToBlob()
        {
            CloudBlobContainer container = await GetExportContainer();

            var sasToken = container.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "saspolicy");
            
            Debug.WriteLine("Export devices");

            var containerSasUri = container.Uri + sasToken;

            await ExportDevices(containerSasUri);
        }

        public async Task<List<string>> GetDeviceIds()
        {
            var container = await GetExportContainer();
            var list = await GetDevicesFromBlobExport(container);
            return list.Select(a => a.Id).ToList();
        }

        public async Task<string> GetDevicesAsJson()
        {
            var container = await GetExportContainer();
            var list = await GetDevicesFromBlobExport(container);
            return JsonConvert.SerializeObject(list);
        }

        private async Task<CloudBlobContainer> GetExportContainer()
        {
            var account = CloudStorageAccount.Parse(this.blobConnectionString);
            var client = account.CreateCloudBlobClient();

            var container = client.GetContainerReference("iothub-export-devices");

            await container.CreateIfNotExistsAsync();

            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            };

            permissions.SharedAccessPolicies.Add(
                "saspolicy",
                new SharedAccessBlobPolicy()
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1),
                    Permissions = SharedAccessBlobPermissions.Write
                                        | SharedAccessBlobPermissions.Read
                                        | SharedAccessBlobPermissions.Delete
                });

            container.SetPermissions(permissions);
            return container;
        }
        
        private async Task ExportDevices(string containerSasUri)
        {
            var registryManager = RegistryManager.CreateFromConnectionString(this.iotHubConnectionString);
            var job = await registryManager.ExportDevicesAsync(containerSasUri, false);

            while (true)
            {
                job = await registryManager.GetJobAsync(job.JobId);
                if (job.Status == JobStatus.Completed || job.Status == JobStatus.Failed || job.Status == JobStatus.Cancelled)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private async Task<List<ExportImportDevice>> GetDevicesFromBlobExport(CloudBlobContainer container)
        {
            var blob = container.GetBlobReference("devices.txt");
            Debug.WriteLine("Devices Exported");

            // read blob with devices.txt into list of device objects

            var exportedDevices = new List<ExportImportDevice>();

            using (var streamReader = new StreamReader(await blob.OpenReadAsync(), Encoding.UTF8))
            {
                var ix = 0;
                var tot = 0;
                while (streamReader.Peek() != -1)
                {
                    string line = await streamReader.ReadLineAsync();
                    var device = JsonConvert.DeserializeObject<ExportImportDevice>(line);
                    exportedDevices.Add(device);
                    if (ix++ == 1000)
                    {
                        tot += 1000;
                        Console.WriteLine($"{tot} devices read so far");
                        ix = 0;
                    }
                }
            }

            return exportedDevices;
        }

    }
}
