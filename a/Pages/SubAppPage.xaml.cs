using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a.Pages
{
    public partial class SubAppPage : Page
    {
        // Статическая переменная для автоматического увеличения ID заявителя
        private static long applicantIdCounter = 45462595;

        // Строка подключения к базе данных (замените на правильную строку подключения)
        private readonly string connectionString = @"Data Source=MLEM\MSSQLSERVER01;Initial Catalog=ApplicantsDatabase;Integrated Security=True";

        public SubAppPage()
        {
            InitializeComponent();
            LoadSpecialitiesAsync();
            InitializeAutomaticFields();
        }

        // Модель специальности для привязки к ComboBox
        public class Speciality
        {
            public int Code { get; set; }
            public string Name { get; set; }
        }

        // Коллекция для хранения специальностей
        private ObservableCollection<Speciality> Specialities { get; set; } = new ObservableCollection<Speciality>();

        // Загружаем специальности асинхронно при инициализации страницы
        private async void LoadSpecialitiesAsync()
        {
            try
            {
                var specialities = await GetSpecialitiesAsync();
                Specialities.Clear();
                foreach (var speciality in specialities)
                {
                    Specialities.Add(speciality);
                }
                SpecialitiesComboBox.ItemsSource = Specialities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки специальностей: {ex.Message}");
            }
        }

        // Асинхронный метод получения специальностей из базы данных
        private async Task<ObservableCollection<Speciality>> GetSpecialitiesAsync()
        {
            var result = new ObservableCollection<Speciality>();
            string query = "SELECT [Id], [Name] FROM [dbo].[Specialities]";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(new Speciality
                        {
                            Code = reader.GetInt32(0), // [Id]
                            Name = reader.GetString(1) // [Name]
                        });
                    }
                }
            }
            return result;
        }

        // Инициализация автоматических полей времени и ID заявителя
        private void InitializeAutomaticFields()
        {
            // Заполняем текущим временем в формате HH:MM
            string currentTime = DateTime.Now.ToString("HH:mm");
            SubmissionTimeTextBox.Text = currentTime;

            // Заполняем ID заявителя текущим значением и увеличиваем его для следующего
            ApplicantIdTextBox.Text = applicantIdCounter.ToString();
        }

        // Обработчик кнопки отправки заявки
        private async void btnReservation(object sender, RoutedEventArgs e)
        {
            // Обновляем время подачи автоматически при отправке
            SubmissionTimeTextBox.Text = DateTime.Now.ToString("HH:mm");

            // Валидация данных
            if (!ValidateInputs(out DateTime submissionDate, out string submissionTime, out string applicantId, out string passport, out string educationDiploma, out string points, out Speciality selectedSpeciality))
            {
                MessageBox.Show("Пожалуйста, заполните все поля правильно.");
                return;
            }

            // Время автоматически устанавливается текущим (уже сделано выше)
            DateTime submissionTimestamp = DateTime.Now;

            // Автоматический ID заявителя
            long currentApplicantId = applicantIdCounter++;
            string applicantIdStr = currentApplicantId.ToString();
            ApplicantIdTextBox.Text = applicantIdStr; // Обновляем поле на форме

            // Вставка заявки
            try
            {
                bool success = await SaveApplicationAsync(submissionDate, submissionTime, applicantIdStr, selectedSpeciality.Name, passport, educationDiploma, points);
                if (success)
                {
                    MessageBox.Show("Заявка успешно отправлена!");
                    ClearFields();
                    // После очистки полей снова устанавливаем автоматические значения
                    InitializeAutomaticFields();
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении заявки.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка базы данных: {ex.Message}");
            }
        }

        // Валидация пользовательского ввода
        private bool ValidateInputs(out DateTime submissionDate, out string submissionTime, out string applicantId, out string passport, out string educationDiploma, out string points, out Speciality speciality)
        {
            submissionDate = SubmissionDatePicker.SelectedDate ?? DateTime.MinValue;
            submissionTime = SubmissionTimeTextBox.Text.Trim();
            applicantId = ApplicantIdTextBox.Text.Trim();
            passport = PassportTextBox.Text.Trim();
            educationDiploma = EducationDiplomaTextBox.Text.Trim();
            points = PointsTextBox.Text.Trim();
            speciality = SpecialitiesComboBox.SelectedItem as Speciality;

            if (submissionDate == DateTime.MinValue ||
                string.IsNullOrEmpty(submissionTime) ||
                string.IsNullOrEmpty(applicantId) ||
                speciality == null ||
                string.IsNullOrEmpty(passport) ||
                string.IsNullOrEmpty(educationDiploma) ||
                string.IsNullOrEmpty(points))
            {
                return false;
            }

            // Можно добавить дополнительные проверки формата времени, числовых значений и т.д.
            return true;
        }

        // Асинхронное сохранение заявки в базу данных
        private async Task<bool> SaveApplicationAsync(DateTime submissionDate, string submissionTime, string applicantId, string specialityName, string passport, string educationDiploma, string points)
        {
            string insertQuery = @"
        INSERT INTO Applications 
        ([PersonalFileNumber], [SubmissionDate], [SubmissionTime], [ApplicantId], [SelectedSpecialties], [Passport], [EducationDiploma], [Points])
        VALUES (@PersonalFileNumber, @SubmissionDate, @SubmissionTime, @ApplicantId, @SelectedSpecialties, @Passport, @EducationDiploma, @Points)";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(insertQuery, conn))
            {
                // Генерация уникального GUID для PersonalFileNumber
                string personalFileNumber = Guid.NewGuid().ToString();

                await conn.OpenAsync();

                cmd.Parameters.AddWithValue("@PersonalFileNumber", personalFileNumber);
                cmd.Parameters.AddWithValue("@SubmissionDate", submissionDate.Date);
                cmd.Parameters.AddWithValue("@SubmissionTime", submissionTime);
                cmd.Parameters.AddWithValue("@ApplicantId", applicantId);
                cmd.Parameters.AddWithValue("@SelectedSpecialties", specialityName);
                cmd.Parameters.AddWithValue("@Passport", passport);
                cmd.Parameters.AddWithValue("@EducationDiploma", educationDiploma);
                cmd.Parameters.AddWithValue("@Points", points);

                int rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
        }

        // Очистка полей формы
        private void ClearFields()
        {
            SubmissionDatePicker.SelectedDate = null;
            SubmissionTimeTextBox.Clear();
            ApplicantIdTextBox.Clear();
            PassportTextBox.Clear();
            EducationDiplomaTextBox.Clear();
            PointsTextBox.Clear();
            SpecialitiesComboBox.SelectedItem = null;
        }
    }
}
