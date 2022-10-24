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

        public void AddNewRecord(string firstName, string lastName, string identificationNumber,
            DateTime dateOfBirth, Enums.InsuranceCompanyCodes insuranceCompanyCode)
        {
            Patient newPatient = new Patient(firstName, lastName, identificationNumber, dateOfBirth, insuranceCompanyCode);
            _patients.Add(newPatient);
        }
    }
}
