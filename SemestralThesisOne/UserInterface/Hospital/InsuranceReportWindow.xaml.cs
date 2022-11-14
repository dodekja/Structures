using SemestralThesisOne.Core;
using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;
using SemestralThesisOne.Core.Model;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for InsuranceReportWindow.xaml
    /// </summary>
    public partial class InsuranceReportWindow : Window
    {
        private HospitalManager _hospitalManager;

        public InsuranceReportWindow(HospitalManager hospitalManager)
        {
            _hospitalManager = hospitalManager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            int daysOfHospitalizations = 0;
            var patients = _hospitalManager.GetPatientsList(HospitalNameTextBox.Text);

            PatientsTextBlock.Text = "";

            int daysInMonth = DateTime.DaysInMonth(MonthDatePicker.SelectedDate.Value.Year, MonthDatePicker.SelectedDate.Value.Month);
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDay = new DateTime(MonthDatePicker.SelectedDate.Value.Year, MonthDatePicker.SelectedDate.Value.Month, day);

                PatientsTextBlock.Text += $"Day: {currentDay.ToShortDateString()}\n";
                foreach (Core.Model.Patient patient in patients)
                {
                    if (Enums.GetStringFromInsuranceCompanyCode(patient.InsuranceCompanyCode) == InsuranceCompanyCodeComboBox.Text)
                    {
                        if (currentDay >= patient.CurrentHospitalization.Start)
                        {
                            daysOfHospitalizations++;
                            PatientsTextBlock.Text += patient.ToString() + "\n";
                        }
                        else
                        {
                            foreach (var hospitalization in patient.HospitalizationsEnded.InOrderData())
                            {
                                if (currentDay >= hospitalization.Start && currentDay <= patient.CurrentHospitalization.End)
                                {
                                    daysOfHospitalizations++;
                                    PatientsTextBlock.Text += patient.ToString() + "\n";
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            PatientsTextBlock.Text += $"Insurance: {InsuranceCompanyCodeComboBox.Text} Days: {daysOfHospitalizations}\n";
        }
    }
}
