using fiveFunction.Models;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Net;

namespace fiveFunction.Services
{
    public interface IHttpResponderService
    {
        HttpResponseData ProcessRequest(HttpRequestData httpRequest);
    }

    public class DefaultHttpResponderService : IHttpResponderService
    {
        public HttpResponseData ProcessRequest(HttpRequestData httpRequest)
        {
            var response = new HttpResponseData(HttpStatusCode.OK);
            var headers = new Dictionary<string, string>();
            headers.Add("Date", $"{DateTime.Now.ToLongTimeString()}");
            headers.Add("Content-Type", ContentType.Html);

            response.Headers = headers;
            response.Body = "I'm .Net 5, yo!.  O Brave New World!";

            return response;
        }
    }
}
