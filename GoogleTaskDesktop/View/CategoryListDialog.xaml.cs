using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoogleTaskDesktop
{
    /// <summary>
    /// CategoryListPopup.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CategoryListDialog : UserControl
    {
        public CategoryListDialog()
        {
            InitializeComponent();
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            var mouseDownEvent =
                new MouseButtonEventArgs(Mouse.PrimaryDevice,
                    Environment.TickCount,
                    MouseButton.Right)
                {
                    RoutedEvent = Mouse.MouseUpEvent,
                    Source = sender,
                };

            InputManager.Current.ProcessInput(mouseDownEvent);
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {   
            e.Handled = e.ChangedButton == MouseButton.Right;
        }

        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
