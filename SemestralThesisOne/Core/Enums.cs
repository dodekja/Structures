using System;

namespace SemestralThesisOne.Core
{
    public class Enums
    {
        public enum InsuranceCompanyCodes
        {
            Dovera = 24, 
            VSZP = 25,
            Union = 27
        }

        public static string GetStringFromInsuranceCompanyCode(InsuranceCompanyCodes code)
        {
            switch (code)
            {
                case InsuranceCompanyCodes.Dovera:
                    return "Dovera";
                case InsuranceCompanyCodes.VSZP:
                    return "VSZP";
                case InsuranceCompanyCodes.Union:
                    return "Union";
                default:
                    throw new ArgumentOutOfRangeException(nameof(code), code, "Invalid insurance company code provided.");
            }
        }
    }
}
