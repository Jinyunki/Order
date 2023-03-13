/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Order"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Order.ViewModel {
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator() {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UnderBarViewModel>();
            SimpleIoc.Default.Register<SideBarViewModel>();
            SimpleIoc.Default.Register<RealTimeViewModel>();
            SimpleIoc.Default.Register<ActionHistoryViewModel>();
        }

        public ActionHistoryViewModel ActionHistoryViewModel {
            get {
                return ServiceLocator.Current.GetInstance<ActionHistoryViewModel>();
            }
        }

        public RealTimeViewModel RealTimeViewModel {
            get {
                return ServiceLocator.Current.GetInstance<RealTimeViewModel>();
            }
        }

        public SideBarViewModel SideBarViewModel {
            get {
                return ServiceLocator.Current.GetInstance<SideBarViewModel>();
            }
        }

        public UnderBarViewModel UnderBarViewModel {
            get {
                return ServiceLocator.Current.GetInstance<UnderBarViewModel>();
            }
        }

        public MainViewModel Main {
            get {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup() {
            // TODO Clear the ViewModels
        }
    }
}