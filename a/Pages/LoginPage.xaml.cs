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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTexboxPag3.Text;
            var password = PasswordPag3.Password;

            try
            {
                using (var db = new ApplicantsDatabaseEntities())
                {
                    var user = db.User.FirstOrDefault(u => u.email == email && u.password == password);

                    if (user != null)
                    {
                        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                        if (mainWindow != null)
                        {
                            if (user.RoleId == 1)
                            {
                                ApplicantsPage clientPage = new ApplicantsPage();
                                mainWindow.MainFrame.NavigationService.Navigate(clientPage);
                                return;
                            }
                            if (user.RoleId == 2)
                            {
                                AdminPage adminPage = new AdminPage();
                                mainWindow.MainFrame.NavigationService.Navigate(adminPage);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Неизвестная роль пользователя.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("MainWindow is null.  Cannot navigate.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
