using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowHospitalsWindow.xaml
    /// </summary>
    public partial class ShowHospitalsWindow : Window
    {
        private HospitalManager _hospitalManager;

        public ShowHospitalsWindow(HospitalManager hospitalManager)
        {
            InitializeComponent();
            _hospitalManager = hospitalManager;
            var hospitals = hospitalManager.GetAllHospitals();
            foreach (Core.Model.Hospital hospital in hospitals)
            {
                HospitalsTextBlock.Text += $"Name: {hospital.ToString()} \n";
            }
        }
    }
}
