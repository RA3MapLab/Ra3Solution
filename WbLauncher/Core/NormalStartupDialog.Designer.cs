using System.ComponentModel;

namespace WbLauncher.Core
{
    partial class NormalStartupDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NormalStartupDialog));
            this.modList = new System.Windows.Forms.ListBox();
            this.languageSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectModTv = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.qqgroup = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // modList
            // 
            this.modList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.modList.FormattingEnabled = true;
            this.modList.ItemHeight = 16;
            this.modList.Location = new System.Drawing.Point(5, 68);
            this.modList.Name = "modList";
            this.modList.Size = new System.Drawing.Size(362, 404);
            this.modList.TabIndex = 0;
            // 
            // languageSelect
            // 
            this.languageSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageSelect.FormattingEnabled = true;
            this.languageSelect.Location = new System.Drawing.Point(81, 13);
            this.languageSelect.Name = "languageSelect";
            this.languageSelect.Size = new System.Drawing.Size(173, 21);
            this.languageSelect.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "选择语言";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // selectModTv
            // 
            this.selectModTv.Location = new System.Drawing.Point(5, 42);
            this.selectModTv.Name = "selectModTv";
            this.selectModTv.Size = new System.Drawing.Size(112, 23);
            this.selectModTv.TabIndex = 3;
            this.selectModTv.Text = "选择mod:";
            this.selectModTv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Location = new System.Drawing.Point(106, 482);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Launch_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 533);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "作者：物w  地编问题请加群";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // qqgroup
            // 
            this.qqgroup.Location = new System.Drawing.Point(184, 533);
            this.qqgroup.Name = "qqgroup";
            this.qqgroup.Size = new System.Drawing.Size(93, 16);
            this.qqgroup.TabIndex = 6;
            this.qqgroup.TabStop = true;
            this.qqgroup.Text = "613550502";
            // 
            // NormalStartupDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 558);
            this.Controls.Add(this.qqgroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.selectModTv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.languageSelect);
            this.Controls.Add(this.modList);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NormalStartupDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新地编启动器";
            this.Load += new System.EventHandler(this.NormalStartupDialog_Load);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.LinkLabel qqgroup;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label selectModTv;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.ComboBox languageSelect;

        private System.Windows.Forms.ListBox modList;

        #endregion
    }
}