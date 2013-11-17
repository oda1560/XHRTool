using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.BLL.Common
{
    public class HttpHeader : ModelBase
    {
        private string _Name;
        private string _Value;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value) return;
                _Name = value;
                onPropertyChanged("Name");
            }
        }
        public string Value
        {
            get { return _Value; }
            set
            {
                if (_Value == value) return;
                _Value = value;
                onPropertyChanged("Value");
            }
        }
    }
}
