using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SVPP_CS_WPF_Lab1_task1_individual__Thimbles
{

    /// <summary>
    /// Класс содержит игровые данные пользователя
    /// </summary>
    public class UserGameData: INotifyPropertyChanged
    {
        public static readonly Dictionary<string, string> Statuses = new()
        {
            {"Start", "Начните игру" },
            {"Stir", "Перемешайте" },
        };

        private int numberAttempts = 1;
        private int guessed = 0;
        private string status = Statuses.GetValueOrDefault("Start", "");

        public event PropertyChangedEventHandler? PropertyChanged;

        public int NumberAttempts
        { 
            get => numberAttempts; 
            set
            {
                numberAttempts = value;
                OnPropertyChanged(nameof(NumberAttempts));
            } 
        }
        public int Guessed
        {
            get => guessed;
            set 
            { 
                guessed = value; 
                OnPropertyChanged($"{nameof(Guessed)}");
            }
        }
        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public UserGameData() { }

        /// <summary>
        /// Сброс всех данных.
        /// </summary>
        public void Reset()
        {
            Guessed = 0;
            Status = Statuses["Start"];
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
