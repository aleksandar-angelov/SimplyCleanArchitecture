using System.Web;

namespace Aspekt.KICBIntegrations.Infrastructure.HttpClient;

public static class HttpClientHelper
{
    public static string GetQueryString(object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select p.Name.ToLower() + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

        return String.Join("&", properties.ToArray());
    }
}
