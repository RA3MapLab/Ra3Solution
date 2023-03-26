using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using MapCoreLib.Core.Scripts;
using MapCoreLib.Core.Util;

namespace WbHelper.UI
{
    public partial class ChooseScriptDialog : Window
    {
        public string selectScriptName = "";
        private Dictionary<string, string> scriptDescs = ScriptHandler.getScriptDescs().ToDictionary(item => item.label, item => item.helpText);

        public ChooseScriptDialog()
        {
            InitializeComponent();

            scriptList.ItemsSource = scriptDescs.Keys;
        }


        private void onConfirm(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            selectScriptName = scriptList.SelectedItem as string;
            Close();
        }

        private void OnItemSelected(object sender, RoutedEventArgs e)
        {
            HelpBox.Text = scriptDescs[scriptList.SelectedItem as string];
        }
    }
}