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
using SemestralThesisOne.Core.Model;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowPatientsBetweenDatesWindow.xaml
    /// </summary>
    public partial class ShowPatientsBetweenDatesWindow : Window
    {
        private HospitalManager _hospitalManager;

        public ShowPatientsBetweenDatesWindow(HospitalManager hospitalManager)
        {
            _hospitalManager = hospitalManager;
            InitializeComponent();
        }

        private void OkButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            PatientsTextBlock.Text = "";
            try
            {
                var patientsList = _hospitalManager.GetPatientsList(NameTextBox.Text);
                foreach (var patient in patientsList)
                {
                    if (patient.CurrentHospitalization.Start >= FromDatePicker.SelectedDate.Value &&
                        patient.CurrentHospitalization.Start < ToDatePicker.SelectedDate.Value)
                    {
                        PatientsTextBlock.Text += patient.ToString() + "\n";
                    }
                    else
                    {
                        foreach (var tuple in patient.HospitalizationsEnded.InOrder())
                        {
                            Hospitalization hospitalization = tuple.Item2;
                            if (hospitalization.Start >= FromDatePicker.SelectedDate.Value && patient.CurrentHospitalization.Start < ToDatePicker.SelectedDate.Value ||
                                hospitalization.End >= FromDatePicker.SelectedDate.Value && patient.CurrentHospitalization.End < ToDatePicker.SelectedDate.Value)
                            {
                                PatientsTextBlock.Text += patient.ToString() + "\n";
                                break;
                            }
                        }
                    }
                }

                if (patientsList.Count == 0)
                {
                    MessageBox.Show($"No patients in hospital.", "Error",
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
