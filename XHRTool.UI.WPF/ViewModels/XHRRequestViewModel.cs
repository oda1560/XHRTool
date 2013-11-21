using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XHRTool.XHRLogic.Common;

namespace XHRTool.UI.WPF.ViewModels
{
    public class XHRRequestViewModel : XHRRequestModel
    {
        private string _selectedAction;

        public string SelectedAction
        {
            get { return _selectedAction; }
            set 
            {
                if (_selectedAction == value) return;
                _selectedAction = value;
                Verb = HttpMethod.Get;
                onPropertyChanged();
            }
        }
        static readonly List<string> _commonActions;
        public List<string> CommonActions
        {
            get
            {
                return _commonActions;
            }
        }

        static XHRRequestViewModel()
        {
            _commonActions = typeof(HttpMethod).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(p => p.PropertyType == typeof(HttpMethod)).Select(p => p.Name.ToUpper()).ToList();
        }
    }
}
