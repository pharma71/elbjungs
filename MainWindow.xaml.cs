using System.Windows;
using System.Windows.Controls;




namespace WpfApp
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }


        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabItem2.IsSelected)
            {
                tabItem2.Window_Loaded(sender, e);
            }

        }


    }
}
