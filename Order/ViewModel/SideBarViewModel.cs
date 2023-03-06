using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Order.ViewModel {
    public class SideBarViewModel : ViewModelBase {
        private Command<RealTimeViewModel> _btnRealTime;
        private Command<ActionHistoryViewModel> _btnActionHistor;
        private Command<RealTimeViewModel> refreshAuto;
        private ViewModelLocator _locator = new ViewModelLocator();
        private Thread thr;
        private TcpListener listener;
        private TcpClient client;

        public SideBarViewModel() {
            initTCP();
            initButtonEvent();
        }

        // Click Event ViewChnager
        public RelayCommand<string> ViewChangeCommand {
            get {
                return new RelayCommand<string>((confType) => {
                    if (confType == "RealTimeViewModel") {
                        CurrentViewModel = _locator.RealTimeViewModel;
                    } else if (confType == "ActionHistoryViewModel") {
                        CurrentViewModel = _locator.ActionHistoryViewModel;
                    }
                });
            }
        }

        // Client Message
        private string _message;
        public string Message {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        // thread 작동
        public void initTCP() {
            thr = new Thread(new ThreadStart(ListenRequests));
            thr.IsBackground = true;
            thr.Start();
        }
        // tcp통신
        private void ListenRequests() {
            try {
                listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1991);
                listener.Start();
                Byte[] bytes = new Byte[1024];
                while (true) {
                    using (client = listener.AcceptTcpClient()) {
                        using (NetworkStream stream = client.GetStream()) {
                            int length;
                            try {
                                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
                                    var incomingData = new Byte[length];
                                    Array.Copy(bytes, 0, incomingData, 0, length);
                                    string msgClient = Encoding.UTF8.GetString(incomingData);
                                    Message = msgClient;
                                    CurrentViewModel = _locator.RealTimeViewModel;
                                }
                            } catch (Exception e) {
                                client.Close();
                            }
                        }
                    }
                }
            } catch (SocketException se) {
                Console.WriteLine("Socket exception " + se.ToString());
            }
        }

        // button Event
        private void initButtonEvent() {
            _btnRealTime = new Command<RealTimeViewModel>(CommandExecute, CommandCanExecute);
            _btnActionHistor = new Command<ActionHistoryViewModel>(CommandExecuteAction, CommandCanExecute);
        }

        private void CommandExecuteAction(ActionHistoryViewModel obj) {
            this.CurrentViewModel = obj;
        }

        private void CommandExecute(RealTimeViewModel obj) {
            this.CurrentViewModel = obj;
        }

        private bool CommandCanExecute(object param) {
            return true;
        }
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
