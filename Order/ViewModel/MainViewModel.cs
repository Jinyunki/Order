using Order.Model;
using Order.Utiles;
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
        private IDispatcher _dispatcher;
        private ViewModelLocator _locator = new ViewModelLocator();
        public MainViewModel(IDispatcher dispatcher) {
            initViewModelBases();
            initThread();
            _dispatcher = dispatcher;
        }

        private void initViewModelBases() {
            for (int i = 0; i < 9; i++) {
                var viewModel = new IViewModelBase();
                _iViewModelBases[i] = viewModel;
                CurrentView.Add(viewModel);
            }
        }
        // Main 실시간 화면
        private void initThread() {
            thr = new Thread(new ThreadStart(AsyncServerStart));
            thr.IsBackground = true;
            thr.Start();
        }
        // 조치내역
        private void addGridThread() {
            thr = new Thread(new ThreadStart(AddDataGrid));
            thr.IsBackground = true;
            thr.Start();
        }
        

        #region TCP ITEM
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
                addGridThread();

                callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
            } catch (Exception e) {
            }
        }

        public void ReadMsgNumber(int clientNumber, string readString) {
            int idx = clientNumber - 1;
            IViewModelBase viewModel = _iViewModelBases[idx];
            viewModel.ItemTitle = "AA" + clientNumber;

            if (readString == "Order") {
                TotalOrder++;

                viewModel.OrderCount++;
                gridTitle = viewModel.ItemTitle;
                orderTime = DateTime.Now.ToString("hh:mm:tt");
                clearTime = "";

            } else if (readString == "Clear") {
                TotalClear++;

                viewModel.OrderClearCount++;
                gridTitle = viewModel.ItemTitle;
                clearTime = DateTime.Now.ToString("hh:mm:tt");
                orderTime = "";
            }

            AddViewChange(viewModel, clientNumber);
        }
        

        public void AddViewChange(IViewModelBase viewModels, int clientNumber) {
            IViewModelBase addViewItem = new IViewModelBase {
                ItemTitle = viewModels.ItemTitle,
                OrderCount = viewModels.OrderCount,
                OrderClearCount = viewModels.OrderClearCount
            };

            // 들어온 clientNumber값과 포문의 인트값이 동일할때 CurrentView[index-1] = gridView갱신
            for (int i = 1; i <= 9; i++) {
                if (clientNumber == i) {
                    CurrentView[i - 1] = addViewItem;
                    break;
                }
            }
        }


        #region DataGrid ActionHistory
        private ObservableCollection<MainModel> _dataGridItem = null;
        public ObservableCollection<MainModel> DataGridItem {
            get {
                if (_dataGridItem == null) {
                    _dataGridItem = new ObservableCollection<MainModel>();
                }
                return _dataGridItem;
            }
            set {
                _dataGridItem = value;
            }
        }

        private string orderTime;
        private string clearTime;
        private string gridTitle;
        public void AddDataGrid() {
            MainModel model = new MainModel();
            model.OrderTime = orderTime;
            model.OrderClearTime = clearTime;
            model.GridTitle = gridTitle;
            _dispatcher.Invoke(() => {
                DataGridItem.Add(model);
            });
        }
        #endregion

        #region RealTime Current
        private ObservableCollection<IViewModelBase> _currentView = null;
        public ObservableCollection<IViewModelBase> CurrentView {
            get {
                if (_currentView == null) {
                    _currentView = new ObservableCollection<IViewModelBase>();
                }
                return _currentView;
            }
            set {
                _currentView = value;
            }
        }
        #endregion

        #endregion

    }
}