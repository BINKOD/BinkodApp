using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BinkodApp.Core;
using System.IO;
using System.Net;

namespace BinkodApp.Web.Helper
{
    public class CaptureScreenshot
    {
        private static string ScreenshotPath = HttpContext.Current.Server.MapPath("~/Images/Screenshots/");
        private static string ScreenshotDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static string BinkodShotFolderPath = Path.Combine(ScreenshotDesktopPath, "BinkodShot boredsilly.in");
        private static string ScreenshotName = "binkodShot_" + Utils.DateFormatForFilename();

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private static class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private static class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        private static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        public static void CaptureWindowToFile(IntPtr handle, string filename = "", ImageFormat format = null)
        {
            if (string.IsNullOrEmpty(filename))
                filename = ScreenshotName;
            if (format == null)
                format = ImageFormat.Jpeg;

            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        public static void CaptureScreenToFile(string filename = "", ImageFormat format = null)
        {
            if (string.IsNullOrEmpty(filename))
                filename = ScreenshotName;
            if (format == null)
                format = ImageFormat.Jpeg;

            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Takes a fullscreen screenshot of the monitor and saves the specified file in a directory with custom name.
        /// It expects the Format of the file.
        /// Call //FullScreenshot(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "screenshot.jpg", ImageFormat.Jpeg);
        /// </summary>
        public static void FullScreenshot(String filepath = "", String filename = "", ImageFormat format = null)
        {
            if (string.IsNullOrEmpty(filepath))
                filepath = ScreenshotPath;
            if (string.IsNullOrEmpty(filename))
                filename = ScreenshotName;
            if (format == null)
                format = ImageFormat.Jpeg;

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics _graphics = Graphics.FromImage(bitmap))
                {
                    try
                    {
                        _graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);                      
                    }
                    catch (Exception ex)
                    {
                        Common.ExceptionLog(ex.Message + " - " + ex.StackTrace);
                        try
                        {
                            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                            string fullpath = filepath + filename + "." + format.ToString();
                            bmpScreenshot.Save(fullpath, format);
                        }
                        catch (Exception exp)
                        {
                            Common.ExceptionLog(exp.Message + " - " + exp.StackTrace);                            
                            throw exp;
                        }
                    }
                }

                try
                {
                    string fullpath = filepath + filename + "." + format.ToString();
                    bitmap.Save(fullpath, format);
                }
                catch
                {
                    //to save img on desktop
                    if (!System.IO.Directory.Exists(BinkodShotFolderPath))
                        System.IO.Directory.CreateDirectory(BinkodShotFolderPath);
                    //BinkodShotFolderPath = ScreenshotDesktopPath;
                    string DesktopPath = BinkodShotFolderPath + "\\" + filename + "." + format.ToString();
                    bitmap.Save(DesktopPath, format);
                }
            }
        }

        /// <summary>
        ///  Creates a screenshot using the ScreenCapture class.
        ///  call /// FullScreenshotWithClass(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "screenshotfull_class.jpg", ImageFormat.Jpeg);
        /// </summary>
        public static void FullScreenshotWithClass(String filepath = "", String filename = "", ImageFormat format = null)
        {
            if (string.IsNullOrEmpty(filepath))
                filepath = ScreenshotPath;
            if (string.IsNullOrEmpty(filename))
                filename = ScreenshotName;
            if (format == null)
                format = ImageFormat.Jpeg;

            Image img = CaptureScreen();

            string fullpath = filepath +  filename;
            img.Save(fullpath, format);
            //to save img on desktop
            //string DesktopPath = ScreenshotDesktopPath + filename;
            //img.Save(DesktopPath, format);
        }

    }

}