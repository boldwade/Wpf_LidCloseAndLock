using System;
using System.Windows.Forms;
using Wpf_LidCloseAndLock.Properties;

namespace Wpf_LidCloseAndLock {
    internal class ProcessIcon : IDisposable {
        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        private readonly NotifyIcon _notifyIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon() {
            // Instantiate the NotifyIcon object.
            _notifyIcon = new NotifyIcon();
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public void Display() {
            // Put the icon in the system tray and allow it react to mouse clicks.			
            _notifyIcon.MouseClick += NotifyIconMouseClick;
            _notifyIcon.Icon = Resources.LidCloseAndLock;
            _notifyIcon.Text = @"Lid Close and Lock";
            _notifyIcon.Visible = true;

            // Attach a context menu.
            _notifyIcon.ContextMenuStrip = new ContextMenus().Create();
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose() {
            // When the application closes, this will remove the icon from the system tray immediately.
            _notifyIcon.Dispose();
        }

        /// <summary>
        /// Handles the MouseClick event of the ni control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
        private void NotifyIconMouseClick(object sender, MouseEventArgs e) {
            // Handle mouse button clicks.
            //if (e.Button == MouseButtons.Left) {
            //    Process.Start("explorer", null);
            //}
        }
    }
}
