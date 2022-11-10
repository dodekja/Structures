using System.Windows;
using SemestralThesisOne.UserInterface.App;
using SemestralThesisOne.UserInterface.Hospital;
using SemestralThesisOne.UserInterface.Patient;

namespace SemestralThesisOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App app;

        public MainWindow()
        {
            app = Application.Current as App;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void AddPatient(object sender, RoutedEventArgs e)
        {
            AddPatientWindow addPatientDialog = new AddPatientWindow(app.PatientManager,app.HospitalManager);
            addPatientDialog.ShowDialog();
        }

        private void AddHospital(object sender, RoutedEventArgs e)
        {
            AddHospitalWindow addHospitalDialog = new AddHospitalWindow(app.HospitalManager);
            addHospitalDialog.ShowDialog();
        }

        private void ShowPatients(object sender, RoutedEventArgs e)
        {
            ShowPatientsWindow showPatients = new ShowPatientsWindow(app.HospitalManager);
            showPatients.ShowDialog();
        }

        private void StartHospitalization(object sender, RoutedEventArgs e)
        {
            AddHospitalizationWindow addHospitalization =
                new AddHospitalizationWindow(app.HospitalManager);
            addHospitalization.ShowDialog();
        }

        private void EndHospitalization(object sender, RoutedEventArgs e)
        {
            EndHospitalizationWindow endHospitalization = new EndHospitalizationWindow(app.HospitalManager);
            endHospitalization.ShowDialog();
        }

        private void GenerateData(object sender, RoutedEventArgs e)
        {
            GenerateDataWindow generateData = new GenerateDataWindow(app.HospitalManager,app.PatientManager);
            generateData.ShowDialog();
        }

        private void ShowHospitals(object sender, RoutedEventArgs e)
        {
            ShowHospitalsWindow showPatients = new ShowHospitalsWindow(app.HospitalManager);
            showPatients.ShowDialog();
        }

        private void FindPatient(object sender, RoutedEventArgs e)
        {
            FindPatientWindow findPatient = new FindPatientWindow(app.HospitalManager);
            findPatient.ShowDialog();
        }

        private void FindPatientsByName(object sender, RoutedEventArgs e)
        {
            FindPatientsRangeWindow findPatients = new FindPatientsRangeWindow(app.HospitalManager);
            findPatients.ShowDialog();
        }

        private void ShowCurrentlyHospitalizedPatients(object sender, RoutedEventArgs e)
        {
            ShowCurrentlyHospitalizedPatientsWindow currentlyHospitalizedPatients = new ShowCurrentlyHospitalizedPatientsWindow(app.HospitalManager);
            currentlyHospitalizedPatients.ShowDialog();
        }

        private void ShowInsuranceReport(object sender, RoutedEventArgs e)
        {
            InsuranceReportWindow insuranceReport = new InsuranceReportWindow(app.HospitalManager);
            insuranceReport.ShowDialog();
        }


        private void BalanceStructures(object sender, RoutedEventArgs e)
        {
            app.HospitalManager.Balance();
            app.PatientManager.Balance();
            MessageBox.Show($"All structures successfully balanced.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RemoveHospital(object sender, RoutedEventArgs e)
        {
            RemoveHospitalWindow removeHospital = new RemoveHospitalWindow(app.HospitalManager);
            removeHospital.ShowDialog();
        }

        private void ShowHospitalizedPatientsBetweenDates(object sender, RoutedEventArgs e)
        {
            ShowPatientsBetweenDatesWindow showPatientsBetweenDates =
                new ShowPatientsBetweenDatesWindow(app.HospitalManager);
            showPatientsBetweenDates.ShowDialog();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            app.HospitalManager.Save();
            MessageBox.Show($"Save complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            app.HospitalManager.Load(app.PatientManager);
            MessageBox.Show($"Load complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
