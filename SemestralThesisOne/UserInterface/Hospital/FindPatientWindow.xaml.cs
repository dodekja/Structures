using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for FindPatientWindow.xaml
    /// </summary>
    public partial class FindPatientWindow : Window
    {
        private HospitalManager _hospitalManager;

        public FindPatientWindow(HospitalManager manager)
        {
            _hospitalManager = manager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            try
            {
                var patient = _hospitalManager.GetPatientFromHospitalById(HospitalNameTextBox.Text,PatientIdTextBox.Text);
                if (patient != null)
                {
                    PatientsTextBlock.Text += patient.ToString() + "\n";
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
