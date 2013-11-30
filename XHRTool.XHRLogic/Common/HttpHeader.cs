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

        public HttpHeader(string name)
        {
            Name = name;
        }
        public HttpHeader(string name, string value)
            : this(name)
        {
            Value = value;
        }

        public HttpHeader(string name, List<string> commonValues)
            : this(name)
        {
            CommonValues = commonValues;
        }

        private string _Name;
        private string _Value;
        private List<string> _CommonValues;

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

        public List<string> CommonValues
        {
            get { return _CommonValues; }
            set
            {
                if (_CommonValues == value) return;
                _CommonValues = value;
                onPropertyChanged();
            }
        }
        public bool? IsValid()
        {
            return !string.IsNullOrWhiteSpace(Value);
        }
    }
}
