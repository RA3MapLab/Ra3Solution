﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WbLauncher.Core
{
    public class SelectModDialog: Form
    {
        private ListBox listBox;
        private Button okButton;
        private Button cancelButton;

        public List<ModInfo> Items;
        private readonly Action<string> onSelect;

        public SelectModDialog(Action<string> onSelect)
        {
            // Items = new List<string>();
            this.onSelect = onSelect;
            Items = getMods();
            listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            Items.ForEach(item => listBox.Items.Add(item.label));
            listBox.Font = new Font(listBox.Font.FontFamily, 13);
            listBox.SelectionMode = SelectionMode.One;
            listBox.DoubleClick += onSelectMod;
            listBox.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                {
                    onSelectMod(sender, args);
                }
            };
            listBox.SelectedIndex = 0;
            Controls.Add(listBox);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            Text = "选择要加载的mod";
            Size = new Size(300, 400);
            StartPosition = FormStartPosition.CenterScreen;
            // StartPosition = FormStartPosition.CenterParent;
        }

        private void onSelectMod(object sender, EventArgs e)
        {
            onSelect(Items[listBox.SelectedIndex].skudefPath);
            DialogResult = DialogResult.OK;
            Close();
        }


        private List<ModInfo> getMods()
        {
            var mods =  Directory.EnumerateFiles(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Red Alert 3",
                    "Mods"
                ), "*.skudef", SearchOption.AllDirectories)
                .Select(path =>
                {
                    return new ModInfo()
                    {
                        skudefPath = path,
                        label = Path.GetFileNameWithoutExtension(path)
                    };
                }).ToList();
            mods.Insert(0, new ModInfo()
            {
                skudefPath = "",
                label = "原版(不加载任何mod)"
            });
            return mods;
        }
        
        public class ModInfo
        {
            public string skudefPath;
            public string label;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(-1, 79);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(296, 364);
            this.listBox1.TabIndex = 0;
            // 
            // SelectModDialog
            // 
            this.ClientSize = new System.Drawing.Size(294, 443);
            this.Controls.Add(this.listBox1);
            this.Name = "SelectModDialog";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox listBox1;
    }
}