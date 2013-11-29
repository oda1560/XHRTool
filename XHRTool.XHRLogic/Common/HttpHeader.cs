using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace XHRTool.XHRLogic.Common
{
    [DebuggerDisplay("HttpHeader {Name}: {Value}")]
    public class HttpHeader : ModelBase
    {
        public HttpHeader()
        {
            
        }
        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }
        private string _Name;
        private string _Value;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                onPropertyChanged();
            }
        }
        public string Value
        {
            get { return _Value; }
            set
            {
                if (_Value == value) return;
                _Value = value;
                onPropertyChanged();
            }
        }
        HttpRequestMessage validationMessage = new HttpRequestMessage(); 
        public bool? IsValid()
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return false;
            }
            // HACK: need a way to do this property without throwing exceptions
            // and having a dummy http request message class
            try
            {
                validationMessage.Headers.Add(Name, Value);
                return true;
            }
            catch
            {
                validationMessage.Headers.TryAddWithoutValidation(Name, Value);
                return null;
            }
        }
    }
}
