﻿using System;
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
        private string _statusMessage;
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
                onPropertyChanged();
                onPropertyChanged("Summary");
            }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                onPropertyChanged();
                onPropertyChanged("Summary");
            }
        }

        public string Summary
        {
            get
            {
                return ToString();
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                onPropertyChanged();
                onPropertyChanged("Summary");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Return Code: {0}", (int)StatusCode));
            sb.AppendLine(string.Format("Return Message: {0}", StatusMessage));
            sb.AppendLine("Headers");
            HttpHeaders.ForEach(header => sb.AppendLine(string.Format("{0}: {1}", header.Name, header.Value)));
            sb.AppendLine(string.Format("Content: {0}", Content));
            return sb.ToString();
        }
    }
}
