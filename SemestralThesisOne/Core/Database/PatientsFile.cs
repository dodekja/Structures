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
        /// <param name="fileName">name of the binary file used</param>
        /// <param name="blockFactor">Max number of items in a single block</param>
        /// <param name="numberOfBlocks">Number of blocks in file</param>
        public PatientsFile(string fileName, int blockFactor, int numberOfBlocks)
        {
            _patients = new StaticHashFile<Patient>(fileName, blockFactor, numberOfBlocks);
        }

        public Patient FindPatient(string identificationNumber)
        {
            Patient patient = new Patient(identificationNumber);
            if (_patients is StaticHashFile<Patient> file)
            {
                return file.Find(patient);
            }

            return (_patients as DynamicHashFile<Patient>)!.Find(patient);
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

        private (Patient record, int index, int blockAddress) GetPatientForUpdate(string patientId)
        {
            Patient patient = new Patient(patientId);
            if (_patients is StaticHashFile<Patient> file)
            {
                return file.GetRecordForUpdate(patient);
            }

            return (_patients as DynamicHashFile<Patient>)!.GetRecordForUpdate(patient);
        }

        public Hospitalization FindHospitalization(string patientId, int hospitalizationId)
        {
            Patient patient = FindPatient(patientId);
            return patient.FindHospitalization(hospitalizationId);
        }

        public void AddCurrentHospitalization(string patientId, DateTime start, string diagnosis)
        {
            var patientUpdateInfo = GetPatientForUpdate(patientId); 

            patientUpdateInfo.record.AddCurrentHospitalization(start, diagnosis);

            _patients.UpdateRecord(patientUpdateInfo.record,patientUpdateInfo.index,patientUpdateInfo.blockAddress);
        }

        public void EndCurrentHospitalization(string patientId, DateTime end)
        {
            var patientUpdateInfo = GetPatientForUpdate(patientId);
            
            if (patientUpdateInfo.record.IsHospitalized())
            {
               patientUpdateInfo.record.EndHospitalization(end);
            }

            _patients.UpdateRecord(patientUpdateInfo.record, patientUpdateInfo.index, patientUpdateInfo.blockAddress);
        }

        public void AddPatient(Patient patient)
        {
            if (_patients is DynamicHashFile<Patient> file)
            {
                file.Insert(patient);
            }
            else
            {
                (_patients as StaticHashFile<Patient>).Insert(patient);
            }
        }

        public void RemoveHospitalization(string patientId, int hospitalizationId)
        {
            var patientUpdateInfo = GetPatientForUpdate(patientId);

            patientUpdateInfo.record.RemoveHospitalization(hospitalizationId);

            _patients.UpdateRecord(patientUpdateInfo.record, patientUpdateInfo.index, patientUpdateInfo.blockAddress);
        }

        public void SaveFile()
        {
            if (_patients is DynamicHashFile<Patient> file)
            {
               file.SaveIndex();
            }
        }

        public string GetAllBlockContents()
        {
            return _patients.GetAllBlockContents();
        }
    }
}
