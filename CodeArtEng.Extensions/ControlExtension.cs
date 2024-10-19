using System.Reflection;
using System.Windows.Forms;

namespace CodeArtEng.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Control"/> class.
    /// </summary>
    public static class ControlExtension
    {

        /// <summary>
        /// Enables or disables double buffering for the specified control.
        /// </summary>
        /// <param name="control">The control on which to set double buffering.</param>
        /// <param name="enable">Indicates whether to enable or disable double buffering.</param>
        /// <param name="recursive">Apply to all childs controls.</param>
        /// <remarks>
        /// This method uses reflection to access the non-public <c>DoubleBuffered</c> property of the <see cref="Control"/> class.
        /// </remarks>
        public static void SetDoubleBuffering(this Control control, bool enable, bool recursive = true)
        {
            PropertyInfo property = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            property.SetValue(control, enable, null);

            if (recursive)
            {
                // Recursively apply to child controls
                foreach (Control child in control.Controls)
                {
                    SetDoubleBuffering(child, enable);
                }
            }
        }
    }
}
