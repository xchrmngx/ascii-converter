using System;
using System.Drawing;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace ascii_converter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string asciiChars = "@%#*+=-:. ";
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OpenImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                Bitmap image = new Bitmap(openFileDialog.FileName);

                Bitmap resizedImage = ResizeImage(image, 150); // ширина арта (для компактности)
                string asciiArt = ConvertToAscii(resizedImage);
                AsciiTextBox.Text = asciiArt;
            }
        }

        private string ConvertToAscii(Bitmap image)
        {
            StringBuilder asciiArt = new StringBuilder();
            for (int y = 0; y < image.Height; y += 3) // тоже компактность, но высота
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);

                    int charIndex = grayValue * (asciiChars.Length - 1) / 255;
                    asciiArt.Append(asciiChars[charIndex]);
                }
                asciiArt.AppendLine();
            }
            return asciiArt.ToString();
        }
        private Bitmap ResizeImage(Bitmap image, int width)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            int height = (int)(originalHeight * width / originalWidth);

            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
    }
}
