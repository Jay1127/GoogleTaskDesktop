using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;

namespace GoogleTaskDesktop.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => DialogCoordinator.Instance);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PopupViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PopupViewModel Popup
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PopupViewModel>();
            }
        }
        
        public static void Cleanup()
        {
        }
    }
}