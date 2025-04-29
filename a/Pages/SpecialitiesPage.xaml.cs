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
    /// Логика взаимодействия для SpecialitiesPage.xaml
    /// </summary>
    public partial class SpecialitiesPage : Page
    {
        public SpecialitiesPage()
        {
            InitializeComponent();
            LoadRoomData();
        }

        private async void LoadRoomData()
        {
            try
            {
                var specialities = await System.Threading.Tasks.Task.Run(() => Core.DB.Specialities.ToList());
                SpecialitiesDataGrid.ItemsSource = specialities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rooms: {ex.Message}");
            }
        }
    }
}
