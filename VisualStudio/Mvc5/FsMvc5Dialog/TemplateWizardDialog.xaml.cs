using System.Windows;
using System.Windows.Controls;

namespace FsMvc5Dialog
{
    public partial class TemplateWizardDialog : Window
    {
        public TemplateWizardDialog()
        {
            InitializeComponent();
        }

        public int SelectedProjectTypeIndex { get; protected set; }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedProjectTypeIndex = lvwProjectType.SelectedIndex;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
