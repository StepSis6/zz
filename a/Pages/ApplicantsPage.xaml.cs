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

namespace a.Pages
{
    /// <summary>
    /// Логика взаимодействия для ApplicantsPage.xaml
    /// </summary>
    public partial class ApplicantsPage : Page
    {
        public ApplicantsPage()
        {
            InitializeComponent();
        }

        private void btnSup(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null && mainWindow.MainFrame != null)
            {
                mainWindow.MainFrame.NavigationService.Navigate(new SubAppPage());
            }
            else
            {
                MessageBox.Show("MainFrame not found. Ensure it's defined in MainWindow.");
            }
        }
    }
}
