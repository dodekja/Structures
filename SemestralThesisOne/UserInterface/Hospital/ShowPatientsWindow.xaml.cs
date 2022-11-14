using System;
using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowPatientsWindow.xaml
    /// </summary>
    public partial class ShowPatientsWindow : Window
    {
        private HospitalManager hospitalManager;

        public ShowPatientsWindow(HospitalManager manager)
        {
            hospitalManager = manager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            try
            {
                var patientsList = hospitalManager.GetPatientsList(NameTextBox.Text);
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
    }
}
