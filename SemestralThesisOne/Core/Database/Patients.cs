using System;
using SemestralThesisOne.Core.Model;
using Structures.Tree;

namespace SemestralThesisOne.Core.Database
{
    internal class Patients : ITable<Patient>
    {
        private BinarySearchTree<string, Patient> PatientsByID;

        public Patients()
        {
            PatientsByID = new BinarySearchTree<string, Patient>();
        }

        public void Add(Patient patient)
        {
            PatientsByID.Add(patient.IdentificationNumber,patient);
        }

        public void Balance()
        {
            PatientsByID.Balance();
            foreach (Tuple<string, Patient?> tuple in PatientsByID.InOrder())
            {
                tuple.Item2.Balance();
            }
        }
    }
}
