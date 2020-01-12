using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    /// <summary>
    /// Application Extension Class
    /// </summary>
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Register application to HKCU autorun registry with additional arguments
        /// </summary>
        /// <param name="arguments"></param>
        public static void AddApplicationToStartup(string arguments)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(Application.ProductName, "\"" + Application.ExecutablePath + "\" " + arguments);
            }
        }

        /// <summary>
        /// Register application to HKCU autorun registry.
        /// </summary>
        public static void AddApplicationToStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(Application.ProductName, "\"" + Application.ExecutablePath + "\"");
            }
        }


        /// <summary>
        /// Unregister application from HKCU autorun registry
        /// </summary>
        public static void RemoveApplicationFromStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue(Application.ProductName, false);
            }
        }

        /// <summary>
        /// Check if application is registered in HKCU autorun registry
        /// </summary>
        /// <returns></returns>
        public static bool StartApplicationOnStartupEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
            {
                return key.GetValue(Application.ProductName) != null;
            }
        }

        /// <summary>
        /// Verify and update path if application is registered in HKCU autorun registery.
        /// This method is useful to configure auto startup for ClickOnce application where path changed on each update.
        /// NOTE: This method does not support argument
        /// </summary>
        public static void CheckAndUpdateApplicationStartupPath()
        {
            if(StartApplicationOnStartupEnabled())
            {
                AddApplicationToStartup();
            }
        }

    }
}
