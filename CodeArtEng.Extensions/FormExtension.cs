using System.Text;
using System.Drawing;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    /// <summary>
    /// <see cref="Form"/> class extension.
    /// </summary>
    public static class FormExtension
    {
        /// <summary>
        /// Set from Icon with application Icon.
        /// </summary>
        /// <param name="form"></param>
        public static void SetAppIcon(this Form form )
        {
            //form.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath); //This code does not support network path.
            form.Icon = ExtractAssociatedIcon(Application.ExecutablePath);
        }

        public static bool IsOutsideViewRegion(this Form form)
        {
            foreach(Screen ptrScren in Screen.AllScreens)
            {
                if (ptrScren.Bounds.Contains(form.Location)) return false;
            }
            return true;
        }

        #region [ Improved version of ExtractAssociatedIcon which works with network path ]
        //Source: https://stackoverflow.com/questions/1842226/how-to-get-the-associated-icon-from-a-network-share-file

        /// <summary>
        /// Returns an icon representation of an image contained in the specified file.
        /// This function is identical to System.Drawing.Icon.ExtractAssociatedIcon, xcept this version works.
        /// </summary>
        /// <param name="filePath">The path to the file that contains an image.</param>
        /// <returns>The System.Drawing.Icon representation of the image contained in the specified file.</returns>
        /// <exception cref="System.ArgumentException">filePath does not indicate a valid file.</exception>
        private static Icon ExtractAssociatedIcon(String filePath)
        {
            int index = 0;

            Uri uri;
            if (filePath == null)
            {
                throw new ArgumentException(String.Format("'{0}' is not valid for '{1}'", "null", "filePath"), "filePath");
            }
            try
            {
                uri = new Uri(filePath);
            }
            catch (UriFormatException)
            {
                filePath = Path.GetFullPath(filePath);
                uri = new Uri(filePath);
            }
            //if (uri.IsUnc)
            //{
            //  throw new ArgumentException(String.Format("'{0}' is not valid for '{1}'", filePath, "filePath"), "filePath");
            //}
            if (uri.IsFile)
            {
                if (!File.Exists(filePath))
                {
                    //IntSecurity.DemandReadFileIO(filePath);
                    throw new FileNotFoundException(filePath);
                }

                StringBuilder iconPath = new StringBuilder(260);
                iconPath.Append(filePath);

                IntPtr handle = SafeNativeMethods.ExtractAssociatedIcon(new HandleRef(null, IntPtr.Zero), iconPath, ref index);
                if (handle != IntPtr.Zero)
                {
                    //IntSecurity.ObjectFromWin32Handle.Demand();
                    return Icon.FromHandle(handle);
                }
            }
            return null;
        }


        /// <summary>
        /// This class suppresses stack walks for unmanaged code permission. 
        /// (System.Security.SuppressUnmanagedCodeSecurityAttribute is applied to this class.) 
        /// This class is for methods that are safe for anyone to call. 
        /// Callers of these methods are not required to perform a full security review to make sure that the 
        /// usage is secure because the methods are harmless for any caller.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon", CharSet = CharSet.Auto)]
            internal static extern IntPtr ExtractAssociatedIcon(HandleRef hInst, StringBuilder iconPath, ref int index);
        }

        #endregion  
    }
}
