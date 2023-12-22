using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ColorCraft
{
    public partial class ConicGradientGpu : Gradient
    {
        [DllImport("Resources/DirectXGradients.dll")]
        private static extern int GetConicGradientBitmap(string binPath, int width, int height, float angle, out IntPtr outBitmap, out int stride);

        //[LibraryImport("DirectXGradients.dll")]
        //private static partial int GetConicGradientBitmap(out IntPtr outBitmap, int width, int height, float angle, out int stride);

        [LibraryImport("Resources/DirectXGradients.dll")]
        private static partial void FreeBitmap(ref IntPtr bitmap);

        [LibraryImport("Resources/DirectXGradients.dll")]
        private static partial void CleanupDirectXResources();

        #region Constructors

        static ConicGradientGpu()
        {
            // cleanup DirectX resources when the app exits
            Application.Current.Exit += DirextXCleanup;
        }

        #endregion

        #region Dependency Properties

        // AngleOffset [0, 360]
        public float AngleOffset
        {
            get => (float)GetValue(AngleOffsetProperty);
            set => SetValue(AngleOffsetProperty, value);
        }

        public static readonly DependencyProperty AngleOffsetProperty = DependencyProperty.Register(nameof(AngleOffset), typeof(float), typeof(ConicGradientGpu), new FrameworkPropertyMetadata(0f, OnAngleOffsetChanged));

        private static void OnAngleOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onAngleOffsetChangedCount++;
            if (d is not ConicGradientGpu self) return;
            self.OnAngleOffsetChanged(e);
        }

        protected virtual void OnAngleOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // SpiralStrength
        public float SpiralStrength
        {
            get => (float)GetValue(SpiralStrengthProperty);
            set => SetValue(SpiralStrengthProperty, value);
        }

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register(nameof(SpiralStrength), typeof(float), typeof(ConicGradientGpu), new FrameworkPropertyMetadata(0f, OnSpiralStrengthChanged));

        private static void OnSpiralStrengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onSpiralStrengthChangedCount++;
            if (d is not ConicGradientGpu self) return;
            self.OnSpiralStrengthChanged(e);
        }

        protected virtual void OnSpiralStrengthChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // KaleidoscopeCount
        public float KaleidoscopeCount
        {
            get => (float)GetValue(KaleidoscopeCountProperty);
            set => SetValue(KaleidoscopeCountProperty, value);
        }

        public static readonly DependencyProperty KaleidoscopeCountProperty = DependencyProperty.Register(nameof(KaleidoscopeCount), typeof(float), typeof(ConicGradientGpu), new FrameworkPropertyMetadata(1f, OnKaleidoscopeCountChanged));

        private static void OnKaleidoscopeCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onKaleidoscopeCountChangedCount++;
            if (d is not ConicGradientGpu self) return;
            self.OnKaleidoscopeCountChanged(e);
        }

        protected virtual void OnKaleidoscopeCountChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // CenterX
        public float CenterX
        {
            get => (float)GetValue(CenterXProperty);
            set => SetValue(CenterXProperty, value);
        }

        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register(nameof(CenterX), typeof(float), typeof(ConicGradientGpu), new FrameworkPropertyMetadata(0.5f, OnCenterXChanged), ValidateNormalizedRange);

        private static bool ValidateNormalizedRange(object value)
        {
            if (value is not float f) return false;
            return f >= 0 && f <= 1;
        }

        private static void OnCenterXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onCenterXChangedCount++;
            if (d is not ConicGradientGpu self) return;
            self.OnCenterXChanged(e);
        }

        protected virtual void OnCenterXChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        // CenterY
        public float CenterY
        {
            get => (float)GetValue(CenterYProperty);
            set => SetValue(CenterYProperty, value);
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register(nameof(CenterY), typeof(float), typeof(ConicGradientGpu), new FrameworkPropertyMetadata(0.5f, OnCenterYChanged), ValidateNormalizedRange);

        private static void OnCenterYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            onCenterYChangedCount++;
            if (d is not ConicGradientGpu self) return;
            self.OnCenterYChanged(e);
        }

        protected virtual void OnCenterYChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateGradient();
        }

        #endregion

        #region Methods

        private static string GetCurrentSourcePath([CallerFilePath] string path = "")
        {
            return path;
        }

        public override void DrawGradient()
        {
            if (_bitmap == null) return;



            string binPath = AppDomain.CurrentDomain.BaseDirectory;

            //string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //binPath = Path.GetDirectoryName(exePath) ?? "";

            //binPath = Environment.GetCommandLineArgs()[0];
            //binPath = Path.GetDirectoryName(binPath) ?? "";

            //string[] args = Environment.GetCommandLineArgs();
            //binPath = string.Join(", ", args);
            //Debug.WriteLine("Command Line Args: " + string.Join(", ", args));

            //binPath = GetCurrentSourcePath();





            //TestConcurrentAccess();


            int size = 300;
            int width = size;
            int height = size;

            // Create bitmap from DirectX
            IntPtr outBitmap = IntPtr.Zero;
            try
            {
                int hr = GetConicGradientBitmap(
                    binPath,
                    width,
                    height,
                    AngleOffset * MathF.PI / 180f,
                    out outBitmap,
                    out int stride);

                if (hr < 0)
                    throw new Exception($"GetBitmap failed with hr = 0x{hr:x}");
                if (outBitmap == IntPtr.Zero)
                    throw new Exception("Bitmap is null");

                // Copy the data from unmanaged memory to a managed byte array
                byte[] managedArray = new byte[height * stride];
                Marshal.Copy(outBitmap, managedArray, 0, managedArray.Length);

                // Create a Bitmap from the byte array
                //_bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Pbgra32, null);
                _bitmap.WritePixels(new Int32Rect(0, 0, width, height), managedArray, stride, 0);
            }
            finally
            {
                // Free the unmanaged memory
                if (outBitmap != IntPtr.Zero)
                {
                    FreeBitmap(ref outBitmap);
                }
            }
        }

        private void CallGetConicGradientBitmap(float angleOffset)
        {
            int width = 300;  // Example dimensions
            int height = 300;
            IntPtr outBitmap = IntPtr.Zero;
            var bitmap = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32, null);

            try
            {
                int hr = GetConicGradientBitmap(
                    "foo",  // Example string
                    width,
                    height,
                    angleOffset * MathF.PI / 180f,
                    out outBitmap,
                    out int stride
                );

                if (hr < 0)
                    throw new Exception($"GetBitmap failed with hr = 0x{hr:x}");
                if (outBitmap == IntPtr.Zero)
                    throw new Exception("Bitmap is null");

                byte[] managedArray = new byte[height * stride];
                Marshal.Copy(outBitmap, managedArray, 0, managedArray.Length);

                bitmap.WritePixels(new Int32Rect(0, 0, width, height), managedArray, stride, 0);

            }
            finally
            {
                if (outBitmap != IntPtr.Zero)
                {
                    FreeBitmap(ref outBitmap);
                }
            }
        }


        public void TestConcurrentAccess()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)  // Adjust the number of tasks as needed
            {
                float angleOffset = i * 10;  // Vary the angle for each task
                tasks.Add(Task.Run(() => CallGetConicGradientBitmap(angleOffset)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static void DirextXCleanup(object sender, ExitEventArgs e)
        {
            CleanupDirectXResources();
        }

        #endregion

        private static int onAngleOffsetChangedCount = 0;
        private static int onSpiralStrengthChangedCount = 0;
        private static int onKaleidoscopeCountChangedCount = 0;
        private static int onCenterXChangedCount = 0;
        private static int onCenterYChangedCount = 0;

        public static void DumpConicEventCounts()
        {
            Debug.WriteLine($"onAngleOffsetChangedCount: {onAngleOffsetChangedCount}");
            Debug.WriteLine($"onSpiralStrengthChangedCount: {onSpiralStrengthChangedCount}");
            Debug.WriteLine($"onKaleidoscopeCountChangedCount: {onKaleidoscopeCountChangedCount}");
            Debug.WriteLine($"onCenterXChangedCount: {onCenterXChangedCount}");
            Debug.WriteLine($"onCenterYChangedCount: {onCenterYChangedCount}");
        }

    }
}
