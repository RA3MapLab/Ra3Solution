using System.Drawing;
using System.Windows.Forms;

namespace WbLauncher.Core
{
    public class LoadingDialog: Form
    {
        public LoadingDialog(string msg)
        {
            var label = new Label();
            label.Text = msg;
            label.Dock = DockStyle.Fill;
            label.Font = new Font(label.Font.FontFamily, 13);
            Controls.Add(label);

            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Text = "提示";
            Size = new Size(100, 200);
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}