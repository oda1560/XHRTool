using System;
using System.Collections.Generic;
using System.IO;
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
                Verb = new HttpMethod(SelectedAction);
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

        private string _UIUrl;

        public string UIUrl
        {
            get { return _UIUrl; }
            set
            {
                if (_UIUrl == value) return;
                _UIUrl = value;
                onPropertyChanged();
                Url = value;
            }
        }

        static XHRRequestViewModel()
        {
            _commonActions = typeof(HttpMethod).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(p => p.PropertyType == typeof(HttpMethod)).Select(p => p.Name.ToUpper()).ToList();
        }
    }
}
