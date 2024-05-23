using System.Reflection;
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
using Thimbles_Game;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SVPP_CS_WPF_Lab1_task1_individual__Thimbles
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Содержит игровые счетчики.
        public UserGameData GameData = new UserGameData();

        // Скорость игры.
        private Dictionary<string, int> speedVariants = new()
        { {"1x", 250 }, { "2x", 200 }, { "3x", 100 } };
        private int speed = 3000;

        public MainWindow()
        {
            InitializeComponent();
            Grid_GameText.DataContext = GameData;
            Thimbles.CheckBall += glassSelect_Handler;


        }

        /// <summary>
        /// Обработчик нажатия копки Начать/Стоп.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_StartEnd_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;

            // Если нажато Начать
            if (bt.Content as string == "Начать")
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
                GameData.Reset(); // Сброс игровых данных

                // Устанавливает счетчик оставшихся поптыток на количество
                // выбранное в ComboBox выбора попыток
                ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_NumberStirs.SelectedItem;
                GameData.NumberAttempts = int.Parse((string)selectedItem.Content);
            }

        }

        /// <summary>
        /// Обработчик конпки Перемешать.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Stir_Click(object sender, RoutedEventArgs e)
        {
            // Отключение кнопки "Перемешать", кнопки "Начать/Стоп", окна выбора стаканов
            Button bt = (Button)sender;
            bt.IsEnabled = false;
            Button_StartEnd.IsEnabled = false;
            Thimbles.IsEnabled = false;

            // Перемешивание
            GameData.Status = UserGameData.Statuses["Stiring"];
            await Thimbles.Stir(1, speed);

            // Включение опций
            bt.IsEnabled = true;
            Button_StartEnd.IsEnabled = true;
            Thimbles.IsEnabled = true;

            GameData.Status = UserGameData.Statuses["Select"];
            
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
            ComboBoxItem item = (ComboBoxItem)cb.SelectedItem;
            if (item.Content is not null)
            {
                string val = (string)item.Content;
                Int32.TryParse(val, out tempNumberAttempts);
                if (tempNumberAttempts != -1) GameData.NumberAttempts = tempNumberAttempts;
            }
        }

        /// <summary>
        /// Обработчик выбора скорости.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = (RadioButton)sender;
            var speedKey = (string)rb.Content;
            if (speedKey != null)
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
                if (item is RadioButton bt) bt.IsEnabled = enabled;
            }
            // Включение и отключение выбора количества попыток (Combobox)
            ComboBox_NumberStirs.IsEnabled = enabled;
        }

        /// <summary>
        /// Проверяет количество попыток.Если равно 0 -заверашет игру.
        /// </summary>
        private void сheckNumberAttempts()
        {
            // Если количество попыток исчерпано
            if (GameData.NumberAttempts == 0) 
            {
               
                ComboBoxItem selectedItem = (ComboBoxItem)ComboBox_NumberStirs.SelectedItem;
                int numberAttemps = int.Parse((string)selectedItem.Content); // Общее к-во попыток
                int guessed = GameData.Guessed; // Угадано
                int notGuessed = numberAttemps - GameData.Guessed; //не угадано

                string header = "Игра окончена";
                string message = string.Empty;

                if (guessed >= notGuessed) message = "Вы выйграли!\n";
                else message = "Проигрыш :(\n";

                message += $"\nКоличество попыток: {numberAttemps}\n" +
                    $"Угадано: {guessed}\nМимо: {notGuessed}";

                // Нажатие кнопки Стоп
                Button_StartEnd_Click(Button_StartEnd, new RoutedEventArgs());
                MessageBox.Show(message, header);


            }
        }

        /// <summary>
        /// Обработчик события Thimbles.CheckBall ,
        /// которое происходить при поднятии стакана в котором есть мяч.
        /// Обработичк увеличивает счетчик угадываний.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void glassSelect_Handler(object sender, CheckBallEventArgs e)
        {
            if (e.HasBall == true)
            {
                GameData.Guessed++;
                GameData.Status = UserGameData.Statuses["Success"];
            }
            else GameData.Status = UserGameData.Statuses["Fail"];

            // Проверка и уменьшение количества оставшихся попыток
            GameData.NumberAttempts--;
            сheckNumberAttempts();         
        }
    }
}