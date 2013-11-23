using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XHRTool.XHRLogic.Common;

namespace XHRTool.UI.WPF.ViewModels
{
    public class HttpHeaderViewModel : HttpHeader
    {
        public HttpHeaderViewModel()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Value")
            {
                var isValid = IsValid();
                IsSelected = isValid != false;
                IsInvalidValue = !isValid.HasValue;
                // TODO: create data trigger to show yellow in grid
            }
        }

        private bool _IsInvalidValue;

        public bool IsInvalidValue
        {
            get { return _IsInvalidValue; }
            set
            {
                if (_IsInvalidValue == value) return;
                _IsInvalidValue = value;
                onPropertyChanged();
            }
        }

        public HttpHeaderViewModel(HttpHeader baseHeader)
        {
            Value = baseHeader.Value;
            Name = baseHeader.Name;
            PropertyChanged += OnPropertyChanged;
        }
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected == value) return;
                _IsSelected = value;
                onPropertyChanged();
            }
        }
    }
}
