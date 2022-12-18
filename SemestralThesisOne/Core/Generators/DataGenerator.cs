using System;
using SemestralThesisOne.Core.Model;

namespace SemestralThesisOne.Core.Generators
{
    internal static class DataGenerator
    {
        public static Patient GeneratePatient()
        {
            string firstName = StringGenerator.GenerateRandomString(20);
            string lastName = StringGenerator.GenerateRandomString(20);
            string identificationNumber = StringGenerator.GenerateRandomString(11);
            DateTime dateOfBirth = DateTimeGenerator.GenerateRandomDateTime(new DateTime(1970, 1, 1), DateTime.Now);
            int randomNumber = Random.Shared.Next(0, 3);
            Enums.InsuranceCompanyCodes code = randomNumber switch
            {
                0 => Enums.InsuranceCompanyCodes.VSZP,
                1 => Enums.InsuranceCompanyCodes.Dovera,
                _ => Enums.InsuranceCompanyCodes.Union
            };
            return new Patient(firstName, lastName, identificationNumber, dateOfBirth, code);
        }

        public static Hospital GenerateHospital()
        {
            return new Hospital(StringGenerator.GenerateRandomString(10));
        }

        public static Hospitalization GenerateHospitalization()
        {
            string diagnosis = StringGenerator.GenerateRandomString(20);
            DateTime start = DateTimeGenerator.GenerateRandomDateTime(new DateTime(1970, 1, 1), DateTime.Now);
            DateTime end = DateTimeGenerator.GenerateRandomDateTime(new DateTime(1970, 1, 1), DateTime.Now);
            return new Hospitalization(start, end, diagnosis);
        }

        public static Hospitalization GenerateHospitalizationInProgress()
        {
            string diagnosis = StringGenerator.GenerateRandomString(5);
            DateTime start = DateTimeGenerator.GenerateRandomDateTime(new DateTime(1970, 1, 1), DateTime.Now);
            return new Hospitalization(start, null, diagnosis);
        }
    }
}
