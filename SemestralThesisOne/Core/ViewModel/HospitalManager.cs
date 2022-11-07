using System;
using System.Collections.Generic;
using SemestralThesisOne.Core.Database;
using SemestralThesisOne.Core.Model;

namespace SemestralThesisOne.Core.ViewModel
{
    public class HospitalManager
    {
        private Hospitals _hospitals;

        public HospitalManager()
        {
            _hospitals = new Hospitals();
        }

        public void AddNewRecord(string name)
        {
            Hospital newHospital = new Hospital(name);
            _hospitals.Add(newHospital);
        }

        public void AddNewRecord(Hospital hospital)
        {
            _hospitals.Add(hospital);
        }

        public void AddPatientToHospital(string hospitalName, Patient patient)
        {
            _hospitals.Get(hospitalName).AddPatient(patient);
        }

        /// <summary>
        /// Get all patients.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Patient> GetPatientsList(string name)
        {
            Hospital? hospital = _hospitals.Get(name);
            List<Tuple<string, Patient>> inOrderList = new List<Tuple<string, Patient>>();
            if (hospital != null)
            {
                inOrderList = hospital.GetAllPatients();
            }

            List<Patient> patients = new List<Patient>();
            foreach (var item in inOrderList)
            {
                patients.Add(item.Item2);
            }

            return patients;
        }

        public List<Patient> GetCurrentlyHospitalizedPatients(string hospitalName)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            List<Tuple<DateTime, Patient>> inOrderList = new List<Tuple<DateTime, Patient>>();
            if (hospital != null)
            {
                inOrderList = hospital.GetAllCurrentlyHospitalizedPatients();
            }

            List<Patient> patients = new List<Patient>();
            foreach (var item in inOrderList)
            {
                patients.Add(item.Item2);
            }

            return patients;
        }

        public List<Patient> GetCurrentlyHospitalizedPatientsById(string hospitalName)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            List<Tuple<string, Patient>> inOrderList = new List<Tuple<string, Patient>>();
            if (hospital != null)
            {
                inOrderList = hospital.GetAllCurrentlyHospitalizedPatientsById();
            }

            List<Patient> patients = new List<Patient>();
            foreach (var item in inOrderList)
            {
                patients.Add(item.Item2);
            }

            return patients;
        }

        public List<Patient> GetCurrentlyHospitalizedPatientsRangeByDate(string hospitalName, DateTime rangeStart, DateTime rangeEnd)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            List<Patient> patients = new List<Patient>();
            if (hospital != null)
            {
                patients = hospital.GetAllCurrentlyHospitalizedPatientsRange(rangeStart,rangeEnd);
            }

            return patients;
        }

        public Patient? GetPatientFromHospitalById(string hospitalName, string patientId)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            if (hospital != null)
            {
                return hospital.GetPatient(patientId);
            }
            else
            {
                throw new ArgumentException("Invalid hospital name");
            }
        }

        public List<Patient> GetPatientsFromHospitalByName(string hospitalName, string patientName)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            if (hospital != null)
            {
                return hospital.GetPatientsRange(patientName);
            }
            else
            {
                throw new ArgumentException("Invalid hospital name");
            }
        }

        public void AddCurrentlyHospitalizedPatient(string hospitalName, Patient patient)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            hospital.AddCurrentlyHospitalizedPatient(patient);
        }

        public void RemoveCurrentlyHospitalizedPatient(string hospitalName, Patient patient)
        {
            Hospital? hospital = _hospitals.Get(hospitalName);
            hospital.RemoveCurrentlyHospitalizedPatient(patient);
        }

        public List<Hospital> GetAllHospitals()
        {
            return _hospitals.GetAllHospitals();
        }

        public void Balance()
        {
            _hospitals.Balance();
        }

        public void RemoveHospital(string toDelete, string newDocumentationOwner)
        {
            _hospitals.RemoveHospital(toDelete, newDocumentationOwner);
        }
    }
}
