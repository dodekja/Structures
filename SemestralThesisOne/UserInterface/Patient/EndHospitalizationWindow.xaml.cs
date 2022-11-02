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

namespace SemestralThesisOne.UserInterface.Patient
{
    /// <summary>
    /// Interaction logic for EndHospitalizationWindow.xaml
    /// </summary>
    public partial class EndHospitalizationWindow : Window
    {
        private HospitalManager _hospitalManager;
        private Core.Model.Patient? _patient;

        public EndHospitalizationWindow(HospitalManager hospitalManager)
        {
            InitializeComponent();
            _hospitalManager = hospitalManager;
            _patient = null;
        }

        private void FindButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _patient = _hospitalManager.GetPatientFromHospitalById(HospitalNameTextBox.Text, PatientIdTextBox.Text);
                if (_patient != null)
                {
                    PatientsTextBlock.Text += _patient.ToString() + "\n";
                }
                else
                {
                    MessageBox.Show($"Patient with ID: {PatientIdTextBox.Text} was not found in hospital {HospitalNameTextBox.Text}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddEndButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
