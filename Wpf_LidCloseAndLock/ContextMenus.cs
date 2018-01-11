using System;
using System.Diagnostics;
using System.Windows.Forms;
using Wpf_LidCloseAndLock.Properties;

namespace Wpf_LidCloseAndLock {
    internal class ContextMenus {
        /// <summary>
        /// Is the About box displayed?
        /// </summary>
        private bool _isAboutLoaded;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create() {
            // Add the default menu options.
            var menu = new ContextMenuStrip();

            // Windows Explorer.
            var item = new ToolStripMenuItem { Text = @"Explorer" };
            item.Click += Explorer_Click;
            item.Image = Resources.Explorer;
            menu.Items.Add(item);

            // About.
            item = new ToolStripMenuItem { Text = @"About" };
            item.Click += About_Click;
            item.Image = Resources.About;
            menu.Items.Add(item);

            // Separator.
            var sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem { Text = @"Exit" };
            item.Click += Exit_Click;
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        /// <summary>
        /// Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Explorer_Click(object sender, EventArgs e) {
            Process.Start("explorer", null);
        }

        /// <summary>
        /// Handles the Click event of the About control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void About_Click(object sender, EventArgs e) {
            if (_isAboutLoaded) return;

            _isAboutLoaded = true;
            new AboutBox().ShowDialog();
            _isAboutLoaded = false;
        }

        /// <summary>
        /// Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Exit_Click(object sender, EventArgs e) {
            // Quit without further ado.
            System.Windows.Application.Current.Shutdown();
            //Application.Exit();
        }
    }
}
