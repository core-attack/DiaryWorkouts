using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiaryWorkouts
{
    /// <summary>
    /// Сообщения, предупреждения, возникающие в процессе работы приложения
    /// </summary>
    class Messages
    {
        /// <summary>
        /// Окно сообщения об успешном произошедшем событии
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="text"></param>
        public static void Info(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Окно сообщения об успешном произошедшем событии
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="text"></param>
        public static void Warning(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
