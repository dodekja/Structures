using System;
using SemestralThesisOne.Core.Model;
using Structures.File;

namespace SemestralThesisOne.Core.Database
{
    internal class PatientsFile
    {
        private AbstractHashFile<Patient> _patients;

        /// <summary>
        /// Creates new dynamic hash file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="blockFactor"></param>
        public PatientsFile(string fileName, int blockFactor)
        {
            _patients = new DynamicHashFile<Patient>(fileName, blockFactor);
        }

        /// <summary>
        /// Loads dynamic hash file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="indexFileName"></param>
        public PatientsFile(string fileName, string indexFileName)
        {
            _patients = new DynamicHashFile<Patient>(fileName, indexFileName);
        }

        /// <summary>
        /// Loads static hash file.
        /// </summary>
        /// <param name="fileName"></param>
        public PatientsFile(string fileName)
        {
            _patients = new StaticHashFile<Patient>(fileName);
        }

        /// <summary>
        /// Creates new static hash file.
        /// </summary>
        /// <param name="fileName"></param>
        public PatientsFile(string fileName, int blockFactor, int numberOfBlocks)
        {
            _patients = new StaticHashFile<Patient>(fileName, blockFactor, numberOfBlocks);
        }

        public Patient FindPatient(string identificationNumber)
        {
            Patient patient = new Patient(identificationNumber);
            if (_patients is StaticHashFile<Patient> file)
            {
                patient = file.Find(patient);
            }
            else
            {
                patient = (_patients as DynamicHashFile<Patient>)!.Find(patient);
            }

            return patient;
        }

        public Patient Remove(string identificationNumber)
        {
            Patient patient = new Patient(identificationNumber);
            if (_patients is StaticHashFile<Patient> file)
            {
                patient = file.Delete(patient);
            }
            else
            {
                patient = (_patients as DynamicHashFile<Patient>)!.Delete(patient);
            }

            return patient;
        }

        public Hospitalization FindHospitalization(string patientId, int hospitalizationId)
        {
            Patient patient = FindPatient(patientId);
            return patient.FindHospitalization(hospitalizationId);
        }

        public void AddCurrentHospitalization(string patientId, DateTime start, string diagnosis)
        {
            Patient patient = Remove(patientId);
            patient.AddCurrentHospitalization(start, diagnosis);
            _patients.Insert(patient);
        }

        public void EndCurrentHospitalization(string patientId, DateTime end)
        {
            Patient patient = Remove(patientId);
            if (patient.IsHospitalized())
            {
               patient.EndHospitalization(end);
            }
            _patients.Insert(patient);
        }

        public void AddPatient(Patient patient)
        {
            _patients.Insert(patient);
        }

        public void RemoveHospitalization(string patientId, int hospitalizationId)
        {
            Patient patient = Remove(patientId);
            patient.RemoveHospitalization(hospitalizationId);
            _patients.Insert(patient);
        }

        public void RemovePatient(Patient patient)
        {
            _patients.Delete(patient);
        }
    }
}
