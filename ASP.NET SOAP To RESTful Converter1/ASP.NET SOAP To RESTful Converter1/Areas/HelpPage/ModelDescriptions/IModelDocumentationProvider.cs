using System;
using System.Reflection;

namespace ASP.NET_SOAP_To_RESTful_Converter1.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}