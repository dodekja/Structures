using System;
using System.Collections.Generic;

namespace SemestralThesisOne.Core.Model
{
    public class Patient
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Enums.InsuranceCompanyCodes InsuranceCompanyCode { get; set; }

        //TODO: Change this to the augmented BST
        public List<Hospitalization> Hospitalizations { get; set; }

        public Patient(string firstName, string lastName, string identificationNumber, DateTime dateOfBirth, Enums.InsuranceCompanyCodes insuranceCompanyCode)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            DateOfBirth = dateOfBirth;
            InsuranceCompanyCode = insuranceCompanyCode;
            Hospitalizations = new List<Hospitalization>();
        }

        public override string ToString()
        {
            return $"Name: {FirstName} {LastName}, ID: {IdentificationNumber}," +
                   $" Date of Birth: {DateOfBirth.ToShortDateString()} Insurance: {InsuranceCompanyCode}";
        }
    }
}
