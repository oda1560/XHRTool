using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.BLL
{
    public class XHRLogicManager
    {


        public void Send()
        {
            var httpClient = new HttpClient();
            var response = httpClient.GetAsync("http://localhost:2032/api/values").Result;
            var content = response.Content.ReadAsStringAsync();
            
        }

    }
}
