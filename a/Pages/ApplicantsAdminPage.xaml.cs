using a.ApplicationData;
using Microsoft.VisualBasic;
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
    public partial class ApplicantsAdminPage : Page
    {
        public ApplicantsAdminPage()
        {
            InitializeComponent(); // Ensure this method is auto-generated from XAML
            LoadSettlementData();
        }

        // Remove the manual implementation of InitializeComponent
        // If no XAML exists, you need to create one, otherwise leave it as is.

        private async void LoadSettlementData()
        {
            try
            {
                List<Applicants> applicants = await Task.Run(() => Core.DB.Applicants.ToList());
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ApplicantsAdminDataGrid.ItemsSource = applicants;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settlement {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedApplicant = ApplicantsAdminDataGrid.SelectedItem as Applicants; 
            if (selectedApplicant != null)
            {
                string newApplicantId = Interaction.InputBox("Введите новый идентификатор абитуриента:", "Редактировать", selectedApplicant.ApplicantId.ToString());
                string newPassportData = Interaction.InputBox("Введите новые паспортные данные:", "Редактировать", selectedApplicant.PassportData);
                string newBirthDateStr = Interaction.InputBox("Введите новую дату рождения (дд/мм/гггг):", "Редактировать", selectedApplicant.BirthDate.ToString("dd/MM/yyyy"));
                string newAddress = Interaction.InputBox("Введите новый адрес:", "Редактировать", selectedApplicant.Address);
                string newEmail = Interaction.InputBox("Введите новый Email:", "Редактировать", selectedApplicant.Email);
                string newPhone = Interaction.InputBox("Введите новый телефон:", "Редактировать", selectedApplicant.Phone);

                if (int.TryParse(newApplicantId, out int newApplicantIdInt) &&
                    DateTime.TryParseExact(newBirthDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newBirthDate))
                {
                    selectedApplicant.ApplicantId = newApplicantIdInt;
                    selectedApplicant.PassportData = newPassportData;
                    selectedApplicant.BirthDate = newBirthDate;
                    selectedApplicant.Address = newAddress;
                    selectedApplicant.Email = newEmail;
                    selectedApplicant.Phone = newPhone;
                    ApplicantsAdminDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Некорректный формат данных. Пожалуйста, попробуйте ещё раз.", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите абитуриента для редактирования.");
            }
        }
    }
}
