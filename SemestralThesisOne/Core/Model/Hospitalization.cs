using System;
using Structures.Interface;

namespace SemestralThesisOne.Core.Model
{
    public class Hospitalization : IData<Hospitalization>
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public string Diagnosis { get; set; }

        public const int DiagnosisLength = 20;

        public Hospitalization()
        {
            Id = -1;
            Start = default;
            End = null;
            Diagnosis = Helpers.PadOrTrimString("", DiagnosisLength);
        }


        public Hospitalization(DateTime start, DateTime? end, string diagnosis)
        {
            Start = start;
            End = end;
            Id = Random.Shared.Next();

            Diagnosis = Helpers.PadOrTrimString(diagnosis, DiagnosisLength);
        }

        public Hospitalization(int id ,DateTime start, DateTime? end, string diagnosis)
        {
            Start = start;
            End = end;
            Id = id;

            Diagnosis = Helpers.PadOrTrimString(diagnosis, DiagnosisLength);
        }

        public override string ToString()
        {
            if (End == null)
            {
                return $"ID: {Id} Start: {Start.ToShortDateString()} {Start.ToShortTimeString()} Diag: {Diagnosis}";
            }

            return $"ID: {Id} Start: {Start.ToShortDateString()} {Start.ToShortTimeString()} End: {End?.ToShortDateString()} {End?.ToShortTimeString()} Diag: {Diagnosis}";
        }

        public string ToCsvString()
        {
            return End.HasValue ? $"{Start.Ticks},{End.Value.Ticks},{Diagnosis}" : $"{Start.Ticks},{Diagnosis}";
        }

        public byte[] ToByteArray()
        {
            byte[] array = new byte[GetSize()];

            byte[] bytes = BitConverter.GetBytes(Id);
            int offset = 0;
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset = bytes.Length;

            bytes = BitConverter.GetBytes(Start.Ticks);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;

            bytes = BitConverter.GetBytes((End ?? default).Ticks);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;

            bytes = System.Text.Encoding.UTF8.GetBytes(Diagnosis);
            Buffer.BlockCopy(bytes, 0, array, offset, bytes.Length);

            return array;
        }

        public void FromByteArray(byte[] array)
        {
            byte[] data = new byte[sizeof(int)];
            int srcOffset = 0;
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(int));
            Id = BitConverter.ToInt32(data);
            srcOffset += sizeof(int);

            data = new byte[sizeof(long)];
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(long));
            Start = new DateTime(BitConverter.ToInt64(data));
            srcOffset += sizeof(long);

            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(long));
            DateTime end = new DateTime(BitConverter.ToInt64(data));
            End = new DateTime(BitConverter.ToInt64(data)) == default ? null : end;
            srcOffset += sizeof(long);

            data = new byte[DiagnosisLength];
            Buffer.BlockCopy(array, srcOffset, data, 0, DiagnosisLength);
            Diagnosis = System.Text.Encoding.UTF8.GetString(data);
        }

        public int GetSize()
        {
            return sizeof(int) + 2 * sizeof(long) + DiagnosisLength;
        }

        public int GetHash()
        {
            return Id;
        }

        public bool IsEqual(Hospitalization data)
        {
            return Id == data.Id;
        }

        public Hospitalization CreateClass()
        {
            return new Hospitalization();
        }
    }
}
