using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace DiaryWorkouts
{
    /// <summary>
    /// Класс обработчика исключений
    /// </summary>
    class ErrorsHandler
    {
        /// <summary>
        /// Выводит сообщение об ошибке пользователю
        /// </summary>
        /// <param name="e"></param>
        public static void ShowError(Exception e)
        {
            MessageBox.Show(string.Format("Сообщение : {0}\nПуть: {1}.", e.Message, e.StackTrace), "Возникла ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }
}
