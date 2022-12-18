using System;
using System.Windows;
using SemestralThesisOne.Core.Model;
using SemestralThesisOne.Core.ViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace SemestralThesisOne.UserInterface.Examination
{
    /// <summary>
    /// Interaction logic for ExaminationsWindow.xaml
    /// </summary>
    public partial class ExaminationsWindow : Window
    {
        PatientManager _patientManager;

        public ExaminationsWindow(PatientManager patientManager)
        {
            _patientManager = patientManager;
            InitializeComponent();
        }

        private void RemoveButton_OnClickButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var examination = _patientManager.RemoveExamination(int.Parse(ExaminationIdTextBox.Text), PatientIdTextBox.Text);
                ExaminationTextBlock.Text = examination.ToString() + "removed";
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Examination not found", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Core.Model.Examination examination = new(int.Parse(ExaminationIdTextBox.Text), PatientIdTextBox.Text, int.Parse(ValueTextBox.Text), DescriptionTextBox.Text);
                _patientManager.AddExamination(examination);
                ExaminationTextBlock.Text = examination.ToString();

            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Examination already exists", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FindButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var examination = _patientManager.FindExamination(int.Parse(ExaminationIdTextBox.Text), PatientIdTextBox.Text);
                if (examination != null)
                {
                    ExaminationTextBlock.Text = examination.ToString();
                }
                else
                {
                    MessageBox.Show("Examination not found", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Examination not found", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ContentsButton_OnClick(object sender, RoutedEventArgs e)
        {
            ExaminationTextBlock.Text = _patientManager.GetExaminationsFromFile();
        }
    }
}
