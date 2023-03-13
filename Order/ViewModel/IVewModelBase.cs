﻿using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Order.ViewModel {
    public class IViewModelBase : ViewModelBase {
        public void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public IViewModelBase() { }

        public IViewModelBase(string itemTitle, int orderCount, int orderClearCount) {
            ItemTitle = itemTitle;
            OrderCount = orderCount;
            OrderClearCount = orderClearCount;
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
                    OnPropertyChanged("OrderCount");
                }
            }
        }

        private string _orderTime ;
        public string OrderTime {
            get => _orderTime;
            set {
                if (_orderTime == value)
                    return;
                _orderTime = value;
                OnPropertyChanged(nameof(OrderTime));
            }
        }
        private int _orderClearCount = 0;
        public int OrderClearCount {
            get {
                return _orderClearCount;
            }
            set {
                if (_orderClearCount == value)
                    return;
                _orderClearCount = value;
                OnPropertyChanged("OrderClearCount");
            }
        }

        private string _orderClearTime ;
        public string OrderClearTime {
            get => _orderClearTime;
            set {
                if (_orderClearTime == value)
                    return;
                _orderClearTime = value;
                OnPropertyChanged(nameof(OrderClearTime));
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

        private string _msgBox;
        public string MsgBox {
            get {
                return _msgBox;
            }
            set {
                if (_msgBox == value)
                    return;
                _msgBox = value;
                OnPropertyChanged("MsgBox");
            }
        }
    }
}
