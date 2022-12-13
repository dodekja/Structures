using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;
using SemestralThesisOne.Core.Generators;

namespace SemestralThesisOne.UserInterface.Patient
{
    /// <summary>
    /// Interaction logic for AddHospitalizationWindow.xaml
    /// </summary>
    public partial class AddHospitalizationWindow
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
            PatientsTextBlock.Text = "";
            try
            {
                _patient = _hospitalManager.GetPatientFromHospitalById(HospitalNameTextBox.Text, PatientIdTextBox.Text);
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

        private void AddStartButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            if (_patient != null)
            {
                if (!_patient.IsHospitalized())
                {
                    if (HospitalizationStartDatePicker.SelectedDate != null)
                    {
                        if (!string.IsNullOrWhiteSpace(DiagnosisTextBox.Text))
                        {
                            DateTime dateTime = HospitalizationStartDatePicker.SelectedDate.Value;
                            dateTime = DateTimeGenerator.AddRandomTime(dateTime);
                            _patient.AddCurrentHospitalization(dateTime, DiagnosisTextBox.Text);
                            _hospitalManager.AddCurrentlyHospitalizedPatient(HospitalNameTextBox.Text, _patient);
                            PatientsTextBlock.Text = _patient + "\n";
                        }
                        else
                        {
                            MessageBox.Show("Please enter a diagnosis", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select hospitalization start date", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Patient is already hospitalized", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Patient not loaded", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindInFileButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
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

        private void AddHospitalizationToFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            if (!_patient.IsHospitalized())
            {
                if (HospitalizationStartDatePicker.SelectedDate != null)
                {
                    if (!string.IsNullOrWhiteSpace(DiagnosisTextBox.Text))
                    {
                        DateTime dateTime = HospitalizationStartDatePicker.SelectedDate.Value;
                        dateTime = DateTimeGenerator.AddRandomTime(dateTime);
                        try
                        {
                            _patientManager.AddHospitalizationToFile(PatientIdTextBox.Text, dateTime, DiagnosisTextBox.Text);
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
                        MessageBox.Show("Please enter a diagnosis", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select hospitalization start date", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Patient is already hospitalized", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
