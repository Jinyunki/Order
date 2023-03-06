using System.ComponentModel;
using System.Windows;

namespace Order.Model {
    public class MainModel : INotifyPropertyChanged {

        string setText;
        public string SetText {
            get { return setText; }
            set {
                if (setText == value) {
                    return;
                }
                setText = value;
                OnPropertyChanged("SetText");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
