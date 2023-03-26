using System;
using System.ComponentModel;
using System.Windows;
using WbHelper.UI;
using wbInject;
using wbInject.Core;

namespace WbHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WbStarter.ICommandListener
    {
        private readonly WbStarter starter;

        public MainWindow()
        {
            InitializeComponent();
            starter = new WbStarter();
            starter.commandListener = this;
            
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            starter.closeInjector();
            Logger.close();
        }

        private void LoadPlugin(object sender, RoutedEventArgs e)
        {
            try
            { 
                starter.initWbInjector();
            }
            catch (Exception exception)
            {
                Logger.log("LoadPlugin", exception.Message + "  " + exception.StackTrace);
                MessageBox.Show(exception.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public string onSelectScript()
        {
            return (string)Application.Current.Dispatcher.Invoke(new Func<string>(() =>
            {
                var chooseScriptDialog = new ChooseScriptDialog();
                if (chooseScriptDialog.ShowDialog() == true)
                {
                    return chooseScriptDialog.selectScriptName;
                }

                return "";
            }));
            
            
        }

        public void onClose()
        {
            Close();
        }

        public void showErrorBox(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnHelp(object sender, RoutedEventArgs e)
        {
            
        }
    }
}