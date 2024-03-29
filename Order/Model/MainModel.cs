﻿using GalaSoft.MvvmLight;
using LiveCharts;
using Order.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Order.ViewModel {
    public class MainModel : ViewModelBase {
        public void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainModel() { }

        public MainModel(string itemTitle, int orderCount, int orderClearCount) {
            ItemTitle = itemTitle;
            OrderCount = orderCount;
            OrderClearCount = orderClearCount;
        }


        #region RealTime Current
        private ObservableCollection<MainModel> _currentView = null;
        public ObservableCollection<MainModel> CurrentView {
            get {
                if (_currentView == null) {
                    _currentView = new ObservableCollection<MainModel>();
                }
                return _currentView;
            }
            set {
                _currentView = value;
            }
        }
        #endregion

        #region ActionHistory DataGridItem
        private ObservableCollection<ActionHistoryModel> _dataGridItem = null;
        public ObservableCollection<ActionHistoryModel> DataGridItem {
            get {
                if (_dataGridItem == null) {
                    _dataGridItem = new ObservableCollection<ActionHistoryModel>();
                }
                return _dataGridItem;
            }
            set {
                _dataGridItem = value;
            }
        }
        #endregion

        #region StatsViewItem
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection {
            get { return _seriesCollection; }
            set {
                _seriesCollection = value;
                RaisePropertyChanged("SeriesCollection");
            }
        }
        public string[] Labels { get; set; }

        private int _totalOrder = 0;

        public int TotalOrder {
            get { return _totalOrder; }
            set {
                if (_totalOrder == value) {
                    return;
                } else {
                    _totalOrder = value;
                    OnPropertyChanged("TotalOrder");
                }
            }
        }
        #endregion

        #region Context
        private int _totalClear = 0;

        public int TotalClear {
            get { return _totalClear; }
            set {
                if (_totalClear == value) {
                    return;
                } else {
                    _totalClear = value;
                    OnPropertyChanged("TotalClear");
                }
            }
        }


        private string _itemTitle;
        public string ItemTitle {
            get {
                return _itemTitle;
            }
            set {
                if (_itemTitle == value)
                    return;
                _itemTitle = value;
                OnPropertyChanged("ItemTitle");
            }
        }
        

        private int _orderCount = 0;
        public int OrderCount {
            get => _orderCount;
            set {
                if (_orderCount == value) {
                    return;
                } else {
                    _orderCount = value;
                    if (value > 10) {
                        _orderCount = 1;
                    }
                    OnPropertyChanged("OrderCount");
                }
            }
        }

        
        private int _orderClearCount = 0;
        public int OrderClearCount {
            get {
                return _orderClearCount;
            }
            set {
                if (_orderClearCount == value) {
                    return;
                } else {
                    _orderClearCount = value;
                    if (value > 10) {
                        _orderClearCount = 1;
                    }
                OnPropertyChanged("OrderClearCount");
                }
            }
        }

        private bool _isConnected = false;
        public bool IsConnected {
            get {
                return _isConnected;
            }
            set {
                if (_isConnected == value)
                    return;
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }
        #endregion
    }
}
