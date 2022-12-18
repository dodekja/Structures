using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;
using SemestralThesisOne.Core.Generators;

namespace SemestralThesisOne.UserInterface.Patient
{
    /// <summary>
    /// Interaction logic for EndHospitalizationWindow.xaml
    /// </summary>
    public partial class EndHospitalizationWindow : Window
    {
        private HospitalManager _hospitalManager;
        private PatientManager _patientManager;
        private Core.Model.Patient? _patient;

        public EndHospitalizationWindow(HospitalManager hospitalManager, PatientManager patientManager)
        {
            InitializeComponent();
            _hospitalManager = hospitalManager;
            _patient = null;
            _patientManager = patientManager;
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
                    DateTime dateTime = HospitalizationEndDatePicker.SelectedDate.Value;
                    dateTime = DateTimeGenerator.AddRandomTime(dateTime);
                    _patient.EndHospitalization(dateTime);
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

        private void AddEndToFileButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            if (_patient.IsHospitalized())
            {
                DateTime dateTime = HospitalizationEndDatePicker.SelectedDate.Value;
                dateTime = DateTimeGenerator.AddRandomTime(dateTime);
                try
                {
                    _patientManager.EndCurrentHospitalizationInFile(_patient.IdentificationNumber, dateTime);
                }
                catch (InvalidOperationException exception)
                {
                    MessageBox.Show($"{exception.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                PatientsTextBlock.Text = _patient + "\n";
            }
            else
            {
                MessageBox.Show("Patient is not hospitalized", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void FindInFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            try
            {
                _patient = _patientManager.GetPatientFromFile(PatientIdTextBox.Text);
                if (_patient != null)
                {
                    PatientsTextBlock.Text = _patient + "\n";
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
    }
}
