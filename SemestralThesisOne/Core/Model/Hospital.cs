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
        private BinarySearchTree<DateTime, Patient> _currentlyHospitalizedPatients;

        public BinarySearchTree<string, Hospitalization> Hospitalizations { get; set; }

        public Hospital(string name)
        {
            Name = name;
            Hospitalizations = new BinarySearchTree<string, Hospitalization>();
            _patientsById = new BinarySearchTree<string, Patient>();
            _patientsByName = new BinarySearchTree<string, Patient>();
            _currentlyHospitalizedPatients = new BinarySearchTree<DateTime, Patient>();
        }

        public void AddPatient(Patient patient)
        {
            _patientsById.Add(patient.IdentificationNumber,patient);
            _patientsByName.Add(patient.FirstName + patient.LastName + patient.IdentificationNumber,patient);
        }

        public List<Tuple<string,Patient>> GetAllPatients()
        {
            return _patientsById.InOrder();
        }

        public List<Tuple<DateTime, Patient>> GetAllCurrentlyHospitalizedPatients()
        {
            return _currentlyHospitalizedPatients.InOrder();
        }

        public Patient? GetPatient(string id)
        {
            return _patientsById.Find(id);
        }

        public void AddCurrentlyHospitalizedPatient(Patient patient)
        {
            if (patient.IsHospitalized())
            {
                _currentlyHospitalizedPatients.Add(patient.CurrentHospitalization.Start, patient);
            }
        }

        public void RemoveCurrentlyHospitalizedPatient(Patient patient)
        {
            _ = _currentlyHospitalizedPatients.Remove(patient.CurrentHospitalization.Start);
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
            //TODO: Change FindPatients By Name
            return _patientsByName.FindRange(patientName + "00000000000", patientName + "ZZZZZZZZZZZ");
        }
    }
}
