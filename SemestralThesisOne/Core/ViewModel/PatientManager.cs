using System;
using System.Windows;
using SemestralThesisOne.Core.Database;
using SemestralThesisOne.Core.Model;

namespace SemestralThesisOne.Core.ViewModel
{
    public class PatientManager
    {
        private Patients _patients;
        private PatientsFile _patientsFile;

        public PatientManager()
        {
            _patientsFile = null;
            _patients = new Patients();
        }

        public void AddNewRecord(Patient patient)
        {
            _patients.Add(patient);
            _patientsFile.AddPatient(patient);
        }

        public Patient? GetPatient(string id)
        {
            return _patients.GetPatient(id);
        }

        public Hospitalization? GetHospitalization(string patientId, int hospitalizationId)
        {
            return _patients.GetPatient(patientId)?.FindHospitalization(hospitalizationId);
        }

        public void LoadStaticPatientsFile(string filename)
        {
            _patientsFile = new PatientsFile(filename);
            MessageBox.Show($"Load complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LoadDynamicPatientsFile(string filename, string indexFileName)
        {
            _patientsFile = new PatientsFile(filename, indexFileName);
            MessageBox.Show($"Load complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void CreateStaticPatientsFile(string filename, int blockFactor, int numberOfBlocks)
        {
            _patientsFile = new PatientsFile(filename, blockFactor, numberOfBlocks);
            MessageBox.Show($"Creation complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void CreateDynamicPatientsFile(string filename, int blockFactor)
        {
            _patientsFile = new PatientsFile(filename, blockFactor);
            MessageBox.Show($"Creation complete", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Balance()
        {
            _patients.Balance();
        }
    }
}
