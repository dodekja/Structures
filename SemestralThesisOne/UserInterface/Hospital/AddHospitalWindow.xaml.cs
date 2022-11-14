using System;
using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for AddHospitalWindow.xaml
    /// </summary>
    public partial class AddHospitalWindow : Window
    {
        private HospitalManager hospitalManager;

        public AddHospitalWindow(HospitalManager manager)
        {
            hospitalManager = manager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                try
                {
                    hospitalManager.AddNewRecord(NameTextBox.Text);
                    MessageBox.Show("Hospital added successfully.", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (ArgumentException argumentException)
                {
                    MessageBox.Show("Hospital already exists", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please enter hospital name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
