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
using XHRTool.BLL.Common;

namespace XHRTool.BLL
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
                //("{Value1 : Test1, Value2 : Test2}")
                message.Content = new ObjectContent(typeof(string), "{Value1 : Test1, Value2 : Test2}", new JsonMediaTypeFormatter());
            }
            requestModel.Headers.ForEach(h => message.Headers.Add(h.Name, h.Value));

            var result = client.SendAsync(message).Result;

            var response = new XHRResponseModel
            {
                Content = result.Content.ReadAsStringAsync().Result,
                StatusCode = result.StatusCode
            };

            return response;
        }

    }
}
