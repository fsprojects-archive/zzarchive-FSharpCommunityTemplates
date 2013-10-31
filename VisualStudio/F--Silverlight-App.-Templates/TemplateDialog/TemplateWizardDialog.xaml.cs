using System.Windows;

namespace FSSLOnlyDialog
{
    public partial class TemplateWizardDialog : Window
    {
        public TemplateWizardDialog()
        {
            InitializeComponent();
        }

        public string SelectedSilverlightVersion { get; protected set; }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedSilverlightVersion = cbSilverlightVersion.SelectionBoxItem.ToString();
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
