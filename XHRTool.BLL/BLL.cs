using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XHRTool.BLL.Common;

namespace XHRTool.BLL
{
    public class XHRLogicManager
    {
        public XHRResponseModel SendXHR(XHRRequestModel requestModel)
        {
            var client = new HttpClient();
            var message = new HttpRequestMessage
                {
                    Method = requestModel.Verb,
                    RequestUri = new Uri(requestModel.Url, UriKind.Absolute)
                };
            message.Headers.Clear();
            requestModel.HttpHeaders.ForEach(h => message.Headers.Add(h.Name, h.Value));
            var result = client.SendAsync(message).Result;
            
            var response =  new XHRResponseModel
            {
                Content = result.Content.ReadAsStringAsync().Result,
                StatusCode = result.StatusCode
            };

            return response;
        }

    }
}
