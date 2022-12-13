using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Structures.Interface;
using Structures.Tree;

namespace SemestralThesisOne.Core.Model
{
    public class Patient : IData<Patient>
    {
        public const int FirstNameLength = 15;
        public const int LastNameLength = 20;
        public const int IdLength = 10;
        public const int MaxHospitalizations = 10;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Enums.InsuranceCompanyCodes InsuranceCompanyCode { get; set; }

        public Hospitalization? CurrentHospitalization { get; set; }

        public BinarySearchTree<string, Hospitalization> HospitalizationsEnded { get; set; }

        public List<Hospitalization?> hospitalizationsForFile;

        public Patient()
        {
            FirstName = Helpers.PadOrTrimString("", FirstNameLength);
            LastName = Helpers.PadOrTrimString("", LastNameLength);
            IdentificationNumber = Helpers.PadOrTrimString("", IdLength);
            DateOfBirth = default;
            InsuranceCompanyCode = default;
            CurrentHospitalization = null;
            HospitalizationsEnded = new BinarySearchTree<string, Hospitalization>();
            hospitalizationsForFile = new List<Hospitalization?>(MaxHospitalizations);
            //for (int i = 0; i < MaxHospitalizations; i++)
            //{
            //    hospitalizationsForFile.Add(new Hospitalization());
            //}
        }

        public Patient(string identificationNumber)
        {
            FirstName = Helpers.PadOrTrimString("", FirstNameLength);
            LastName = Helpers.PadOrTrimString("", LastNameLength);
            IdentificationNumber = Helpers.PadOrTrimString(identificationNumber, IdLength);
            DateOfBirth = default;
            InsuranceCompanyCode = default;
            CurrentHospitalization = null;
            HospitalizationsEnded = new BinarySearchTree<string, Hospitalization>();
            hospitalizationsForFile = new List<Hospitalization?>(MaxHospitalizations);
            //for (int i = 0; i < MaxHospitalizations; i++)
            //{
            //    hospitalizationsForFile.Add(new Hospitalization());
            //}
        }

        public Patient(string firstName, string lastName, string identificationNumber, DateTime dateOfBirth, Enums.InsuranceCompanyCodes insuranceCompanyCode)
        {
            FirstName = Helpers.PadOrTrimString(firstName, FirstNameLength);
            LastName = Helpers.PadOrTrimString(lastName, LastNameLength);
            IdentificationNumber = Helpers.PadOrTrimString(identificationNumber, IdLength);
            DateOfBirth = dateOfBirth;
            InsuranceCompanyCode = insuranceCompanyCode;
            CurrentHospitalization = null;
            HospitalizationsEnded = new BinarySearchTree<string, Hospitalization>();
            hospitalizationsForFile = new List<Hospitalization?>(MaxHospitalizations);
            for (int i = 0; i < MaxHospitalizations; i++)
            {
                hospitalizationsForFile.Add(new Hospitalization());
            }
        }

        public override string ToString()
        {
            string str = $"Name: {FirstName} {LastName}, ID: {IdentificationNumber}," +
                         $" Date of Birth: {DateOfBirth.ToShortDateString()} Insurance: {InsuranceCompanyCode}";
            if (CurrentHospitalization != null)
            {
                str += " Current Hos.: ";
                str += CurrentHospitalization.ToString();
            }

            if (HospitalizationsEnded.Count > 0)
            {
                str += " Past Hos.: ";
                foreach (var hospitalization in HospitalizationsEnded.InOrderData())
                {
                    str += $"{hospitalization}";
                }
            }
            return str;
        }

        public string ToCsvString()
        {
            return $"{FirstName},{LastName},{IdentificationNumber},{DateOfBirth.Ticks},{InsuranceCompanyCode}";
        }

        public void AddCurrentHospitalization(DateTime start, string diagnosis)
        {
            CurrentHospitalization = new Hospitalization(start, null, diagnosis);
            hospitalizationsForFile.Add(CurrentHospitalization);
        }

        public void EndHospitalization(DateTime? end)
        {
            int index = hospitalizationsForFile.FindIndex(x => x.IsEqual(CurrentHospitalization));

            CurrentHospitalization.End = end;

            HospitalizationsEnded.Add(CurrentHospitalization.Start.ToShortDateString() + CurrentHospitalization.Start.ToShortTimeString() +
                                      CurrentHospitalization.End.Value.ToShortDateString() + CurrentHospitalization.End.Value.ToShortTimeString(),
                                        CurrentHospitalization);

            hospitalizationsForFile[index] = CurrentHospitalization;

            CurrentHospitalization = null;
        }

        public bool IsHospitalized()
        {
            return CurrentHospitalization != null;
        }

        public void Balance()
        {
            HospitalizationsEnded.Balance();
        }

        public void SetCurrentHospitalization(Hospitalization? hospitalization)
        {
            CurrentHospitalization = hospitalization;
            hospitalizationsForFile.Add(hospitalization);
        }

        public void SetEndedHospitalization(Hospitalization hospitalization)
        {
            HospitalizationsEnded.Add(hospitalization.Start.ToShortDateString() + hospitalization.Start.ToShortTimeString() +
                                      hospitalization.End.Value.ToShortDateString() + hospitalization.End.Value.ToShortTimeString(), hospitalization);

            hospitalizationsForFile.Add(hospitalization);
        }

        public void RemoveHospitalization(int hospitalizationId)
        {
            hospitalizationsForFile.RemoveAll(x => x?.Id == hospitalizationId);
        }

        public byte[] ToByteArray()
        {
            byte[] array = new byte[GetSize()];

            int offset = 0;

            byte[] bytes = Encoding.UTF8.GetBytes(FirstName);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += FirstNameLength;

            bytes = Encoding.UTF8.GetBytes(LastName);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += LastNameLength;

            bytes = Encoding.UTF8.GetBytes(IdentificationNumber);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += IdLength;

            bytes = BitConverter.GetBytes(DateOfBirth.Ticks);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;

            bytes[0] = Enums.GetByte(InsuranceCompanyCode);
            Buffer.BlockCopy(bytes, 0, array, offset, 1);
            offset += 1;

            for (int i = 0; i < MaxHospitalizations; i++)
            {

                if (i < hospitalizationsForFile.Count && hospitalizationsForFile[i] != null)
                {
                    bytes = hospitalizationsForFile[i]?.ToByteArray();
                }
                else
                {
                    bytes = new Hospitalization().ToByteArray();
                }
                Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
                offset += bytes.Length;
            }

            return array;
        }

        public void FromByteArray(byte[] array)
        {
            hospitalizationsForFile = new List<Hospitalization?>(MaxHospitalizations);
            HospitalizationsEnded = new BinarySearchTree<string, Hospitalization>();

            byte[] data = new byte[FirstNameLength];
            int srcOffset = 0;
            Buffer.BlockCopy(array, srcOffset, data, 0, FirstNameLength);
            FirstName = Encoding.UTF8.GetString(data);
            srcOffset += FirstNameLength;

            data = new byte[LastNameLength];
            Buffer.BlockCopy(array, srcOffset, data, 0, LastNameLength);
            LastName = Encoding.UTF8.GetString(data);
            srcOffset += LastNameLength;

            data = new byte[IdLength];
            Buffer.BlockCopy(array, srcOffset, data, 0, IdLength);
            IdentificationNumber = Encoding.UTF8.GetString(data);
            srcOffset += IdLength;

            data = new byte[sizeof(long)];
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(long));
            DateOfBirth = new DateTime(BitConverter.ToInt64(data));
            srcOffset += sizeof(long);

            data = new byte[sizeof(byte)];
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(byte));
            InsuranceCompanyCode = Enums.FromByte(data[0]);
            srcOffset += sizeof(byte);

            
            for (int i = 0; i < MaxHospitalizations; i++)
            {
                Hospitalization hospitalization = new Hospitalization();
                int sizeOfHospitalization = hospitalization.GetSize();
                data = new byte[sizeOfHospitalization];
                Buffer.BlockCopy(array, srcOffset, data, 0, sizeOfHospitalization);
                hospitalization.FromByteArray(data);
                if (hospitalization.IsEqual(new Hospitalization()))
                {
                    continue;
                }

                if (hospitalization.End == null)
                {
                    SetCurrentHospitalization(hospitalization);
                }
                else
                {
                    SetEndedHospitalization(hospitalization);
                }

                srcOffset += sizeOfHospitalization;
            }
        }

        public int GetSize()
        {
            return FirstNameLength + LastNameLength + IdLength + sizeof(long) + sizeof(byte) +
                   MaxHospitalizations * (sizeof(int) + 2 * sizeof(long) + Hospitalization.DiagnosisLength);
        }

        public int GetHash()
        {
            var bytes = Encoding.UTF8.GetBytes(IdentificationNumber);
            return BitConverter.ToInt32(bytes.Take(4).ToArray());
        }

        public bool IsEqual(Patient data)
        {
            return IdentificationNumber == data.IdentificationNumber;
        }

        public Patient CreateClass()
        {
            return new Patient();
        }

        public Hospitalization FindHospitalization(int hospitalizationId)
        {
            return hospitalizationsForFile.Find(x => x.Id == hospitalizationId);
        }
    }
}
