using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;

namespace WX
{
    public partial class WXForm : Form
    {
        public WXForm()
        {
            InitializeComponent();
            try
            {
                trayIcon = Properties.Resources.tray;
                blankIcon = Properties.Resources.blank;
            }
            catch { }
        }

        string lastContactList = null, lastMsgList = null;
        bool hasNewMsg = false, isUnfocused = false, hasNotification = false, icoFlag = false;
        Icon trayIcon, blankIcon;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (hasNotification && trayIcon != null && blankIcon != null)
            {
                this.notifyIcon.Icon = (icoFlag = !icoFlag) ? trayIcon : blankIcon;
            }
            hasNewMsg = false;
            HtmlElement contactList = webBrowser.Document.GetElementById("chat_conversationListContent");
            HtmlElement msgList = webBrowser.Document.GetElementById("chat_chatmsglist");

            if (contactList != null && !String.IsNullOrEmpty(contactList.InnerHtml)
                && !String.Equals(lastContactList, contactList.InnerHtml))
            {
                if (lastContactList != null) hasNewMsg = true;
                lastContactList = contactList.InnerHtml;
            }

            if (msgList != null && !String.IsNullOrEmpty(msgList.InnerHtml)
                && !String.Equals(lastMsgList, msgList.InnerHtml))
            {
                if (lastMsgList != null) hasNewMsg = true;
                lastMsgList = msgList.InnerHtml;
            }

            if (!hasNotification) hasNotification = hasNewMsg & isUnfocused;
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.timer.Start();
            this.Text = "微信PC版";
        }

        private void WXForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }


        private void WXForm_Activated(object sender, EventArgs e)
        {
            this.isUnfocused = false;
            hasNotification = false;
            if(trayIcon != null) this.notifyIcon.Icon = trayIcon;
        }

        private void WXForm_Deactivate(object sender, EventArgs e)
        {
            this.isUnfocused = true;
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showWX();
        }

        private void showWX()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutBox()).ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.showWX();
            }
        }

    }
}
