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

        public static InsuranceCompanyCodes GetInsuranceCompanyCodeFromString(string insuranceCompany)
        {
            switch (insuranceCompany)
            {
                case "Dovera":
                    return InsuranceCompanyCodes.Dovera;
                case "VSZP":
                    return InsuranceCompanyCodes.VSZP;
                case "Union":
                    return InsuranceCompanyCodes.Union;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insuranceCompany), insuranceCompany, "Invalid insurance company name provided.");
            }
        }

        public static byte GetByte(InsuranceCompanyCodes code)
        {
            return (byte)code;
        }

        public static InsuranceCompanyCodes FromByte(byte code)
        {
            return (InsuranceCompanyCodes)code;
        }
    }
}
