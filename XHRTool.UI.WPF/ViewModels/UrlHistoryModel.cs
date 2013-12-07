using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using XHRTool.XHRLogic.Common;

namespace XHRTool.UI.WPF.ViewModels
{
    [Serializable]
    public class UrlHistoryModel
    {
        public UrlHistoryModel(string url, string verb)
        {
            Url = url;
            Verb = verb;
        }

        public string Url { get; set; }

        public string Verb { get; set; }
    }

    public class UrlHistoryModelEqualityComparer : IEqualityComparer<UrlHistoryModel>
    {
        public bool Equals(UrlHistoryModel x, UrlHistoryModel y)
        {
            return x.Url.Equals(y.Url, StringComparison.OrdinalIgnoreCase) && x.Verb.Equals(y.Verb, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(UrlHistoryModel obj)
        {
            return (obj.Verb.GetHashCode() + obj.Url.GetHashCode()) / 2;
        }
    }
}
