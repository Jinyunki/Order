using GalaSoft.MvvmLight;
using Order.Utiles;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Order.ViewModel {
    public class SideBarViewModel : ViewModelBase {
        private ViewModelLocator _locator = new ViewModelLocator();
        public SideBarViewModel() {
            _messageBoxService = new MessageBoxService();
            initButtonEvent();
            initTCP();
        }
        // === Visibllity Item === 
        #region
        private bool _isVisible;
        public bool IsVisibleContent {
            get { return _isVisible; }
            set {
                if (_isVisible != value) {
                    _isVisible = value;
                    RaisePropertyChanged("IsVisibleContent");
                }
            }
        }

        public bool IsVisibleGrid {
            get { return _isVisible; }
            set {
                if (_isVisible != value) {
                    _isVisible = value;
                    RaisePropertyChanged("IsVisibleGrid");
                }
            }
        }
        #endregion // === Visibllity Item === 

        // === Thread ( TCP ) === 
        #region
        private Thread thr;
        private TcpListener listener;
        private TcpClient client;

        public void initTCP() {
            thr = new Thread(new ThreadStart(ListenRequests));
            thr.IsBackground = true;
            thr.Start();
        }

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
                                    initMsgCatch(Message);
                                    Console.WriteLine(msgClient);

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

        #endregion // === Thread(TCP ) === 

        // === AAView Interaction ===
        #region

        // Client Message Func
        private string _message;
        public string Message {
            get { return _message; }
            set {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        // Client OrderCount
        private int _count = 0;
        public int Count {
            get { return _count; }
            set {
                _count = value;
                OnPropertyChanged(nameof(Count));

            }
        }
        #endregion
        // === CurrentViewChange ===
        #region
        // CurrentViewChange Func
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

        // AAViewModel Locator Func
        #region
        private void SetViewModelProperty(ref ViewModelBase backingField, ViewModelBase value, string propertyName) {
            if (backingField != value) {
                backingField = value;
                RaisePropertyChanged(propertyName);
            }
        }
        public ViewModelBase AA1 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA1));
        }
        public ViewModelBase AA2 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA2));
        }
        public ViewModelBase AA3 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA3));
        }
        public ViewModelBase AA4 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA4));
        }
        public ViewModelBase AA5 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA5));
        }
        public ViewModelBase AA6 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA6));
        }
        public ViewModelBase AA7 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA7));
        }
        public ViewModelBase AA8 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA8));
        }
        public ViewModelBase AA9 {
            get => _currentViewModel;
            set => SetViewModelProperty(ref _currentViewModel, value, nameof(AA9));
        }
        #endregion

        public void initMsgCatch(string msg) {
            IsVisibleContent = false;
            IsVisibleGrid = true;
            switch (msg) {
                case "AA1":
                    //ShowMessage(Message);
                    //Count++; // 호출될때마다 카운트가 1씩증가 > 실시간 반영방법 ?
                    AA1 = _locator.AA1ViewModel;
                    break;

                case "AA2":
                    //ShowMessage(Message);
                    AA2 = _locator.AA2ViewModel;
                    break;

                case "AA3":
                    //ShowMessage(Message);
                    AA3 = _locator.AA1ViewModel;
                    break;

                case "AA4":
                    //ShowMessage(Message);
                    AA4 = _locator.AA2ViewModel;
                    break;

                case "AA5":
                    //ShowMessage(Message);
                    AA5 = _locator.AA1ViewModel;
                    break;

                case "AA6":
                    //ShowMessage(Message);
                    AA6 = _locator.AA2ViewModel;
                    break;

                case "AA7":
                    //ShowMessage(Message);
                    AA7 = _locator.AA1ViewModel;
                    break;

                case "AA8":
                    //ShowMessage(Message);
                    AA8 = _locator.AA2ViewModel;
                    break;

                case "AA9":
                    //ShowMessage(Message);
                    AA9 = _locator.AA1ViewModel;
                    break;
            }
        }
        #endregion // === CurrentViewChange ===

        // === ButtonEvent ===
        #region

        private Command<RealTimeViewModel> _btnRealTime;
        private Command<ActionHistoryViewModel> _btnActionHistor;
        // Button Type Event
        public RelayCommand<string> ViewChangeCommand {
            get {
                return new RelayCommand<string>((confType) => {
                    if (confType == "RealTimeViewModel") {
                        IsVisibleContent = true;
                        IsVisibleGrid = false;
                        CurrentViewModel = _locator.RealTimeViewModel;
                    } else if (confType == "ActionHistoryViewModel") {
                        IsVisibleContent = true;
                        IsVisibleGrid = false;
                        CurrentViewModel = _locator.ActionHistoryViewModel;
                    } else if (confType == "MainGoBack") {
                        IsVisibleContent = false;
                        IsVisibleGrid = true;
                    }
                });
            }
        }

        // Event Method
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
        #endregion // === ButtonEvent ===

        // === Utile ===
        #region
        private IMessageBoxService _messageBoxService;
        public void ShowMessage(string message) {
            _messageBoxService.Show(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        #endregion // === Utile ===

    }
}
