using System;
using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Patient
{
    /// <summary>
    /// Interaction logic for FindPatientInFIle.xaml
    /// </summary>
    public partial class FindPatientInFIle : Window
    {
        public PatientManager PatientManager { get; set; }

        public FindPatientInFIle(PatientManager patientManager)
        {
            PatientManager = patientManager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                PatientsTextBlock.Text = PatientManager.GetPatientFromFile(PatientIdTextBox.Text)?.ToString();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void FindHospitalization_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = PatientManager.GetHospitalization(PatientIdTextBox.Text, int.Parse(HospitalIdTextBox.Text))?.ToString();
        }

        private void RemoveHospitalization_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.RemoveHospitalization(PatientIdTextBox.Text, int.Parse(HospitalIdTextBox.Text));
            //PatientManager.GetPatient(PatientIdTextBox.Text).RemoveHospitalization(int.Parse(HospitalIdTextBox.Text));
        }

        private void RemovePatient_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PatientIdTextBox.Text))
            {
                PatientManager.RemovePatient(PatientIdTextBox.Text);
            }
            else
            {
                MessageBox.Show("Please enter patient ID", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
