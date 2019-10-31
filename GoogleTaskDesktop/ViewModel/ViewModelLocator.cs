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
            SimpleIoc.Default.Register<EditorDialogViewModel>();
            SimpleIoc.Default.Register<TaskDetailFlyoutViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public EditorDialogViewModel Popup
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditorDialogViewModel>();
            }
        }

        public TaskDetailFlyoutViewModel TaskDetailFlyout
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TaskDetailFlyoutViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}