using Structures.Interface;
using System.Text;
using System;

namespace SemestralThesisOne.Core.Model
{
    public class Examination : IData<Examination>
    {
        /// <summary>
        /// Length of <see cref="PatientID"/>
        /// </summary>
        public const int IDLength = 10;

        /// <summary>
        /// Length of <see cref="Description"/>
        /// </summary>
        public const int DescriptionLength = 10;

        public int ID { get; set; }

        public string PatientID { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }

        public Examination(int id, string patientId, int value, string description)
        {
            ID = id;
            PatientID = Helpers.PadOrTrimString(patientId,IDLength);
            Value = value;
            Description = Helpers.PadOrTrimString(description,DescriptionLength);
        }

        public Examination(int id, string patientId)
        {
            ID = id;
            PatientID = Helpers.PadOrTrimString(patientId, IDLength);
            Value = -1;
            Description = Helpers.PadOrTrimString("", DescriptionLength);
        }

        public Examination()
        {
            ID = -1;
            PatientID = Helpers.PadOrTrimString("",IDLength);
            Value = -1;
            Description = Helpers.PadOrTrimString("",DescriptionLength);
        }


        public byte[] ToByteArray()
        {
            byte[] array = new byte[GetSize()];

            int offset = 0;

            byte[] bytes = BitConverter.GetBytes(ID);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;

            bytes = Encoding.UTF8.GetBytes(PatientID);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += IDLength;

            bytes = BitConverter.GetBytes(Value);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;

            bytes = Encoding.UTF8.GetBytes(Description);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);

            return array;
        }

        public void FromByteArray(byte[] array)
        {
            byte[] data = new byte[sizeof(int)];
            int srcOffset = 0;
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(int));
            ID = BitConverter.ToInt32(data);
            srcOffset += sizeof(int);

            data = new byte[IDLength];
            Buffer.BlockCopy(array, srcOffset, data, 0, IDLength);
            PatientID = Encoding.UTF8.GetString(data);
            srcOffset += IDLength;

            data = new byte[sizeof(int)];
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(int));
            Value = BitConverter.ToInt32(data);
            srcOffset += sizeof(int);

            data = new byte[DescriptionLength];
            Buffer.BlockCopy(array, srcOffset, data, 0, DescriptionLength);
            Description = Encoding.UTF8.GetString(data);
        }

        public int GetSize()
        {
            return sizeof(int) + IDLength + sizeof(int) + DescriptionLength;
        }

        public int GetHash()
        {
            byte[] integer = new byte[sizeof(int)];

            byte[] bytes = BitConverter.GetBytes(ID);
            integer[0] = bytes[0];
            integer[1] = bytes[1];

            bytes = Encoding.UTF8.GetBytes(PatientID);
            integer[2] = bytes[0];
            integer[3] = bytes[1];

            return BitConverter.ToInt32(integer);
        }

        public bool IsEqual(Examination data)
        {
            return ID == data.ID && PatientID == data.PatientID;
        }

        public Examination CreateClass()
        {
            return new Examination();
        }

        public override string ToString()
        {
            return $"ID: {ID} PatientID: {PatientID} Value: {Value} Description: {Description}";
        }
    }
}
