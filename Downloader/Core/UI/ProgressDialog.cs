using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Downloader.Core.UI;

public partial class ProgressDialog : Form
{
    // private CancellationTokenSource cancellationToken;
    private Progress<int?> downloadProgress;

    private Progress<string?> speedProgress;
    private Action<Progress<int?>?,Progress<string?>?> task;

    public ProgressDialog(string title, Action<Progress<int?>?, Progress<string?>?> task)
    {
        InitializeComponent();
        FormBorderStyle = FormBorderStyle.FixedSingle;
        this.Text = title;
        progressBar.Value = 0;
        speedTv.Text = "";
        this.task = task;
        downloadProgress = new(i =>
        {
            if (i != null)
            {
                this.Invoke(new Action(() =>
                {
                    this.SetProgress(i.Value);
                }));
            }
        });
        speedProgress = new(s =>
        {
            this.Invoke(new Action(() =>
            {
                this.setSpeed(s ?? "");
                if (s == "Done")
                {
                    MessageBox.Show("Download Complete!");
                    this.Close();
                }
            }));
        });
        
    }

    protected override void OnShown(EventArgs e)
    {
        task.Invoke(downloadProgress, speedProgress);
        base.OnShown(e);
    }

    public void SetProgress(int progress)
    {
        progressTv.Text = $"Progress: {progress}%";
        progressBar.Value = progress;
    }
    
    public void setSpeed(string speed)
    {
        speedTv.Text = $"{speed}";
    }

    
}