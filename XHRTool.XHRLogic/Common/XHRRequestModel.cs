using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private string _Content;
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

        public string Content
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

        public bool HasContent 
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Content);
            }
        }

        public static readonly List<string> CommonVerbs;
        public static readonly List<HttpHeader> CommonHeaders;

        static XHRRequestModel()
        {
            try
            {
                var xDoc = XDocument.Load("XHRHelpData.xml");
                var xVerbs = xDoc.Root.Element("Verbs").Elements("Verb").Select(e => e.Value).ToList();
                CommonVerbs = xVerbs;
                var xHeaders = xDoc.Root.Element("Headers").Elements("Header").Select(e => e).ToList();
                CommonHeaders = xHeaders.Select(xHeader => 
                    {
                        var header = new HttpHeader(xHeader.Attribute("Name").Value);
                        var xValues = xHeader.Element("Values");
                        if (xValues != null)
                        {
                            header.CommonValues = xValues.Elements("Value").DefaultIfEmpty().Select(e => e.Value).ToList();
                        }
                        return header;
                    }).ToList();
            }
            catch (Exception ex)
            {
                CommonVerbs = new List<string>();
                CommonHeaders = new List<HttpHeader>();
                ErrorLogger.WriteLog(ex);
            }
        }
    }
}
