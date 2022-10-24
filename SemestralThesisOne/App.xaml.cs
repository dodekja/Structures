using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SemestralThesisOne.Core.Database;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal HospitalManager HospitalManager { get; set; }
        internal PatientManager PatientManager { get; set; }

        public App()
        {
            HospitalManager = new HospitalManager();
            PatientManager = new PatientManager();
        }
    }
}
