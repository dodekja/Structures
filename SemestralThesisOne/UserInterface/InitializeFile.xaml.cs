using SemestralThesisOne.Core.ViewModel;
using System.Windows;

namespace SemestralThesisOne.UserInterface
{
    /// <summary>
    /// Interaction logic for InitializeFile.xaml
    /// </summary>
    public partial class InitializeFile : Window
    {
        public PatientManager PatientManager { get; set; }

        public InitializeFile(PatientManager patientManager)
        {
            PatientManager = patientManager;

            InitializeComponent();
        }

        private void CreateStaticFile_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.CreateStaticPatientsFile(FileNameTextBox.Text,int.Parse(BlockFactorTextBox.Text), int.Parse(NumberOfBlocksTextBox.Text));
        }

        private void LoadStaticFile_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.LoadStaticPatientsFile(FileNameTextBox.Text);
        }

        private void LoadDynamicFile_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.LoadDynamicPatientsFile(FileNameTextBox.Text, IndexFileNameTextBox.Text);
        }

        private void CreateDynamicFile_OnClick(object sender, RoutedEventArgs e)
        {
            PatientManager.CreateDynamicPatientsFile(FileNameTextBox.Text, int.Parse(BlockFactorTextBox.Text));
        }
    }
}