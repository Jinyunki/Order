using System;
using System.ComponentModel;

namespace Order.Model {
    class UnderBarModel : INotifyPropertyChanged {
        private string _facilities = "A/A 100Class";
        public string Facilities {
            set {
                _facilities = value;
                OnPropertyChanged("Facilities");
            }
            get { return _facilities; }
        }
        private string _date = DateTime.Now.ToString(" hh:mm tt");
        public string DateTiem {
            set {
                _date = value;
                OnPropertyChanged("DateTime");
            }
            get { return _date; } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
