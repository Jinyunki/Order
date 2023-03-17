using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
        private Thread thr;
        private Dictionary<int, IViewModelBase> _iViewModelBases = new Dictionary<int, IViewModelBase>();
        private IDispatcher _dispatcher;
        private const string ORDER_STRING = "Order";
        private const string CLEAR_STRING = "Clear";

        private string orderTime;
        private string clearTime;
        private string gridTitle;

        public MainViewModel(IDispatcher dispatcher) {
            initViewModelBases();
            initThread();
            _dispatcher = dispatcher;

        }

        private void initViewModelBases() {
            Labels = new string[9];
            for (int i = 0; i < 9; i++) {
                var viewModel = new IViewModelBase();
                _iViewModelBases[i] = viewModel;
                CurrentView.Add(viewModel);
                Labels[i] = $"AA{i + 1}호기";
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

        private void addStats() {
            thr = new Thread(new ThreadStart(initStats));
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
                    TcpClient acceptClient = listener.AcceptTcpClient();

                    var clientData = new ClientData(acceptClient);

                    BeginRead(clientData);
                }
            } catch (Exception e) {
                Console.WriteLine("AsyncServerStart Exception : " + e);
            }
        }

        private void BeginRead(ClientData clientData) {
            clientData.client.GetStream().BeginRead(clientData.readByteData, 0, clientData.readByteData.Length, ar => {
                try {
                    int bytesRead = clientData.client.GetStream().EndRead(ar);
                    ReadMsgNumber(clientData.clientNumber, Encoding.Default.GetString(clientData.readByteData, 0, bytesRead));
                    addGridThread();
                    addStats();
                    BeginRead(clientData);
                } catch (Exception e) {
                    Console.WriteLine("BeginRead Exception: " + e);
                }
            }, null);
        }

        public void ReadMsgNumber(int clientNumber, string readString) {
            int idx = clientNumber - 1;
            IViewModelBase viewModel = _iViewModelBases[idx];
            viewModel.ItemTitle = "AA" + clientNumber;

            if (readString == ORDER_STRING) {
                TotalOrder++;

                viewModel.OrderCount++; // realtime data
                viewModel.TotalOrder++; // stats data
                gridTitle = viewModel.ItemTitle;
                orderTime = DateTime.Now.ToString("hh:mm:tt");
                clearTime = "";

            } else if (readString == CLEAR_STRING) {
                TotalClear++;

                viewModel.OrderClearCount++; // realtime data
                viewModel.TotalClear++; // stats data
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
            // dispatcher로 Thread를 사용하여 CurrentView할당
            _dispatcher.Invoke(() => {
                for (int i = 1; i <= 9; i++) {
                    if (clientNumber == i) {
                        CurrentView[i - 1] = addViewItem;
                        break;
                    }
                }
            });

        }

        #region StatsView
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection {
            get { return _seriesCollection; }
            set {
                _seriesCollection = value;
                RaisePropertyChanged("SeriesCollection");
            }
        }
        public string[] Labels { get; set; }

        public void initStats() {
            ChartValues<ObservableValue> chartValues = new ChartValues<ObservableValue>();
            for (int i = 0; i < _iViewModelBases.Count; i++) {
                chartValues.Add(new ObservableValue(_iViewModelBases[i].TotalOrder));
            }
            _dispatcher.Invoke(() => {
                SeriesCollection = new SeriesCollection {
                new ColumnSeries {
                    Title = "Today Order Count",
                    Values = chartValues,
                    DataLabels = true, // enable data labels
                    LabelPoint = point => Labels[(int)point.X],
                    }
                };
            });
        }
        #endregion

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