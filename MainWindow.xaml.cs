using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SVPP_CS_WPF_Lab1_task1_individual__Thimbles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int numberAttempts = 1;
        private int gamesPlayed = 0;
        private Dictionary<string, int> speedVariants = new() 
        { {"1x", 3000 }, { "2x", 2000 }, { "3x", 1000 } };
        private int speed = 3000;

        public MainWindow()
        {
            InitializeComponent();
            
            Binding b = new Binding();
            b.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, this.GetType(), 1);
            b.Path = new PropertyPath(nameof(numberAttempts));
            TextBlock_Attempts.SetBinding(TextBlock.TextProperty, b);


        }

        private void Button_StartEnd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Stir_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Обработчик выбора количества попыток.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_NumberStirs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            ComboBoxItem item = ( ComboBoxItem)cb.SelectedItem;
            if(item.Content is not null)
            {
                string val = (string) item.Content;
                Int32.TryParse(val, out numberAttempts);
                
            }        
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = (RadioButton)sender;
            var speedKey = (string) rb.Content;
            if(speedKey != null)
            {
                speedVariants.TryGetValue(speedKey, out speed);
                
            }

        }
    }
}