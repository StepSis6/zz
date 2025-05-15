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
    /// Логика взаимодействия для ApplicationsAdminPage.xaml
    /// </summary>
    public partial class ApplicationsAdminPage : Page
    {
        public ApplicationsAdminPage()
        {
            InitializeComponent();
            LoadApplicationsData();
        }

        private async void LoadApplicationsData()
        {
            try
            {
                // Загружаем данные асинхронно
                List<Applications> applications = await Task.Run(() => Core.DB.Applications.ToList());
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ApplicantionsAdminDataGrid.ItemsSource = applications;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedApplication = ApplicantionsAdminDataGrid.SelectedItem as Applications;
            if (selectedApplication != null)
            {
                // Ввод новых данных через InputBox
                string newApplicationIdStr = Interaction.InputBox("Введите новый идентификатор заявки:", "Редактировать", selectedApplication.Id.ToString());
                string newPassportData = Interaction.InputBox("Введите новые паспортные данные:", "Редактировать", selectedApplication.Passport);
                string newSubmissionDateStr = Interaction.InputBox("Введите новую дату подачи (дд/мм/гггг):", "Редактировать", selectedApplication.SubmissionDate.ToString("dd/MM/yyyy"));
                string newSubmissionTimeStr = Interaction.InputBox(
     "Введите новое время подачи (чч:мм):",
     "Редактировать",
     selectedApplication.SubmissionTime.ToString(@"hh\:mm"));

                // Преобразование строки в TimeSpan
                if (TimeSpan.TryParse(newSubmissionTimeStr, out TimeSpan newSubmissionTime))
                {
                    selectedApplication.SubmissionTime = newSubmissionTime;
                }
                else
                {
                    MessageBox.Show("Некорректный формат времени. Попробуйте в формате ЧЧ:ММ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // прекращаем выполнение метода, чтобы не сохранить неправильные данные
                }
                string newSelectedSpecialties = Interaction.InputBox("Введите выбранные специальности:", "Редактировать", selectedApplication.SelectedSpecialties);
                string newPointsStr = Interaction.InputBox("Введите количество баллов:", "Редактировать", selectedApplication.Points.ToString());

                // Валидация и преобразование данных
                if (int.TryParse(newApplicationIdStr, out int newApplicationId) &&
                    DateTime.TryParseExact(newSubmissionDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newSubmissionDate) &&
                    int.TryParse(newPointsStr, out int newPoints))
                {
                    // Обновление свойств объекта
                    selectedApplication.Id = newApplicationId;
                    selectedApplication.Passport = newPassportData;
                    selectedApplication.SubmissionDate = newSubmissionDate;
                    selectedApplication.SubmissionTime = newSubmissionTime;
                    selectedApplication.SelectedSpecialties = newSelectedSpecialties;
                    selectedApplication.Points = newPoints;

                    // Обновление отображения
                    ApplicantionsAdminDataGrid.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Некорректный формат данных. Попробуйте ещё раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для редактирования.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
