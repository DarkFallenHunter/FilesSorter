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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace FilesSorter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Renamer renamer;
        Dictionary<string, string> typesOfFiles = new Dictionary<string, string>
        {
            { "photoType", "IMG" },
            { "videoType", "MOV" }
        };

        public MainWindow()
        {
            InitializeComponent();
            renamer = new Renamer();
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            renamer.SelectDirectory();
            textPuth.Text = renamer.FolderPath;
            if (textPuth.Text != "")
                sortButton.IsEnabled = true;
            else
                sortButton.IsEnabled = false;
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            renamer.SortFiles(prefixText.Text, ((ComboBoxItem)filesTypes.SelectedItem).Name);
            MessageBox.Show("Фотографии упорядочены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void filesTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (prefixText != null)
            {
                ComboBox comboBox = (ComboBox)sender;
                prefixText.Text = typesOfFiles[((ComboBoxItem)comboBox.SelectedItem).Name];
            }
        }
    }
}
