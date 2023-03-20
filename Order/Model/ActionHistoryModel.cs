using System.ComponentModel;

namespace Order.Model {
    public class ActionHistoryModel : INotifyPropertyChanged {

        private string _gridTitle;
        public string GridTitle {
            get => _gridTitle;
            set {
                if (_gridTitle == value)
                    return;
                _gridTitle = value;
                OnPropertyChanged(nameof(GridTitle));
            }
        }

        private string _callTime;
        public string CallTime {
            get => _callTime;
            set {
                if (_callTime == value)
                    return;
                _callTime = value;
                OnPropertyChanged(nameof(CallTime));
            }
        }

        private string _dataState;
        public string DataState {
            get => _dataState;
            set {
                if (_dataState == value)
                    return;
                _dataState = value;
                OnPropertyChanged(nameof(DataState));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
