using System.Windows;

namespace Anori.WPF.Blinkers.TestGui.ViewModelBlinking
{
    /// <summary>
    ///     Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var viewModel = new BlinkingCollectionViewModel5();
            for (var i = 0; i < 5000; i++)
            {
                viewModel.Items.Add(new BlinkingViewModel5());
            }

            DataContext = viewModel;
        }
    }
}