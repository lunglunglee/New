using DigitalEyes.Samples.IoT.IotHub.RegistrationManager;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DigitalEyes.Samples.IoT.IotHub.Devices.Export.Wpf.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        const string connectionString = "HostName=????????????.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=????????????";
        const string storageUri = "DefaultEndpointsProtocol=https;AccountName=????blob?????;AccountKey=????blobkey??????";

        public RelayCommand GetDevicesCommand { get; set; }

        string iotHubConnectionString;
        public string IotHubConnectionString
        {
            get
            {
                return iotHubConnectionString;
            }
            set
            {
                if (iotHubConnectionString != value)
                {
                    iotHubConnectionString = value;
                    RaisePropertyChanged("IotHubConnectionString");
                    GetDevicesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string blobConnectionString;
        public string BlobConnectionString
        {
            get
            {
                return blobConnectionString;
            }
            set
            {
                if (blobConnectionString != value)
                {
                    blobConnectionString = value;
                    RaisePropertyChanged("BlobConnectionString");
                    GetDevicesCommand.RaiseCanExecuteChanged();
                }
            }
        }

        string localDeviceExportFolder;
        string hubName = "unknown";

        public MainViewModel()
        {
            GetDevicesCommand = new RelayCommand(DoGetDevices, CanGetDevices);
            localDeviceExportFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "IotHubDevicesExported");
            iotHubConnectionString = connectionString;
            blobConnectionString = storageUri;
            hubName = iotHubConnectionString.Substring(9);
            hubName = hubName.Substring(0, hubName.IndexOf(".azure-devices.net"));
        }

        private bool CanGetDevices()
        {
            return !string.IsNullOrWhiteSpace(IotHubConnectionString) && !string.IsNullOrWhiteSpace(BlobConnectionString);
        }

        private async void DoGetDevices()
        {
            var deviceExporter = new DeviceExport(IotHubConnectionString, BlobConnectionString);
            await deviceExporter.ExportDevicesToBlob();

            var json = await deviceExporter.GetDevicesAsJson();
            SaveStringToFile(json, "json");

            var ids = await deviceExporter.GetDeviceIds();
            var csv = ConvertToExcelCsv(ids);
            SaveStringToFile(csv, "csv");

            MessageBox.Show("Finished!");
        }

        string ConvertToExcelCsv(List<string> devices)
        {
            var csv = "";
            int ix = 0, tot = 0;

            foreach (var id in devices)
            {
                csv += id + "," + Environment.NewLine;

                if (ix++ == 1000)
                {
                    tot += 1000;
                    Console.WriteLine($"{tot} devices to csv so far");
                    ix = 0;
                }
            }
            csv = csv.Substring(0, csv.Length - 3); // drop last comma and line break

            return csv;
        }

        void SaveStringToFile(string content, string extension)
        {
            var datePart = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var fileName = hubName + "_" + datePart + "." + extension;

            if (!System.IO.Directory.Exists(localDeviceExportFolder))
            {
                System.IO.Directory.CreateDirectory(localDeviceExportFolder);
            }

            var filePath = System.IO.Path.Combine(localDeviceExportFolder, fileName);
            System.IO.StreamWriter file = new System.IO.StreamWriter(filePath);
            file.Write(content);
            file.Close();
        }
        
    }
}
