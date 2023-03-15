using System.ComponentModel;

namespace Order.Model {
    public class MainModel : INotifyPropertyChanged {

        

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

        private string _orderTime;
        public string OrderTime {
            get => _orderTime;
            set {
                if (_orderTime == value)
                    return;
                _orderTime = value;
                OnPropertyChanged(nameof(OrderTime));
            }
        }

        private string _orderClearTime;
        public string OrderClearTime {
            get => _orderClearTime;
            set {
                if (_orderClearTime == value)
                    return;
                _orderClearTime = value;
                OnPropertyChanged(nameof(OrderClearTime));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
