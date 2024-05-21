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

        public UserGameData GameData = new UserGameData();

        private Dictionary<string, int> speedVariants = new() 
        { {"1x", 250 }, { "2x", 200 }, { "3x", 100 } };
        private int speed = 3000;

        public MainWindow()
        {
            InitializeComponent();
            Grid_GameText.DataContext = GameData;


        }
        /// <summary>
        /// Обработчик нажатия копки Начать/Стоп.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_StartEnd_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button) sender;

            // Если нажато Начать
            if(bt.Content as string == "Начать")
            {
                bt.Content = "Стоп";               
                Button_Stir.IsEnabled = true;
                selectingGameOptionsEnabled(false);
                GameData.Status = UserGameData.Statuses["Stir"];

            }
            // Если нажато Стоп.
            else if (bt.Content as string == "Стоп") {
                bt.Content = "Начать";
                Button_Stir.IsEnabled = false;
                selectingGameOptionsEnabled(true);
                GameData.Reset();
            }
                       
        }

        private async void Button_Stir_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button ) sender;
            bt.IsEnabled = false;
            await Thimbles.Stir(1, speed);
            bt.IsEnabled= true;
            
        }
        /// <summary>
        /// Обработчик выбора количества попыток.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_NumberStirs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tempNumberAttempts = -1;
            ComboBox cb = (ComboBox)sender;
            ComboBoxItem item = ( ComboBoxItem)cb.SelectedItem;
            if(item.Content is not null)
            {
                string val = (string) item.Content;
                Int32.TryParse(val, out tempNumberAttempts);
                if(tempNumberAttempts != -1) GameData.NumberAttempts = tempNumberAttempts;
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
        /// <summary>
        /// Включает и выключает опции выбора в игре.
        /// </summary>
        /// <param name="enabled"></param>
        private void selectingGameOptionsEnabled(bool enabled)
        {
            // Включение и отключение выбора скорости игры (RadioButtons)
            foreach (var item in StackPanel_RadioButtonsSpeed.Children)
            {
                if(item is RadioButton bt) bt.IsEnabled = enabled;
            }
            // Включение и отключение выбора количества попыток (Combobox)
            ComboBox_NumberStirs.IsEnabled = enabled;
        }
    }
}