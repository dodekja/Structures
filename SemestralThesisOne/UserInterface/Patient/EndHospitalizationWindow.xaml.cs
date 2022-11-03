using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;

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
                    PatientsTextBlock.Text = _patient.ToString() + "\n";
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
            if (_patient != null)
            {
                if (_patient.IsHospitalized())
                {
                    _hospitalManager.RemoveCurrentlyHospitalizedPatient(HospitalNameTextBox.Text, _patient);
                    _patient.EndHospitalization(HospitalizationEndDatePicker.SelectedDate);
                    PatientsTextBlock.Text = _patient.ToString() + "\n";
                }
                else
                {
                    MessageBox.Show($"Patient with ID {_patient.IdentificationNumber} is not currently hospitalized.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Patient not loaded.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
