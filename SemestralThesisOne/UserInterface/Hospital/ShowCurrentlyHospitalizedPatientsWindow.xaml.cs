using SemestralThesisOne.Core.ViewModel;
using System;
using System.Windows;

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

        private void FindFilteredButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RangeStartDatepicker.SelectedDate.HasValue)
                {
                    if (RangeEndDatepicker.SelectedDate.HasValue)
                    {
                        var patientsList = _hospitalManager.GetCurrentlyHospitalizedPatientsRangeByDate(NameTextBox.Text,
                            RangeStartDatepicker.SelectedDate.Value, RangeEndDatepicker.SelectedDate.Value);
                        PatientsTextBlock.Text = "";
                        foreach (var patient in patientsList)
                        {
                            PatientsTextBlock.Text += patient.ToString() + "\n";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a range end date", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a range start date", "Error",
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
