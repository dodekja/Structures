using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml.Linq;
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
    }
}
