using SemestralThesisOne.Core.Generators;
using System;
using System.Collections.Generic;
using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.App
{
    /// <summary>
    /// Interaction logic for GenerateDataWindow.xaml
    /// </summary>
    public partial class GenerateDataWindow
    {
        private HospitalManager _hospitalManager;

        private PatientManager _patientManager;

        public GenerateDataWindow(HospitalManager hospitalManager, PatientManager patientManager)
        {
            _hospitalManager = hospitalManager;
            _patientManager = patientManager;
            InitializeComponent();
        }

        private void GenerateData(object sender, RoutedEventArgs e)
        {
            int numberOfHospitals = 0;
            int numberOfPatients = 0;
            int numberOfHospitalizations = 0;
            try
            {
                numberOfHospitals = int.Parse(HospitalsTextBox.Text);
                numberOfPatients = int.Parse(PatientsTextBox.Text);
                numberOfHospitalizations = int.Parse(HospitalizationsTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Values in the text boxes are not numbers.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            List<string> hospitals = new List<string>();
            Core.Model.Hospital hospital;
            Core.Model.Patient patient;
            for (int hospitalIndex = 0; hospitalIndex < numberOfHospitals; hospitalIndex++)
            {
                hospital = DataGenerator.GenerateHospital();
                _hospitalManager.AddNewRecord(hospital);
                hospitals.Add(hospital.Name);
                for (int patientIndex = 0; patientIndex < numberOfPatients; patientIndex++)
                {
                    patient = DataGenerator.GeneratePatient();
                    patient.SetCurrentHospitalization(DataGenerator.GenerateHospitalizationInProgress());

                    for (int i = 0; i < numberOfHospitalizations; i++)
                    {
                        patient.SetEndedHospitalization(DataGenerator.GenerateHospitalization());
                    }

                    _hospitalManager.AddPatientToHospital(hospitals[hospitalIndex], patient);
                    _hospitalManager.AddCurrentlyHospitalizedPatient(hospitals[hospitalIndex], patient);
                    _patientManager.AddNewRecord(patient);
                }
            }

            MessageBox.Show("Data generated successfully.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
