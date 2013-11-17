using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.BLL.Common
{
    public class XHRRequestModel : ModelBase
    {
        public XHRRequestModel()
        {
            HttpHeaders = new List<HttpHeader>();
        }
        private HttpMethod _Verb;
        private string _Url;

        public HttpMethod Verb
        {
            get { return _Verb; }
            set 
            {
                if (_Verb == value) return;
                _Verb = value; 
                onPropertyChanged("Verb");
            }
        }

        public string Url
        {
            get { return _Url; }
            set 
            {
                if (_Url == value) return;
                _Url = value;
                onPropertyChanged("Url");
            }
        }

        public List<HttpHeader> HttpHeaders { get; set; }

        
    }
}
