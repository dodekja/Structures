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
                }
                catch (ArgumentException argumentException)
                {
                    MessageBox.Show("Patient already exists", "Error",
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
