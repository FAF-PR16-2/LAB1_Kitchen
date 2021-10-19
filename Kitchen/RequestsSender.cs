using System;
using System.Net;
using System.Text.Json;

namespace Kitchen
{
    public static class RequestsSender
    {
        public static void SendOrderRequest(DistributionData distributionData)
        {
            using (var client = new WebClient())
            {
                var dataString = JsonSerializer.Serialize(distributionData);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.UploadString(new Uri("http://host.docker.internal:80/distribution"), "POST", dataString);
            }
        }
    }
}