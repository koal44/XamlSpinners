using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ColorCraft.Demo
{
    public partial class MainWindow : Window
    {
        //readonly WriteableBitmap bitmap = new(300, 300, 96, 96, System.Windows.Media.PixelFormats.Pbgra32, null);

        //[DllImport("DirectXGradients.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        //private static extern int GetConicGradientBitmap(string binPath, int width, int height, float angle, out IntPtr outBitmap, out int stride);

        ////[DllImport("DirectXGradients.dll")]
        ////private static extern int GetConicGradientBitmap(string binPath, int width, int height, float angle, out IntPtr outBitmap, out int stride);

        //[DllImport("DirectXGradients.dll")]
        //private static extern void FreeBitmap(ref IntPtr bitmap);

        //[DllImport("DirectXGradients.dll")]
        //private static extern void CleanupDirectXResources();

        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
            //CallGetConicGradientBitmap();
            //MainImage.Source = bitmap;
            //TestConcurrentAccess();
        }

        //private static string GetCurrentSourcePath([CallerFilePath] string path = "")
        //{
        //    return path;
        //}

        //private void CallGetConicGradientBitmap()
        //{
        //    int width = 300;  // Example dimensions
        //    int height = 300;
        //    IntPtr outBitmap = IntPtr.Zero;
        //    //bitmap = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32, null);

        //    string binPath = AppDomain.CurrentDomain.BaseDirectory;

        //    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        //    binPath = Path.GetDirectoryName(exePath) ?? "";

        //    binPath = Environment.GetCommandLineArgs()[0];
        //    binPath = Path.GetDirectoryName(binPath) ?? "";

        //    string[] args = Environment.GetCommandLineArgs();
        //    binPath = string.Join(", ", args);
        //    Debug.WriteLine("Command Line Args: " + string.Join(", ", args));

        //    binPath = GetCurrentSourcePath();

        //    try
        //    {
        //        int hr = GetConicGradientBitmap("hello", 300, 300, 0, out outBitmap, out int stride);

        //        if (hr < 0)
        //            throw new Exception($"GetBitmap failed with hr = 0x{hr:x}");
        //        if (outBitmap == IntPtr.Zero)
        //            throw new Exception("Bitmap is null");

        //        byte[] managedArray = new byte[height * stride];
        //        Marshal.Copy(outBitmap, managedArray, 0, managedArray.Length);

        //        bitmap.WritePixels(new Int32Rect(0, 0, width, height), managedArray, stride, 0);

        //    }
        //    finally
        //    {
        //        if (outBitmap != IntPtr.Zero)
        //        {
        //            //FreeBitmap(ref outBitmap);
        //        }
        //    }
        //}


        //public void TestConcurrentAccess()
        //{
        //    var tasks = new List<Task>();
        //    for (int i = 0; i < 10; i++)  // Adjust the number of tasks as needed
        //    {
        //        float angleOffset = i * 10;  // Vary the angle for each task
        //        tasks.Add(Task.Run(() => CallGetConicGradientBitmap()));
        //    }

        //    Task.WaitAll(tasks.ToArray());
        //}

    }
}
