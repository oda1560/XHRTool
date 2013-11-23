using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.XHRLogic.Common
{
    public class XHRRequestModel : ModelBase
    {
        public XHRRequestModel()
        {
            Headers = new List<HttpHeader>();
        }
        private HttpMethod _Verb;
        private string _Url;
        private object _Content;
        private List<HttpHeader> _headers;

        public HttpMethod Verb
        {
            get { return _Verb; }
            set 
            {
                if (_Verb == value) return;
                _Verb = value; 
                onPropertyChanged();
            }
        }

        public string Url
        {
            get { return _Url; }
            set 
            {
                if (_Url == value) return;
                _Url = value;
                onPropertyChanged();
            }
        }

        public object Content
        {
            get { return _Content; }
            set
            {
                if (_Content == value) return;
                _Content = value;
                onPropertyChanged();
            }
        }

        public List<HttpHeader> Headers
        {
            get { return _headers; }
            set
            {
                if (_headers == value) return;
                _headers = value;
                onPropertyChanged();
            }
        }

        public static readonly List<string> CommonVerbs;
        public static readonly List<HttpHeader> CommonHeaders;

        static XHRRequestModel()
        {
            CommonVerbs = new List<string>(typeof(HttpMethod).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(p => p.PropertyType == typeof(HttpMethod)).Select(p => p.Name.ToUpper()).ToList());
            CommonHeaders = typeof(HttpRequestHeaders).GetProperties().Select(p => new HttpHeader(p.Name, string.Empty)).ToList();
        }
    }
}
