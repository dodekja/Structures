using Structures.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralThesisOne.Core.Model
{
    public class Hospital
    {
        public string Name { get; set; }

        private BinarySearchTree<string, Patient> _patientsById;

        //TODO: Change this to the augmented BST
        public BinarySearchTree<string,Hospitalization> Hospitalizations { get; set; }

        public Hospital(string name)
        {
            Name = name;
            Hospitalizations = new BinarySearchTree<string, Hospitalization>();
            _patientsById = new BinarySearchTree<string, Patient>();
        }

        public void AddPatient(Patient patient)
        {
            _patientsById.Add(patient.IdentificationNumber,patient);
        }

        public List<Tuple<string,Patient>> GetAllPatients()
        {
            return _patientsById.InOrder();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
