using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using XHRTool.UI.WPF.ViewModels;
using XHRTool.XHRLogic;
using XHRTool.XHRLogic.Common;

namespace XHRTool.UI.WPF
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static RoutedCommand MakeRequestCommand = new RoutedCommand();
        public static RoutedCommand AboutCommand = new RoutedCommand();
        readonly XHRLogicManager xhrLogicManager = new XHRLogicManager();
        private XHRRequestViewModel _currentRequestViewModel;
        private XHRResponseModel _currentResponseViewModel;
        private string _notes;
        private string _headersSearchText;
        private ObservableCollection<UrlHistoryModel> _urlHistory;
        private readonly BinaryFormatter _formatter = new BinaryFormatter();
        readonly string _urlHistoryPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"XHRTool\UrlHistory.bin");
        readonly UrlHistoryModelEqualityComparer urlHistoryModelEqualityComparer = new UrlHistoryModelEqualityComparer();

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

        public string HeadersSearchText
        {
            get { return _headersSearchText; }
            set
            {
                if (_headersSearchText == value) return;
                _headersSearchText = value;
                OnPropertyChanged();
                filterHeaders();
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

        void filterHeaders()
        {
            if (string.IsNullOrWhiteSpace(HeadersSearchText))
            {
                requestHeadersGrid.ItemsSource = CurrentRequestViewModel.UIHeaders;
                return;
            }
            var resultCollection = new ObservableCollection<HttpHeaderViewModel>(from h in CurrentRequestViewModel.UIHeaders
                                                                                 where h.Name.ToLower().Contains(HeadersSearchText.ToLower()) ||
                                                                                 h.Value.ToLower().Contains(HeadersSearchText.ToLower())
                                                                                 select h).ToList();
            requestHeadersGrid.ItemsSource = resultCollection;
        }

        private void MakeRequestCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var currentUrlHistoryItem = getCurrentUrlHistoryModel();
            if (!UrlHistory.Contains(currentUrlHistoryItem, urlHistoryModelEqualityComparer))
            {
                UrlHistory.Insert(0, currentUrlHistoryItem);
                saveHistory();
            }
            CurrentRequestViewModel.Headers = CurrentRequestViewModel.UIHeaders.Where(h => h.IsSelected).Select(h => new HttpHeader(h.Name, h.Value)).ToList();
            _MainWindow.IsEnabled = false;
            new Action(() => CurrentResponseViewModel = xhrLogicManager.SendXHR(CurrentRequestViewModel)).BeginInvoke((ar =>
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => _MainWindow.IsEnabled = true))), null);
        }

        UrlHistoryModel getCurrentUrlHistoryModel()
        {
            return new UrlHistoryModel(CurrentRequestViewModel.UIUrl, CurrentRequestViewModel.SelectedAction);
        }

        void saveHistory()
        {
            try
            {
                var stream = new FileStream(_urlHistoryPath, FileMode.Create);
                _formatter.Serialize(stream, UrlHistory);
                stream.Flush();
                stream.Close();
                stream.Dispose();
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteLog(ex);
            }
        }

        ObservableCollection<UrlHistoryModel> loadUrlHistory()
        {
            try
            {
                if (!File.Exists(_urlHistoryPath))
                {
                    return new ObservableCollection<UrlHistoryModel>();
                }
                var collection = _formatter.Deserialize(File.Open(_urlHistoryPath, FileMode.Open)) as ObservableCollection<UrlHistoryModel>;
                return collection;
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteLog(ex);
                return new ObservableCollection<UrlHistoryModel>();
            }
        }

        private void MakeRequestCommand_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
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

        public ObservableCollection<UrlHistoryModel> UrlHistory
        {
            get
            {
                if (_urlHistory == null)
                {
                    _urlHistory = loadUrlHistory();
                    _urlHistory.CollectionChanged += (sender, args) => saveHistory();
                }
                return _urlHistory;
            }
            set
            {
                if (_urlHistory == value) return;
                _urlHistory = value;
                OnPropertyChanged();
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Notes = Notes;
            Properties.Settings.Default.Headers = CurrentRequestViewModel.TextViewHeaders;
            Properties.Settings.Default.Content = CurrentRequestViewModel.Content;
            Properties.Settings.Default.Save();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(_urlHistoryPath)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_urlHistoryPath));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.WriteLog(ex);
            }
            Notes = Properties.Settings.Default.Notes;
            CurrentRequestViewModel.TextViewHeaders = Properties.Settings.Default.Headers;
            CurrentRequestViewModel.Content = Properties.Settings.Default.Content;
        }

        private void AboutCommand_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var aboutDialog = new AboutDialog();
            aboutDialog.ShowDialog();
        }

        private void exp_Collapsed(object sender, RoutedEventArgs e)
        {
            if (sender == expRequestInfo)
            {
                rRequestInfo.Height = GridLength.Auto;
            }
            else if (sender == expResponseInfo)
            {
                rResponseInfo.Height = GridLength.Auto;
            }
        }

        private void exp_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender == expRequestInfo)
            {
                rRequestInfo.Height = new GridLength(.7, GridUnitType.Star);
            }
            else if (sender == expResponseInfo)
            {
                rResponseInfo.Height = new GridLength(1, GridUnitType.Star);
            }
        }

        private void cbUrl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(cbUrl.SelectedItem is UrlHistoryModel))
            {
                return;
            }
            CurrentRequestViewModel.SelectedAction = (cbUrl.SelectedItem as UrlHistoryModel).Verb;
        }
    }
}
