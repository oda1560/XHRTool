using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XHRTool.UI.WPF.ViewModels;
using XHRTool.XHRLogic;
using XHRTool.XHRLogic.Common;

namespace XHRTool.UI.WPF
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static RoutedCommand MakeRequestCommand = new RoutedCommand();
        readonly XHRLogicManager xhrLogicManager = new XHRLogicManager();
        private XHRRequestViewModel _currentRequestViewModel;
        private XHRResponseModel _currentResponseViewModel;
        private string _notes;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string Notes
        {
            get { return _notes; }
            set 
            {
                if (_notes == value) return;
                _notes = value; 
                OnPropertyChanged();
            }
        }

        public XHRRequestViewModel CurrentRequestViewModel
        {
            get { return _currentRequestViewModel ?? (_currentRequestViewModel = new XHRRequestViewModel()); }
            set 
            {
                if (_currentRequestViewModel == value) return;
                _currentRequestViewModel = value;
                OnPropertyChanged();
            }
        }

        public XHRResponseModel CurrentResponseViewModel
        {
            get { return _currentResponseViewModel; }
            set
            {
                if (_currentResponseViewModel == value) return;
                _currentResponseViewModel = value;
                OnPropertyChanged();
            }
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentRequestViewModel.Headers = CurrentRequestViewModel.UIHeaders.Where(h => h.IsSelected).Select(h => new HttpHeader(h.Name, h.Value)).ToList();
            CurrentResponseViewModel = xhrLogicManager.SendXHR(CurrentRequestViewModel);
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var urlValid = false;

            if (!string.IsNullOrWhiteSpace(CurrentRequestViewModel.UIUrl))
            {
                if (Uri.IsWellFormedUriString(CurrentRequestViewModel.UIUrl, UriKind.Absolute) && (CurrentRequestViewModel.UIUrl.ToLower().StartsWith("http") || CurrentRequestViewModel.UIUrl.ToLower().StartsWith("https")))
                {
                    urlValid = true;
                }
                else
                {
                    if (!CurrentRequestViewModel.UIUrl.ToLower().StartsWith("http") && !CurrentRequestViewModel.UIUrl.ToLower().StartsWith("https"))
                    {
                        if (Uri.IsWellFormedUriString(Uri.UriSchemeHttp + Uri.SchemeDelimiter + Uri.EscapeUriString(CurrentRequestViewModel.UIUrl), UriKind.Absolute))
                        {
                            var schemeUri = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + Uri.EscapeUriString(CurrentRequestViewModel.UIUrl), UriKind.RelativeOrAbsolute);
                            if (schemeUri.IsAbsoluteUri)
                            {
                                CurrentRequestViewModel.Url = schemeUri.ToString();
                                urlValid = true;
                            }
                        }
                    }
                }
            }
            var actionValid = !string.IsNullOrWhiteSpace(CurrentRequestViewModel.SelectedAction);
            e.CanExecute = urlValid && actionValid;
        }


        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Temp_LastUsedUrl = CurrentRequestViewModel.UIUrl;
            Properties.Settings.Default.Notes = Notes;
            Properties.Settings.Default.Headers = CurrentRequestViewModel.TextViewHeaders;
            Properties.Settings.Default.Content = CurrentRequestViewModel.Content;
            Properties.Settings.Default.Save();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentRequestViewModel.UIUrl = Properties.Settings.Default.Temp_LastUsedUrl;
            Notes = Properties.Settings.Default.Notes;
            CurrentRequestViewModel.TextViewHeaders = Properties.Settings.Default.Headers;
            CurrentRequestViewModel.Content = Properties.Settings.Default.Content;
        }
    }
}
