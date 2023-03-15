using System;
using System.Windows;

namespace Order.Utiles {
    public interface IDispatcher {
        void Invoke(Action action);
    }

    public class DispatcherWrapper : IDispatcher {
        public void Invoke(Action action) {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
