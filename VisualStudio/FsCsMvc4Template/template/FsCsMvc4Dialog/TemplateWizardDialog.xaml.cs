using System.Windows;
using System.Windows.Controls;

namespace FsCsMvc4Dialog
{
    public partial class TemplateWizardDialog : Window
    {
        public TemplateWizardDialog()
        {
            InitializeComponent();
        }

        public bool IncludeTestsProject { get; protected set; }
        public string SelectedViewEngine { get; protected set; }
        protected bool IsOnlyRazor { get; set; }
        public int SelectedProjectTypeIndex { get; protected set; }
        public string SelectedJsFramework { get; protected set; }
        public string SelectedUnitTestFramework { get; protected set; }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            IncludeTestsProject =
                cbIncludeTestsProject.IsChecked.HasValue ? cbIncludeTestsProject.IsChecked.Value : false;
            if (IsOnlyRazor)
            {
                SelectedViewEngine = "Razor";
                cbViewEngine.SelectedIndex = 1;
            }
            else
            {
                SelectedViewEngine = cbViewEngine.SelectionBoxItem.ToString();
            }
            SelectedProjectTypeIndex = lvwProjectType.SelectedIndex;
            SelectedJsFramework = ((ComboBoxItem)cbJsFramework.SelectedItem).Tag.ToString();
            SelectedUnitTestFramework = ((ComboBoxItem)cbUnitTestFramework.SelectedItem).Tag.ToString();
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvwProjectType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IsOnlyRazor = lvwProjectType.SelectedIndex == 1 || lvwProjectType.SelectedIndex == 2;
            cbViewEngine.IsEnabled = !IsOnlyRazor;
            cbJsFramework.Visibility = IsSpa() ? Visibility.Visible : Visibility.Collapsed;
            lblJsFramework.Visibility = cbJsFramework.Visibility;
        }

        private bool IsSpa()
        {
            return lvwProjectType.SelectedIndex == 2;
        }

        private void cbIncludeTestsProject_Click(object sender, RoutedEventArgs e)
        {
            spTestFramework.Visibility = cbIncludeTestsProject.IsChecked.HasValue && 
                cbIncludeTestsProject.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed;
        } 
    }
}
