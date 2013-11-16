using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.BLL.Common
{
    public class XHRRequestModel : ModelBase
    {
        private string _verb;
        private string _url;

        public string Verb
        {
            get { return _verb; }
            set 
            {
                if (_verb != value)
                {
                    _verb = value; 
                    onPropertyChanged("Verb");
                }
            }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public List<HttpHeader> HttpHeaders { get; set; }


        
    }
}
