using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralThesisOne.Core.Model
{
    public class Hospitalization
    {
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public string Diagnosis { get; set; }

        public Hospitalization(DateTime start, DateTime? end, string diagnosis)
        {
            Start = start;
            End = end;
            Diagnosis = diagnosis;
        }

        public override string ToString()
        {
            if (End == null)
            {
                return $"Start: {Start.ToShortDateString()} {Start.ToShortTimeString()} Diag: {Diagnosis} ";
            }
            else
            {
                return $"Start: {Start.ToShortDateString()} {Start.ToShortTimeString()} End: {End?.ToShortDateString()} {End?.ToShortTimeString()} Diag: {Diagnosis} ";
            }
        }
    }
}
