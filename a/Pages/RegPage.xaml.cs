using a.ApplicationData;
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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string email = txbLogin.Text.Trim();
            string password = txbPassword.Password;
            var existingUser = AppConect.modelObd.User.FirstOrDefault(x => x.email == email);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином есть!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                User userObj = new User()
                {
                    username = "Абитуриент", 
                    email = email,
                    password = password, 
                    RoleId = 1 
                };

                AppConect.modelObd.User.Add(userObj);
                AppConect.modelObd.SaveChanges();

                MessageBox.Show("Данные успешно добавлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных!\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void psbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string password = txbPassword.Password;
            string confirmPassword = psbPassword.Password;

            if (password != confirmPassword)
            {
                btnCreate.IsEnabled = false;
                psbPassword.Background = Brushes.LightCoral;
                psbPassword.BorderBrush = Brushes.Red;
            }
            else
            {
                btnCreate.IsEnabled = true;
                psbPassword.Background = Brushes.LightGreen;
                psbPassword.BorderBrush = Brushes.Green;
            }
        }
    }
}
