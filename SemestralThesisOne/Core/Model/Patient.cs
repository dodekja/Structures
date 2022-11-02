using System;
using Structures.Tree;

namespace SemestralThesisOne.Core.Model
{
    public class Patient
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Enums.InsuranceCompanyCodes InsuranceCompanyCode { get; set; }

        public Hospitalization? CurrentHospitalization { get; set; }

        public BinarySearchTree<string, Hospitalization> HospitalizationsEnded { get; set; }

        public Patient(string firstName, string lastName, string identificationNumber, DateTime dateOfBirth, Enums.InsuranceCompanyCodes insuranceCompanyCode)
        {
            FirstName = firstName;
            LastName = lastName;
            IdentificationNumber = identificationNumber;
            DateOfBirth = dateOfBirth;
            InsuranceCompanyCode = insuranceCompanyCode;
            CurrentHospitalization = null;
            HospitalizationsEnded = new BinarySearchTree<string, Hospitalization>();
        }

        public override string ToString()
        {
            string str = $"Name: {FirstName} {LastName}, ID: {IdentificationNumber}," +
                         $" Date of Birth: {DateOfBirth.ToShortDateString()} Insurance: {InsuranceCompanyCode}";
            if (CurrentHospitalization != null)
            {
                str += " Current Hos.: ";
                    str += CurrentHospitalization.ToString();
            }

            if (HospitalizationsEnded.Count > 0)
            {
                str += " Past Hos.: ";
                foreach (var tuple in HospitalizationsEnded.InOrder())
                {
                    str += $"{tuple.Item2.ToString()}";
                }
            }
            return str;
        }

        public void AddCurrentHospitalization(DateTime start, string diagnosis)
        {
            CurrentHospitalization = new Hospitalization(start, null, diagnosis);
        }

        public void EndHospitalization(DateTime? end)
        {
            CurrentHospitalization.End = end;
            HospitalizationsEnded.Add(CurrentHospitalization.Start.ToShortDateString() + CurrentHospitalization.Start.ToShortTimeString() +
                                      CurrentHospitalization.End.Value.ToShortDateString() + CurrentHospitalization.End.Value.ToShortTimeString(),
                                        CurrentHospitalization);
            CurrentHospitalization = null;
        }

        public bool IsHospitalized()
        {
            return CurrentHospitalization != null;
        }
    }
}
