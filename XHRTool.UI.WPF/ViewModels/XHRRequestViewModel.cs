using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public ObservableCollection<string> CommonActions
        {
            get
            {
                return new ObservableCollection<string>(CommonVerbs);
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

        private ObservableCollection<HttpHeaderViewModel> _UIHeaders;

        public ObservableCollection<HttpHeaderViewModel> UIHeaders
        {
            get 
            {
                return _UIHeaders ?? (_UIHeaders = new ObservableCollection<HttpHeaderViewModel>(CommonHeaders.Select(h => new HttpHeaderViewModel(h)).ToList()));
            }
            set
            {
                if (_UIHeaders == value) return;
                _UIHeaders = value;
                onPropertyChanged();
            }
        }

        // TODO store usual values for headers
    }
}
