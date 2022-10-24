using System;
using SemestralThesisOne.Core.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using static SemestralThesisOne.Core.Enums;

namespace SemestralThesisOne.UserInterface
{
    /// <summary>
    /// Interaction logic for AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private PatientManager _patientManager;
        public AddPatientWindow(PatientManager patientManager)
        { 
            _patientManager = patientManager;
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
                    _patientManager.AddNewRecord(FirstNameTextBox.Text, LastNameTextBox.Text,
                        IdentificationNumberTextBox.Text, DateOfBirthDatePicker.SelectedDate.Value, code.Value);
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
