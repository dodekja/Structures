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

        public Patient? GetPatientFromFile(string id)
        {
            return _patientsFile.FindPatient(id);
        }

        public Hospitalization? GetHospitalization(string patientId, int hospitalizationId)
        {
            return _patientsFile.FindPatient(patientId)?.FindHospitalization(hospitalizationId);
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

        public void AddHospitalizationToFile(string patientId, DateTime start, string diagnosis)
        {
            _patientsFile.AddCurrentHospitalization(patientId, start, diagnosis);
            MessageBox.Show($"Hospitalization added successfully", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void EndCurrentHospitalizationInFile(string patientId, DateTime end)
        {
            _patientsFile.EndCurrentHospitalization(patientId, end);
            MessageBox.Show("Hospitalization added successfully", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void Balance()
        {
            _patients.Balance();
        }

        public void RemovePatient(string identificationNumber)
        {
            _patientsFile.Remove(identificationNumber);
            MessageBox.Show("Patient removed successfully", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SaveFile()
        {
            _patientsFile.SaveFile();
        }

        public void RemoveHospitalization(string patientID, int hospitalizationId)
        {
            _patientsFile.RemoveHospitalization(patientID, hospitalizationId);
        }

        public string GetAllBlockContentsFromFile()
        {
            return _patientsFile.GetAllBlockContents();
        }

        public Examination? FindExamination(int examinationID, string patientID)
        {
            return _patientsFile.FindExamination(examinationID, patientID);
        }

        public Examination RemoveExamination(int examinationID, string patientID)
        {
            return _patientsFile.RemoveExamination(examinationID, patientID);
            
        }
        
        public void AddExamination(Examination examination)
        {
            _patientsFile.AddExamination(examination);
            MessageBox.Show($"Examination added", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public string GetExaminationsFromFile()
        {
            return _patientsFile.GetExaminations();
        }
    }
}
