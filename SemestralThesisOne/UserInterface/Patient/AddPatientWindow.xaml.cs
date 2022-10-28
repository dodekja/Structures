using System;
using System.ComponentModel;
using System.Windows;
using SemestralThesisOne.Core.ViewModel;
using static SemestralThesisOne.Core.Enums;

namespace SemestralThesisOne.UserInterface.Patient
{
    /// <summary>
    /// Interaction logic for AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private PatientManager _patientManager;

        private HospitalManager _hospitalManager;

        public AddPatientWindow(PatientManager patientManager, HospitalManager hospitalManager)
        { 
            _patientManager = patientManager;
            _hospitalManager = hospitalManager;
            InitializeComponent();
        }

        private void AddPatientWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            IdentificationNumberTextBox.Text = "";
            DateOfBirthDatePicker.Text = "";
            InsuranceCompanyCodeComboBox.SelectedItem = InsuranceCompanyCodes.VSZP;
            base.OnClosed(e);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            InsuranceCompanyCodes? code = InsuranceCompanyCodeComboBox.Text switch
            {
                nameof(InsuranceCompanyCodes.VSZP) => InsuranceCompanyCodes.VSZP,
                nameof(InsuranceCompanyCodes.Dovera) => InsuranceCompanyCodes.Dovera,
                nameof(InsuranceCompanyCodes.Union) => InsuranceCompanyCodes.Union,
                _ => null
            };

            if (DateOfBirthDatePicker.SelectedDate != null && code != null)
            {
                try
                {
                    Core.Model.Patient newPatient = new Core.Model.Patient(FirstNameTextBox.Text, LastNameTextBox.Text, IdentificationNumberTextBox.Text, DateOfBirthDatePicker.SelectedDate.Value, code.Value);
                    _patientManager.AddNewRecord(newPatient);
                    _hospitalManager.AddPatientToHospital(HospitalNameTextBox.Text, newPatient);
                }
                catch (ArgumentException argumentException)
                {
                    MessageBox.Show("Patient already exists", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
            }
            else
            {
                MessageBox.Show("Invalid Date of Birth or Insurance Company Code", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            
        }
    }
}
