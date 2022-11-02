using System.Collections.Generic;
using System.Windows;
using SemestralThesisOne.Core.Generators;
using SemestralThesisOne.Core.Model;
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
            List<string> hospitals = new List<string>();
            Hospital hospital;
            Patient patient;
            for (int hospitalIndex = 0; hospitalIndex < 50; hospitalIndex++)
            {
                hospital = DataGenerator.GenerateHospital();
                app.HospitalManager.AddNewRecord(hospital);
                hospitals.Add(hospital.Name);
                for (int patientIndex = 0; patientIndex < 50; patientIndex++)
                {
                    patient = DataGenerator.GeneratePatient();
                    app.HospitalManager.AddPatientToHospital(hospitals[hospitalIndex], patient);
                    app.PatientManager.AddNewRecord(patient);
                }
            }
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
    }
}
