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
    /// Interaction logic for AddHospitalizationWindow.xaml
    /// </summary>
    public partial class AddHospitalizationWindow : Window
    {
        private HospitalManager _hospitalManager;
        private PatientManager _patientManager;
        private Core.Model.Patient? _patient;

        public AddHospitalizationWindow(HospitalManager hospitalManager, PatientManager patientManager)
        {
            _hospitalManager = hospitalManager;
            _patientManager = patientManager;
            _patient = null;
            InitializeComponent();
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

        private void AddStartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_patient != null)
            {
                if (HospitalizationStartDatePicker.SelectedDate != null)
                {
                    if (!string.IsNullOrWhiteSpace(DiagnosisTextBox.Text))
                    {
                        _patient.AddCurrentHospitalization(HospitalizationStartDatePicker.SelectedDate.Value, DiagnosisTextBox.Text);
                        PatientsTextBlock.Text = _patient.ToString() + "\n";
                    }
                    else
                    {
                        MessageBox.Show($"Please enter a diagnosis", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show($"Please select hospitalization start date", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show($"Patient not loaded", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
