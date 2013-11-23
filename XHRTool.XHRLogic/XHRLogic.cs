using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using XHRTool.XHRLogic.Common;

namespace XHRTool.XHRLogic
{
    public class XHRLogicManager
    {
        public XHRResponseModel SendXHR(XHRRequestModel requestModel)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            var message = new HttpRequestMessage
                {
                    Method = requestModel.Verb,
                    RequestUri = new Uri(requestModel.Url, UriKind.Absolute)
                };
            if (requestModel.Content != null)
            {
                message.Content = new ObjectContent(typeof(string), requestModel.Content, new JsonMediaTypeFormatter());
            }
            requestModel.Headers.ForEach(h => message.Headers.TryAddWithoutValidation(h.Name, h.Value));

            try
            {
                var result = client.SendAsync(message).Result;
                var response = new XHRResponseModel
                {
                    Content = result.Content.ReadAsStringAsync().Result,
                    StatusCode = result.StatusCode,
                    HttpHeaders = result.Headers.Select(h => new HttpHeader(h.Key, h.Value.FirstOrDefault()) ).ToList()
                };
                return response;
            }
            catch (Exception ex)
            {
                return new XHRResponseModel
                {
                    Content = ex.ToString()
                };
            }
        }
    }
}
