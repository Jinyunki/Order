﻿using GalaSoft.MvvmLight;
using Order.Utiles;
using System.ComponentModel;

namespace Order.ViewModel {
    public class SideBarViewModel : ViewModelBase {
        private ViewModelLocator _locator = new ViewModelLocator();
        public SideBarViewModel() {
            _messageBoxService = new MessageBoxService();
            CurrentViewModel = _locator.RealTimeViewModel;
            initButtonEvent();
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
        #endregion

        // === ButtonEvent ===
        #region

        private Command<RealTimeViewModel> _btnRealTime;
        private Command<ActionHistoryViewModel> _btnActionHistor;
        private Command<StatsViewModel> _btnStatsViewModel;
        // Button Type Event
        public RelayCommand<string> ViewChangeCommand {
            get {
                return new RelayCommand<string>((confType) => {
                    if (confType == "RealTimeView") {
                        CurrentViewModel = _locator.RealTimeViewModel;
                    } else if (confType == "ActionHistoryViewModel") {
                        CurrentViewModel = _locator.ActionHistoryViewModel;
                    } else if (confType == "StatsViewModel") {
                        CurrentViewModel = _locator.StatsViewModel;
                    }
                });
            }
        }

        // Event Method
        private void initButtonEvent() {
            _btnRealTime = new Command<RealTimeViewModel>(CommandExecute, CommandCanExecute);
            _btnActionHistor = new Command<ActionHistoryViewModel>(CommandExecuteAction, CommandCanExecute);
            _btnStatsViewModel = new Command<StatsViewModel>(CommandExecuteStats, CommandCanExecute);
        }
        private void CommandExecuteStats(StatsViewModel obj) {
            this.CurrentViewModel = obj;
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
