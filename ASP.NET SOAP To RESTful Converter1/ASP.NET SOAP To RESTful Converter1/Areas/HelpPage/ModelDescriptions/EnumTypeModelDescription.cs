using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ASP.NET_SOAP_To_RESTful_Converter1.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}