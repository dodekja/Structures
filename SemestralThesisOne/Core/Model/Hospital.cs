using Structures.Tree;
using System;
using System.Collections.Generic;

namespace SemestralThesisOne.Core.Model
{
    public class Hospital
    {
        public string Name { get; set; }

        private BinarySearchTree<string, Patient> _patientsById;
        private BinarySearchTree<string, Patient> _patientsByName;
        private BinarySearchTree<DateTime, Patient> _currentlyHospitalizedPatientsByDate;
        private BinarySearchTree<string, Patient> _currentlyHospitalizedPatientsById;
        private BinarySearchTree<string, Patient> _hospitalizedPatientsById;

        public BinarySearchTree<string, Hospitalization> Hospitalizations { get; set; }

        public Hospital(string name)
        {
            Name = name;
            Hospitalizations = new BinarySearchTree<string, Hospitalization>();
            _patientsById = new BinarySearchTree<string, Patient>();
            _patientsByName = new BinarySearchTree<string, Patient>();
            _currentlyHospitalizedPatientsByDate = new BinarySearchTree<DateTime, Patient>();
            _currentlyHospitalizedPatientsById = new BinarySearchTree<string, Patient>();
            _hospitalizedPatientsById = new BinarySearchTree<string, Patient>();
        }

        public void AddPatient(Patient patient)
        {
            _patientsById.Add(patient.IdentificationNumber, patient);
            _patientsByName.Add(patient.FirstName + patient.LastName + patient.IdentificationNumber, patient);
        }

        public List<Patient> GetAllPatients()
        {
            return _patientsById.InOrderData();
        }

        public List<Patient> GetAllHospitalizedPatients()
        {
            return _hospitalizedPatientsById.InOrderData();
        }

        public List<Patient> GetAllPatientsLevel()
        {
            return _patientsById.LevelOrderData();
        }

        public List<Patient> GetAllCurrentlyHospitalizedPatients()
        {
            return _currentlyHospitalizedPatientsByDate.InOrderData();
        }

        public void AddCurrentlyHospitalizedPatients(List<Patient> data)
        {
            foreach (var patient in data)
            {
                AddCurrentlyHospitalizedPatient(patient);
            }
        }

        public void AddPatients(List<Patient> data)
        {
            foreach (var patient in data)
            {
                AddPatient(patient);
            }
        }

        public List<Patient> GetAllCurrentlyHospitalizedPatientsById()
        {
            return _currentlyHospitalizedPatientsById.InOrderData();
        }

        public List<Patient> GetAllCurrentlyHospitalizedPatientsRange(DateTime rangeStart, DateTime rangeEnd)
        {
            return _currentlyHospitalizedPatientsByDate.FindRange(rangeStart, rangeEnd);
        }

        public Patient? GetPatient(string id)
        {
            return _patientsById.Find(id);
        }

        public void AddCurrentlyHospitalizedPatient(Patient patient)
        {
            if (patient.IsHospitalized())
            {
                _currentlyHospitalizedPatientsByDate.Add(patient.CurrentHospitalization.Start, patient);
                _currentlyHospitalizedPatientsById.Add(patient.IdentificationNumber, patient);
            }

            if (_hospitalizedPatientsById.FindNoThrow(patient.IdentificationNumber) == null)
            {
                _hospitalizedPatientsById.Add(patient.IdentificationNumber, patient);
            }
        }

        public void RemoveCurrentlyHospitalizedPatient(Patient patient)
        {
            _ = _currentlyHospitalizedPatientsByDate.Remove(patient.CurrentHospitalization.Start);
            _ = _currentlyHospitalizedPatientsById.Remove(patient.IdentificationNumber);
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Find all patients with the given name.
        /// </summary>
        /// <param name="patientName"></param>
        /// <returns></returns>
        public List<Patient?> GetPatientsRange(string patientName)
        {
            return _patientsByName.FindRange(patientName + "00000000000", patientName + "ZZZZZZZZZZZ");
        }

        public void Balance()
        {
            _patientsById.Balance();
            _patientsByName.Balance();
            _currentlyHospitalizedPatientsByDate.Balance();
            _currentlyHospitalizedPatientsById.Balance();
        }
    }
}