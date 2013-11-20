using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.XHRLogic.Common
{
    public class XHRResponseModel : ModelBase
    {
        private HttpStatusCode _statusCode;
        private string _content;

        public XHRResponseModel()
        {
            HttpHeaders = new List<HttpHeader>();
        }
        public List<HttpHeader> HttpHeaders { get; set; }

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
            set
            {
                if (_statusCode == value) return;
                _statusCode = value;
                onPropertyChanged("StatusCode");
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                onPropertyChanged("Content");
            }
        }
    }
}
