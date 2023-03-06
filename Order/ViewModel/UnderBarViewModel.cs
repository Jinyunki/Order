using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Order;
using Order.Model;
using System.ComponentModel;
using System;

namespace Order.ViewModel {
    public class UnderBarViewModel : ViewModelBase {
        private UnderBarModel underBarModel;
        public UnderBarViewModel() {
            underBarModel = new UnderBarModel();
        }
        // 현재시간
        public string DateTime {
            get { return underBarModel.DateTiem; }
        }

        // 설비
        public string Facilities {
            get { return underBarModel.Facilities;}
        }
    }
}
