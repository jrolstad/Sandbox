using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fiveFunction.Services
{
    public interface IHttpResponderService
    {
        HttpResponseData ProcessRequest(HttpRequestData httpRequest);
    }

    public class DefaultHttpResponderService : IHttpResponderService
    {
        public DefaultHttpResponderService()
        {

        }

        public HttpResponseData ProcessRequest(HttpRequestData httpRequest)
        {
            var response = new HttpResponseData(HttpStatusCode.OK);
            var headers = new Dictionary<string, string>();
            headers.Add("Date", "Mon, 18 Jul 2016 16:06:00 GMT");
            headers.Add("Content", "Content - Type: text / html; charset = utf - 8");

            response.Headers = headers;
            response.Body = "Welcome to .NET 5!!";

            return response;
        }
    }
}
