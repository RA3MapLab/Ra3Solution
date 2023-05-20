using System.Windows;

namespace NetUI.UI
{
    public partial class SelectCodeScriptWindow : Window
    {
        public SelectCodeScriptWindow()
        {
            InitializeComponent();
        }

        public static void Show()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                new SelectCodeScriptWindow().ShowDialog();
            });
        }
    }
}