using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для SubAppPage.xaml
    /// </summary>
    public partial class SubAppPage : Page
    {
        private readonly ApplicantsDatabaseEntities _context;
        public SubAppPage()
        {
            InitializeComponent();
            _context = new ApplicantsDatabaseEntities();
            LoadEmailComboBox();
            
        }

        private void LoadEmailComboBox()
        {
            var specialities = _context.Specialities.ToList();
            SpecialitiesComboBox.ItemsSource = specialities;
        }

       

        private void btnReservation(object sender, RoutedEventArgs e)
        {
            var selectedEmail = SpecialitiesComboBox.SelectedItem as Specialities;
            var points = PointsTextBox.Text;
            var SubmissionDate = ApplicationDatePicker.SelectedDate;
            var SubmissionTime = ApplicationTimeTextBox;
           

            if (selectedEmail == null || !SubmissionDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }




            var newApplicants = new Applicants
            {

                FIO = FullNameTextBox.Text,
                Address = AddressTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text,
            };

            _context.Applicants.Add(newApplicants);
            _context.SaveChanges();

            MessageBox.Show("Бронирование успешно создано.");
        }

    }
}
