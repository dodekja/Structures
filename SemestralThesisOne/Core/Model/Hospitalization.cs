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
    }
}
