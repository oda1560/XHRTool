﻿using System;
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

        public MainWindow()
        {
            InitializeComponent();
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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var returnMessage = xhrLogicManager.SendXHR(CurrentRequestViewModel);
            MessageBox.Show(returnMessage.ToString());
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var urlValid = false;
            
            if (!string.IsNullOrWhiteSpace(CurrentRequestViewModel.UIUrl))
            {
                var tempUrl = string.Empty;
                tempUrl = Uri.IsWellFormedUriString(CurrentRequestViewModel.UIUrl, UriKind.RelativeOrAbsolute) ? 
                    CurrentRequestViewModel.UIUrl :
                    Uri.EscapeUriString(CurrentRequestViewModel.UIUrl);
                
                
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
    }
}
