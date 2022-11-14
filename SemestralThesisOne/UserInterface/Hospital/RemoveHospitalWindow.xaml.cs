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
    /// Interaction logic for RemoveHospitalWindow.xaml
    /// </summary>
    public partial class RemoveHospitalWindow : Window
    {
        private HospitalManager _hospitalManager;

        public RemoveHospitalWindow(HospitalManager hospitalManager)
        {
            InitializeComponent();
            _hospitalManager = hospitalManager;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            _hospitalManager.RemoveHospital(DeleteTextBox.Text, NewOwnerTextBox.Text);
            MessageBox.Show($"Hospital {DeleteTextBox.Text} removed successfully, data moved to the {NewOwnerTextBox.Text} hospital.", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
