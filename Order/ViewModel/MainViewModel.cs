using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        //public Dictionary<int, ViewModelBase> _viewModels = new Dictionary<int, ViewModelBase>();
        public MainViewModel() {
            initViewModelBases();
            initThread();
        }

        private void initViewModelBases() {
            for (int i = 0; i < 9; i++) {
                var viewModel = new IViewModelBase();
                _iViewModelBases[i] = viewModel;
                //_viewModels[i] = _iViewModelBases[i];
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
                Console.WriteLine("서버를 시작합니다.");
                while (true) {
                    TcpClient acceptClient = listener.AcceptTcpClient(); // 시작시 작동

                    ClientData clientData = new ClientData(acceptClient); // 연결 응답이 왔을때 작동

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


        public ObservableCollection<IViewModelBase> DataGridItem { get; set; }
        public void ReadMsgNumber(int clientNumber, string readString) {
            int idx = clientNumber - 1;
            IViewModelBase viewModel = _iViewModelBases[idx];
            viewModel.ItemTitle = "AA" + clientNumber;

            if (readString == "Order") {
                viewModel.OrderCount++;
                viewModel.OrderTime = DateTime.Now.ToString("hh:mm:tt");
                viewModel.ItemTitle = viewModel.ItemTitle;
            } else if (readString == "Clear") {
                viewModel.OrderClearCount++;
                viewModel.OrderClearTime = DateTime.Now.ToString("hh:mm:tt");
                viewModel.ItemTitle = viewModel.ItemTitle;
            }

            AddViewChange(viewModel, clientNumber);

            // 2023.03.13 스터디 종료 , 현재 아래 코드 현재 하나만 구현가능 반복적인 작업 수행하기위한 코드 진행해야함 

            DataGridItem = new ObservableCollection<IViewModelBase>() {
                new IViewModelBase{ ItemTitle = viewModel.ItemTitle, OrderTime = viewModel.OrderTime , OrderClearTime = viewModel.OrderClearTime }
            };
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