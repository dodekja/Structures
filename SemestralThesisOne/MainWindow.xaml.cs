using System.Windows;
using SemestralThesisOne.UserInterface;
using SemestralThesisOne.UserInterface.Hospital;

namespace SemestralThesisOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private App app;

        public MainWindow()
        {
            app = Application.Current as App;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void AddPatient(object sender, RoutedEventArgs e)
        {
            AddPatientWindow addPatientDialog = new AddPatientWindow(app.PatientManager);
            addPatientDialog.ShowDialog();
        }

        private void AddHospital(object sender, RoutedEventArgs e)
        {
            AddHospitalWindow addHospitalDialog = new AddHospitalWindow(app.HospitalManager);
            addHospitalDialog.ShowDialog();
        }

        private void ShowPatients(object sender, RoutedEventArgs e)
        {
            ShowPatientsWindow showPatients = new ShowPatientsWindow(app.HospitalManager);
            showPatients.ShowDialog();
        }
    }
}
