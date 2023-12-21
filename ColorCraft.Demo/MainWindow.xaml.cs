using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorCraft.Demo
{
    public partial class MainWindow : Window
    {
        BitmapSource? bitmapSource;
        string DirectXDll = "C:\\Users\\rando\\source\\repos\\WPF\\XamlSpinners\\x64\\Debug\\DirectXGradients.dll";

        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bitmapSource = GetBitmapFromCpp();
            //Utils.DebugBitmap(bitmapSource);
            DemoImage.Source = bitmapSource;
            DemoImage.Width = bitmapSource.Width;
            DemoImage.Height = bitmapSource.Height;
            App.Current.Exit += DirextXCleanup;
        }

        

        [LibraryImport("C:\\Users\\rando\\source\\repos\\WPF\\XamlSpinners\\x64\\Debug\\DirectXGradients.dll")]
        private static partial int GetConicGradientBitmap(out IntPtr outBitmap, int width, int height, float angle, out int stride);

        [LibraryImport("C:\\Users\\rando\\source\\repos\\WPF\\XamlSpinners\\x64\\Debug\\DirectXGradients.dll")]
        private static partial void FreeBitmap(ref IntPtr bitmap);

        [LibraryImport("C:\\Users\\rando\\source\\repos\\WPF\\XamlSpinners\\x64\\Debug\\DirectXGradients.dll")]
        private static partial void CleanupDirectXResources();

        public static BitmapSource GetBitmapFromCpp()
        {
            int size = 300;
            float angle = 0.0f;

            int width = size;
            int height = size;

            IntPtr outBitmap = IntPtr.Zero;
            WriteableBitmap bitmap;
            try
            {
                int hr = GetConicGradientBitmap(out outBitmap, width, height, angle * MathF.PI / 180f, out int stride);

                if (hr < 0)
                    throw new Exception($"GetBitmap failed with hr = 0x{hr:x}");
                if (outBitmap == IntPtr.Zero)
                    throw new Exception("Bitmap is null");

                // Copy the data from unmanaged memory to a managed byte array
                byte[] managedArray = new byte[height * stride];
                Marshal.Copy(outBitmap, managedArray, 0, managedArray.Length);

                // Create a Bitmap from the byte array
                bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
                bitmap.WritePixels(new Int32Rect(0, 0, width, height), managedArray, stride, 0);
            }
            finally
            {
                // Free the unmanaged memory
                if (outBitmap != IntPtr.Zero)
                {
                    FreeBitmap(ref outBitmap);
                }
            }

            return bitmap;
        }

        private void DirextXCleanup(object sender, ExitEventArgs e)
        {
            CleanupDirectXResources();
        }
    }
}
