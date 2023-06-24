using System.ComponentModel;

namespace Downloader.Core.UI;

partial class ProgressDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDialog));
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.speedTv = new System.Windows.Forms.Label();
        this.progressTv = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(12, 45);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(500, 51);
        this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        this.progressBar.TabIndex = 0;
        // 
        // speedTv
        // 
        this.speedTv.Location = new System.Drawing.Point(364, 19);
        this.speedTv.Name = "speedTv";
        this.speedTv.Size = new System.Drawing.Size(148, 23);
        this.speedTv.TabIndex = 1;
        this.speedTv.Text = "speed";
        // 
        // progressTv
        // 
        this.progressTv.Location = new System.Drawing.Point(12, 19);
        this.progressTv.Name = "progressTv";
        this.progressTv.Size = new System.Drawing.Size(190, 23);
        this.progressTv.TabIndex = 2;
        this.progressTv.Text = "progress";
        // 
        // ProgressDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(524, 109);
        this.Controls.Add(this.progressTv);
        this.Controls.Add(this.speedTv);
        this.Controls.Add(this.progressBar);
        this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Margin = new System.Windows.Forms.Padding(4);
        this.MaximizeBox = false;
        this.Name = "ProgressDialog";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Progress";
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Label progressTv;
    private System.Windows.Forms.Label 速度;

    private System.Windows.Forms.ProgressBar progressBar;
    private System.Windows.Forms.Label speedTv;

    #endregion
}