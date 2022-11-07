using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;
using SemestralThesisOne.Core;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowCurrentlyHospitalizedPatientsWindow.xaml
    /// </summary>
    public partial class ShowCurrentlyHospitalizedPatientsWindow : Window
    {
        private HospitalManager _hospitalManager;

        public ShowCurrentlyHospitalizedPatientsWindow(HospitalManager hospitalManager)
        {
            _hospitalManager = hospitalManager;
            InitializeComponent();
        }

        private void FindButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientsTextBlock.Text = "";
                var patientsList = _hospitalManager.GetCurrentlyHospitalizedPatients(NameTextBox.Text);
                foreach (var patient in patientsList)
                {
                    PatientsTextBlock.Text += patient.ToString() + "\n";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindInsured_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientsTextBlock.Text = "";
                var patientsList = _hospitalManager.GetCurrentlyHospitalizedPatients(NameTextBox.Text);
                foreach (var patient in patientsList)
                {
                    if (Enums.GetStringFromInsuranceCompanyCode(patient.InsuranceCompanyCode) == InsuranceCompanyCodeComboBox.Text)
                    {
                        PatientsTextBlock.Text += patient.ToString() + "\n";
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindInsuredByID_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientsTextBlock.Text = "";
                var patientsList = _hospitalManager.GetCurrentlyHospitalizedPatientsById(NameTextBox.Text);
                foreach (var patient in patientsList)
                {
                    if (Enums.GetStringFromInsuranceCompanyCode(patient.InsuranceCompanyCode) == InsuranceCompanyCodeComboBox.Text)
                    {
                        PatientsTextBlock.Text += patient.ToString() + "\n";
                    }
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
