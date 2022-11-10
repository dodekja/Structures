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

        public List<Tuple<string, Patient>> GetAllPatients()
        {
            return _patientsById.InOrder();
        }

        public List<BinaryTreeNode<string, Patient>> GetAllPatientsLevel()
        {
            return _patientsById.LevelOrder();
        }

        public List<Tuple<DateTime, Patient>> GetAllCurrentlyHospitalizedPatients()
        {
            return _currentlyHospitalizedPatientsByDate.InOrder();
        }

        public void AddCurrentlyHospitalizedPatients(List<Tuple<DateTime, Patient>> data)
        {
            foreach (var tuple in data)
            {
                AddCurrentlyHospitalizedPatient(tuple.Item2);
            }
        }

        public void AddPatients(List<Tuple<string, Patient>> data)
        {
            foreach (var tuple in data)
            {
                AddPatient(tuple.Item2);
            }
        }

        public List<Tuple<string, Patient>> GetAllCurrentlyHospitalizedPatientsById()
        {
            return _currentlyHospitalizedPatientsById.InOrder();
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