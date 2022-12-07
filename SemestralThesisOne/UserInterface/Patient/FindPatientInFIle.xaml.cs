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
            PatientsTextBlock.Text = PatientManager.GetPatient(PatientIdTextBox.Text)?.ToString();
        }

        private void FindHospitalization_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = PatientManager.GetHospitalization(PatientIdTextBox.Text, int.Parse(HospitalIdTextBox.Text))?.ToString();
        }

        private void RemoveHospitalization_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.GetPatient(PatientIdTextBox.Text).RemoveHospitalization(int.Parse(HospitalIdTextBox.Text));
        }
    }
}
