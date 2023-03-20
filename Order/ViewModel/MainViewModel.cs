using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Order.Model;
using Order.Utiles;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Order.ViewModel {

    public class MainViewModel : MainModel {
        private Thread thr;
        private Dictionary<int, MainModel> _mainModelBases = new Dictionary<int, MainModel>();
        private IDispatcher _dispatcher;
        private const string ORDER_STRING = "Order";
        private const string CLEAR_STRING = "Clear";

        private string _callTime;
        private string _dataState;
        private string _gridTitle;

        public MainViewModel(IDispatcher dispatcher) {
            ViewCurrentBases();
            TcpThread();
            _dispatcher = dispatcher;
        }

        private void ViewCurrentBases() {
            Labels = new string[9];
            for (int i = 0; i < 9; i++) {
                var viewModel = new MainModel();
                _mainModelBases[i] = viewModel;
                CurrentView.Add(viewModel);
                Labels[i] = $"AA{i + 1}호";
            }
        }
        // 실시간 Thread
        private void TcpThread() {
            thr = new Thread(new ThreadStart(AsyncServerStart));
            thr.IsBackground = true;
            thr.Start();
        }
        // 조치내역 Thread
        private void ActionHistoryThread() {
            thr = new Thread(new ThreadStart(ActionHistoryData));
            thr.IsBackground = true;
            thr.Start();
        }
        // 통계 Thread
        private void StatsThread() {
            thr = new Thread(new ThreadStart(StatsData));
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
                    ActionHistoryThread();
                    StatsThread();
                    BeginRead(clientData);
                } catch (Exception e) {
                    Console.WriteLine("BeginRead Exception: " + e);
                }
            }, null);
        }

        public string SetColor { get; set; }

        #region ClientMsgCatch
        public void ReadMsgNumber(int clientNumber, string readString) {
            int idx = clientNumber - 1;
            MainModel viewModel = _mainModelBases[idx];
            viewModel.ItemTitle = "AA" + clientNumber;

            if (readString == ORDER_STRING) {
                viewModel.OrderCount++; // realtime data
                viewModel.TotalOrder++; // stats data
                _gridTitle = viewModel.ItemTitle;
                _callTime = DateTime.Now.ToString("hh:mm:tt");
                _dataState = "대기중";

            } else if (readString == CLEAR_STRING) {
                viewModel.OrderClearCount++; // realtime data
                viewModel.TotalClear++; // stats data
                _gridTitle = viewModel.ItemTitle;
                _callTime = DateTime.Now.ToString("hh:mm:tt");
                _dataState = "조치 완료";
            }

            RealTimeData(viewModel, clientNumber);
        }
        #endregion

        #region RealTimeView
        public void RealTimeData(MainModel viewModels, int clientNumber) {
            MainModel addViewItem = new MainModel {
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
        #endregion

        #region StatsView
        public void StatsData() {
            ChartValues<ObservableValue> _orderValue = new ChartValues<ObservableValue>();
            ChartValues<ObservableValue> _clearValue = new ChartValues<ObservableValue>();
            for (int i = 0; i < _mainModelBases.Count; i++) {
                _orderValue.Add(new ObservableValue(_mainModelBases[i].TotalOrder));
                _clearValue.Add(new ObservableValue(_mainModelBases[i].TotalClear));
            }
            _dispatcher.Invoke(() => {
                SeriesCollection = new SeriesCollection {
                
                // total order Count
                new ColumnSeries {
                    Title = "Total Order Count",
                    Values = _orderValue,
                    DataLabels = true, // enable data labels
                    LabelPoint = point => Labels[(int)point.X],
                    },

                // total clear Count
                new ColumnSeries {
                    Title = "Total Clear Count",
                    Values = _clearValue,
                    }
                };
            });
        }
        #endregion

        #region ActionHistoryView
        public void ActionHistoryData() {
            ActionHistoryModel model = new ActionHistoryModel();
            model.CallTime = _callTime;
            model.DataState = _dataState;
            model.GridTitle = _gridTitle;
            _dispatcher.Invoke(() => {
                DataGridItem.Add(model);
            });
        }
        #endregion
        #endregion
    }
}