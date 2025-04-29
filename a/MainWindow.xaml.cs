using a.ApplicationData;
using a.Pages;
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

namespace a
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppFrame.frameMain = MainFrame;
            MainFrame.Navigate(new LoginPage());
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void btnSpecial(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new SpecialitiesPage());
        }
     
        private void btnReg(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RegPage());
        }

        private void btnApplicants(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new ApplicantsPage());
        }
    }
}
