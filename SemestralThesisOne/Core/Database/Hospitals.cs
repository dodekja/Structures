using System;
using SemestralThesisOne.Core.Model;
using Structures.Tree;

namespace SemestralThesisOne.Core.Database
{
    internal class Hospitals : ITable<Hospital>
    {
        private BinarySearchTree<string, Hospital> _tree;

        public Hospitals()
        {
            _tree = new BinarySearchTree<string, Hospital>();
        }

        public void Add(Hospital hospital)
        {
            _tree.Add(hospital.Name,hospital);
        }

        public Hospital Get(string name)
        {
            Hospital? hospital = _tree.Find(name);
            if (hospital != null)
            {
                return hospital;
            }

            throw new InvalidOperationException($"Hospital with the name {name} does not exist.");
        }
    }
}
