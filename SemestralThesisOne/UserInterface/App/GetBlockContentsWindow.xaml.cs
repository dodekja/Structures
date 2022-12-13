using System.Windows;
using SemestralThesisOne.Core.ViewModel;

namespace SemestralThesisOne.UserInterface.App
{
    /// <summary>
    /// Interaction logic for GetBlockContentsWindow.xaml
    /// </summary>
    public partial class GetBlockContentsWindow : Window
    {
        public GetBlockContentsWindow(PatientManager patientManager)
        {
            InitializeComponent();
            AllBlocksContentsTextBlock.Text = patientManager.GetAllBlockContentsFromFile();
        }
    }
}
