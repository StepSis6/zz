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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void btnAppli(object sender, RoutedEventArgs e)
        {
            // Скрываем кнопку "Абитуриенты"
            btnApplicantss.Visibility = Visibility.Collapsed;

            // Также скрываем кнопку "Заявления"
            btnApplicationss.Visibility = Visibility.Collapsed; // добавьте, если она есть

            // Навигируем в страницу ApplicantsAdminPage
            MainFrame.NavigationService.Navigate(new ApplicantsAdminPage());
        }

        private void btnAppliс(object sender, RoutedEventArgs e)
        {
            // Навигируем в страницу ApplicationsAdminPage
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null && mainWindow.MainFrame != null)
            {
                mainWindow.MainFrame.NavigationService.Navigate(new ApplicationsAdminPage());

                // После навигации делаем кнопку "Абитуриенты" видимой снова
                btnApplicantss.Visibility = Visibility.Visible;

                // И, чтобы скрывать кнопку "Заявления" при входе, можно сделать это тут
                btnApplicationss.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("MainFrame not found. Ensure it's defined in MainWindow.");
            }
        }
    }
}
