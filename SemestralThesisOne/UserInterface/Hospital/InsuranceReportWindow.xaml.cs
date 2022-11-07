using SemestralThesisOne.Core.ViewModel;
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

namespace SemestralThesisOne.UserInterface.Hospital
{
    /// <summary>
    /// Interaction logic for InsuranceReportWindow.xaml
    /// </summary>
    public partial class InsuranceReportWindow : Window
    {
        private HospitalManager _hospitalManager;

        public InsuranceReportWindow(HospitalManager hospitalManager)
        {
            _hospitalManager = hospitalManager;
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            //var patients = _hospitalManager.Get
        }
    }
}
