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
using System.Text.RegularExpressions;
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
                if (_selectedAction == value || string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, "([a-z]|[0-9])+", RegexOptions.IgnoreCase)) return;
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


        public string TextViewHeaders
        {
            get 
            { 
                return headersCollectionToString(UIHeaders);
            }
            set
            {
                updateHeadersCollection(value);
                onPropertyChanged();
            }
        }

        string headersCollectionToString(ObservableCollection<HttpHeaderViewModel> headersCollection)
        {
            var sb = new StringBuilder();
            headersCollection.Where(h => h.IsSelected).ToList().ForEach(h => sb.AppendFormat("{0}{1}: {2}", Environment.NewLine, h.Name, h.Value));
            return sb.ToString().Trim();
        }

        void updateHeadersCollection(string headersText)
        {
            var headerLines = headersText.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            UIHeaders.ToList().ForEach(h => 
                {
                    h.IsSelected = false;
                    h.Value = string.Empty;
                });
            headerLines.ForEach(l => 
                {
                    if (!l.Contains(":"))
                    {
                        return;
                    }
                    var headerData = l.Split(new [] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (headerData.Length >= 2)
                    {
                        var existingHeader = UIHeaders.FirstOrDefault(h => h.Name.Equals(headerData[0].Trim(), StringComparison.OrdinalIgnoreCase));
                        if (existingHeader != null)
                        {
                            existingHeader.IsSelected = true;
                            existingHeader.Value = string.Join(string.Empty, headerData.Skip(1)).Trim();
                        }
                        else
                        {
                            var newHeader = new HttpHeaderViewModel { Name = headerData[0].Trim(), Value = string.Join(string.Empty, headerData.Skip(1)).Trim(), IsSelected = true };
                            UIHeaders.Insert(0, newHeader);
                        }
                    }
                });
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
                if (_UIHeaders == null)
                {
                    var headersList = CommonHeaders.Select(h => new HttpHeaderViewModel(h)).ToList();
                    _UIHeaders = new ObservableCollection<HttpHeaderViewModel>(headersList);
                    headersList.ForEach(h => h.PropertyChanged += (sender, args) => onPropertyChanged("TextViewHeaders"));
                    _UIHeaders.CollectionChanged += (sender, args) => 
                        args.NewItems.OfType<INotifyPropertyChanged>().ToList().ForEach(item => item.PropertyChanged += (o, eventArgs) => onPropertyChanged("TextViewHeaders"));
                }
                return _UIHeaders;
            }
            set
            {
                if (_UIHeaders == value) return;
                _UIHeaders = value;
                onPropertyChanged();
                onPropertyChanged("TextViewHeaders");
            }
        }
    }
}
