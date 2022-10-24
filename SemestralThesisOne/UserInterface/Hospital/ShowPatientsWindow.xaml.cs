using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for ShowPatientsWindow.xaml
    /// </summary>
    public partial class ShowPatientsWindow : Window
    {
        private HospitalManager hospitalManager;

        public ShowPatientsWindow(HospitalManager manager)
        {
            hospitalManager = manager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var patientsList = hospitalManager.
        }
    }
}
