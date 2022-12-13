using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowHospitalsWindow.xaml
    /// </summary>
    public partial class ShowHospitalsWindow : Window
    {
        public ShowHospitalsWindow(HospitalManager hospitalManager)
        {
            InitializeComponent();
            HospitalsTextBlock.Text = "";
            var hospitals = hospitalManager.GetAllHospitals();
            foreach (Core.Model.Hospital hospital in hospitals)
            {
                HospitalsTextBlock.Text += $"Name: {hospital.ToString()} \n";
            }
        }
    }
}
