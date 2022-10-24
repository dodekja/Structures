using Structures.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralThesisOne.Core.Model
{
    internal class Hospital
    {
        public string Name { get; set; }

        private BinarySearchTree<string, Patient> _patientsById;

        //TODO: Change this to the augmented BST
        public List<Hospitalization> Hospitalizations { get; set; }

        public Hospital(string name)
        {
            Name = name;
            Hospitalizations = new List<Hospitalization>();
            _patientsById = new BinarySearchTree<string, Patient>();
        }

        public void AddPatient(Patient patient)
        {
            _patientsById.Add(patient.IdentificationNumber,patient);
        }
    }
}
