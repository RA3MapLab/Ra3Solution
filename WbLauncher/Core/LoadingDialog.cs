using System;
using System.Drawing;
using System.Windows.Forms;

namespace WbLauncher.Core
{
    public class LoadingDialog : Form
    {
        public bool Loaded = false;
        public LoadingDialog(string msg)
        {
            var label = new Label
            {
                Text = msg,
                Dock = DockStyle.Fill
            };
            label.Font = new Font(label.Font.FontFamily, 13);
            Controls.Add(label);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            Text = "提示";
            Size = new Size(100, 200);
            StartPosition = FormStartPosition.CenterScreen;

            FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (MessageBox.Show("确定取消启动？", "地编启动器", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Environment.Exit(1);
                    }
                    e.Cancel = true;
                }
            };
        }

    }
}