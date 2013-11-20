using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using XHRTool.Testing.Service.Models;

namespace XHRTool.Testing.Service.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }


        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [ActionName("the-post")]
        public HttpResponseMessage PostTest(TestPostModel model)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ObjectContent(typeof(Tuple<string, string>), new Tuple<string, string>("T1", "T2"), new JsonMediaTypeFormatter())
                };
        }
    }
}
