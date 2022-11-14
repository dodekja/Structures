using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
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
            _tree.Add(hospital.Name, hospital);
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

        public List<Hospital> GetAllHospitals()
        {
            var hospitals = _tree.InOrderData();
            return hospitals;
        }

        public void Balance()
        {
            _tree.Balance();
            foreach (var hospital in _tree.InOrder())
            {
                hospital.Item2.Balance();
            }
        }

        public void RemoveHospital(string toDelete, string newDocumentationOwnerName)
        {
            Hospital? deleted = null;
            Hospital? newOwner = null;
            try
            {
                deleted = _tree.Remove(toDelete);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Hospital {toDelete} does not exist", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            try
            {
                newOwner = Get(newDocumentationOwnerName);
            }
            catch (Exception e)
            {
                _tree.Add(deleted.Name,deleted);
                MessageBox.Show($"Hospital {newDocumentationOwnerName} does not exist", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            newOwner.AddPatients(deleted.GetAllPatients());
            newOwner.AddCurrentlyHospitalizedPatients(deleted.GetAllCurrentlyHospitalizedPatients());
        }

        public void Save()
        {
            StringBuilder hospitalsText = new StringBuilder();
            StringBuilder patientsText = new StringBuilder();
            StringBuilder currentHospitalizationsText = new StringBuilder();
            StringBuilder endedHospitalizationsText = new StringBuilder();
            var levelOrder = _tree.LevelOrderData();
            foreach (Hospital hospital in levelOrder)
            {
                hospitalsText.AppendLine($"{hospital.ToString()}");
                var patients = hospital.GetAllPatientsLevel();
                foreach (var patient in patients)
                {
                    patientsText.AppendLine($"{hospital.ToString()},{patient.ToCsvString()}");
                    if (patient.IsHospitalized())
                    {
                        currentHospitalizationsText.AppendLine(
                            $"{hospital.Name},{patient.IdentificationNumber},{patient.CurrentHospitalization.ToCsvString()}");
                    }

                    foreach (var endedHospitalization in patient.HospitalizationsEnded.LevelOrderData())
                    {
                        endedHospitalizationsText.AppendLine($"{patient.IdentificationNumber},{endedHospitalization.ToCsvString()}");
                    }
                }
            }
            File.WriteAllText(@"..\..\..\Data\hospitals.csv",hospitalsText.ToString());
            File.WriteAllText(@"..\..\..\Data\patients.csv", patientsText.ToString());
            File.WriteAllText(@"..\..\..\Data\current_hospitalizations.csv", currentHospitalizationsText.ToString());
            File.WriteAllText(@"..\..\..\Data\ended_hospitalizations.csv", endedHospitalizationsText.ToString());
            
        }
    }
}
