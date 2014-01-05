using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MultiProjectDialog
{
    public partial class TemplateWizardDialog : Window
    {
        public int SelectedProjectTypeIndex { get; protected set; }

        public TemplateWizardDialog()
        {
            InitializeComponent();
        }

        public TemplateWizardDialog(IEnumerable<Tuple<string, string, string>> projects)
        {
            InitializeComponent();
            lvwProjectType.Items.Clear();

            foreach (var project in projects)
            {
                // Get the project related info
                var displayText = project.Item2;
                var iconFileName = project.Item3.ToLower() == "fsharp" ? "MultiProjectDialog.images.file-icon.png" : "MultiProjectDialog.images.generic-file-icon.png";

                // Build the image
                var iconImage = new Image();
                using (var fileStream = GetType().Assembly.GetManifestResourceStream(iconFileName))
                {
                    var bitmapDecoder = new PngBitmapDecoder(fileStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    var imageSource = bitmapDecoder.Frames[0];
                    iconImage.Source = imageSource;
                }
                Grid.SetRow(iconImage, 0);

                // Build the label
                var label = new Label { Content = displayText, HorizontalAlignment = HorizontalAlignment.Center };
                Grid.SetRow(label, 1);

                // Build the Grid
                var grid = new Grid { Width = 96 };
                var row1 = new RowDefinition();
                var row2 = new RowDefinition();
                grid.RowDefinitions.Add(row1);
                grid.RowDefinitions.Add(row2);

                grid.Children.Add(label);
                grid.Children.Add(iconImage);

                var content = new ContentControl { Content = grid };

                var listItem = new ListViewItem { Content = content };

                lvwProjectType.Items.Add(listItem);
            }
        }

        private void SelectAndClose()
        {
            SelectedProjectTypeIndex = lvwProjectType.SelectedIndex;
            DialogResult = true;
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectAndClose();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void lvwProjectType_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectAndClose();
        }
    }
}
