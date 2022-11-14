using System;
using SemestralThesisOne.Core.Database;
using SemestralThesisOne.Core.Model;

namespace SemestralThesisOne.Core.ViewModel
{
    public class PatientManager
    {
        private Patients _patients;

        public PatientManager()
        {
            _patients = new Patients();
        }

        public void AddNewRecord(Patient patient)
        {
            _patients.Add(patient);
        }

        public Patient? GetPatient(string id)
        {
            return _patients.GetPatient(id);
        }

        public void Balance()
        {
            _patients.Balance();
        }
    }
}
