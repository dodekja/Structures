using SemestralThesisOne.Core.ViewModel;
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
using System.Windows.Shapes;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for FindPatientsRangeWindow.xaml
    /// </summary>
    public partial class FindPatientsRangeWindow : Window
    {
        private HospitalManager _hospitalManager;

        public FindPatientsRangeWindow(HospitalManager manager)
        {
            _hospitalManager = manager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            try
            {
                var patientsList = _hospitalManager.GetPatientsFromHospitalByName(HospitalNameTextBox.Text, PatientNameTextBox.Text);
                foreach (var patient in patientsList)
                {
                    PatientsTextBlock.Text += patient.ToString() + "\n";
                }

                if (patientsList.Count == 0)
                {
                    MessageBox.Show($"Patients with ID: {PatientNameTextBox.Text} were not found in hospital {HospitalNameTextBox.Text}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
