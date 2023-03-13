using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Order.ViewModel {
    public class MainViewModel : IViewModelBase {
        private TcpListener listener;
        private Thread thr;
        private TcpClient client;
        private ClientData clientData;
        private Dictionary<int, IViewModelBase> _iViewModelBases = new Dictionary<int, IViewModelBase>();
        public Dictionary<int, ViewModelBase> _viewModels = new Dictionary<int, ViewModelBase>();
        public MainViewModel() {
            initViewModelBases();
            initThread();
        }

        private void initViewModelBases() {
            for (int i = 0; i < 9; i++) {
                _iViewModelBases[i] = new IViewModelBase();
                _viewModels[i] = _iViewModelBases[i];
            }
        }

        // ==== Tcp Item ====
        #region
        private void initThread() {
            thr = new Thread(new ThreadStart(AsyncServerStart));
            thr.IsBackground = true;
            thr.Start();
        }

        private void AsyncServerStart() {
            try {
                TcpListener listener = new TcpListener(new IPEndPoint(IPAddress.Any, 9999));
                listener.Start();
                Console.WriteLine("������ �����մϴ�.");
                while (true) {
                    TcpClient acceptClient = listener.AcceptTcpClient(); // ���۽� �۵�

                    ClientData clientData = new ClientData(acceptClient); // ���� ������ ������ �۵�

                    clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, new AsyncCallback(DataReceived), clientData);
                }
            } catch (Exception e) { }
        }

        private void DataReceived(IAsyncResult ar) {
            try {
                ClientData callbackClient = ar.AsyncState as ClientData;
                int bytesRead = callbackClient.client.GetStream().EndRead(ar);
                string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);

                ReadMsgNumber(callbackClient.clientNumber, readString);

                callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
            } catch (Exception e) {
            }
        }

        public void ReadMsgNumber(int clientNumber, string readString) {
            int idx = clientNumber - 1;
            IViewModelBase viewModel = _iViewModelBases[idx];
            viewModel.ItemTitle = "AA" + clientNumber;

            if (readString == "Order") {
                viewModel.OrderCount++;
            } else if (readString == "Clear") {
                viewModel.OrderClearCount++;
            }
            AddViewChange(viewModel, clientNumber);
        }

        public void AddViewChange(IViewModelBase viewModels, int clientNumber) {
            IViewModelBase addViewItem = new IViewModelBase(viewModels.ItemTitle, viewModels.OrderCount, viewModels.OrderClearCount);
            switch (clientNumber) {
                case 1:
                    CurrentViewModel = addViewItem;
                    break;
                case 2:
                    CurrentViewModel2 = addViewItem;
                    break;
                case 3:
                    CurrentViewModel3 = addViewItem;
                    break;
                case 4:
                    CurrentViewModel4 = addViewItem;
                    break;
                case 5:
                    CurrentViewModel5 = addViewItem;
                    break;
                case 6:
                    CurrentViewModel6 = addViewItem;
                    break;
                case 7:
                    CurrentViewModel7 = addViewItem;
                    break;
                case 8:
                    CurrentViewModel8 = addViewItem;
                    break;
                case 9:
                    CurrentViewModel9 = addViewItem;
                    break;
            }
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

        public ViewModelBase CurrentViewModel2 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel2");
                }
            }
        }

        public ViewModelBase CurrentViewModel3 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel3");
                }
            }
        }

        public ViewModelBase CurrentViewModel4 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel4");
                }
            }
        }

        public ViewModelBase CurrentViewModel5 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel5");
                }
            }
        }

        public ViewModelBase CurrentViewModel6 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel6");
                }
            }
        }

        public ViewModelBase CurrentViewModel7 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel7");
                }
            }
        }

        public ViewModelBase CurrentViewModel8 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel8");
                }
            }
        }

        public ViewModelBase CurrentViewModel9 {
            get {
                return _currentViewModel;
            }
            set {
                if (_currentViewModel != value) {
                    Set<ViewModelBase>(ref _currentViewModel, value);
                    _currentViewModel.RaisePropertyChanged("CurrentViewModel9");
                }
            }
        }

        #endregion
    }
}