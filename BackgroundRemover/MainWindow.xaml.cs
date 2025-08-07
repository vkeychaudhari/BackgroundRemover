using BackgroundRemover.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace BackgroundRemover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _selectedImagePath;
        private readonly BackgroundRemoverService _removerService = new BackgroundRemoverService();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                DisplayImage(OriginalImage, _selectedImagePath);
                ProcessButton.IsEnabled = true;
                StatusText.Text = "Image loaded. Click 'Remove Background' to process.";
            }
        }

        private async void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedImagePath) || !File.Exists(_selectedImagePath))
            {
                MessageBox.Show("Please select an image first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProcessButton.IsEnabled = false;
            StatusText.Text = "Processing... Please wait...";

            try
            {
                string outputPath = Path.Combine(
                    Path.GetDirectoryName(_selectedImagePath),
                    Path.GetFileNameWithoutExtension(_selectedImagePath) + "_nobg.png");

                await _removerService.RemoveBackgroundAsync(_selectedImagePath, outputPath);

                DisplayImage(ResultImage, outputPath);
                StatusText.Text = "Background removed successfully!";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
                MessageBox.Show(ex.Message, "Processing Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ProcessButton.IsEnabled = true;
            }
        }

        private void DisplayImage(System.Windows.Controls.Image imageControl, string imagePath)
        {
            if (!File.Exists(imagePath)) return;

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze(); // Important for cross-thread operations

            imageControl.Source = bitmap;
        }
    }
}
