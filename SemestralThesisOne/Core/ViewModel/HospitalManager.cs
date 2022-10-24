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

        public void AddPatientToHospital(string hospitalName, Patient patient)
        {
            _hospitals.Get(hospitalName).AddPatient(patient);
        }

        public void GetPatientsList()
        {
            
        }
    }
}
