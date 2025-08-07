using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackgroundRemover.Services
{
    public class BackgroundRemoverService
    {
        private readonly string _pythonScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "bgRemover.py");
        private readonly string _pythonExePath;

        public BackgroundRemoverService()
        {
            _pythonExePath = FindPythonPath();
        }

        public async Task<string> RemoveBackgroundAsync(string inputPath, string outputPath)
        {
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input image not found", inputPath);

            if (!File.Exists(_pythonScriptPath))
                throw new FileNotFoundException("Python script not found", _pythonScriptPath);

            await Task.Run(() => ExecutePythonScript(inputPath, outputPath));

            if (!File.Exists(outputPath))
                throw new Exception("Background removal failed - no output file created");

            return outputPath;
        }

        private void ExecutePythonScript(string inputPath, string outputPath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = _pythonExePath,
                Arguments = $"\"{_pythonScriptPath}\" \"{inputPath}\" \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(_pythonScriptPath)
            };

            using (var process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (File.Exists(outputPath))
                {
                    // Replace this line in ExecutePythonScript method:
                    //StatusText.Text = "Background removed successfully!";

                    // With this thread-safe version:
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    StatusText.Text = "Background removed successfully!";
                    //});
                    //MessageBox.Show("Background removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Optionally, you can display the output image in the UI
                    // ResultImage.Source = new BitmapImage(new Uri(outputPath));
                }
                else
                {
                    MessageBox.Show($"Error: Failed");
                }

                //if (process.ExitCode != 0)
                //    throw new Exception($"Python script failed: {error}");
            }
        }

        private static string FindPythonPath()
        {
            var paths = new[]
            {
                "python",
                "python3",
                @"C:\Python310\python.exe",
                @"C:\Program Files\Python310\python.exe",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                           "Programs\\Python\\Python310\\python.exe")
            };

            foreach (var path in paths)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(psi))
                    {
                        string version = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        if (process.ExitCode == 0 && version.Contains("Python 3"))
                            return path;
                    }
                }
                catch { /* Try next path */ }
            }

            throw new Exception("Python 3 not found. Please install Python 3.10+ and add it to PATH");
        }
    }
}
