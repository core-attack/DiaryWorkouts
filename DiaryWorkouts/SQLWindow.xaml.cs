using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Common;
using System.Data.SQLite.EF6;
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.ReferenceBooks;
using DiaryWorkouts.DataBase;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for SQLWindow.xaml
    /// </summary>
    public partial class SQLWindow : Window
    {
        SQLite sqlite;
        public SQLWindow()
        {
            InitializeComponent();
            sqlite = new SQLite();
            sqlite.Connect();
        }

        private void buttonGoScript_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isRequestDanger(textBoxRequest.Text))
                    dataGridTable.ItemsSource = sqlite.SendUserRequest(textBoxRequest.Text);
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }

        private void buttonDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            textBoxRequest.Text = "";
            textBoxRequest.Focus();
        }

        /// <summary>
        /// Может ли запрос причинить вред БД?
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool isRequestDanger(string s)
        {
            if (s.ToLower().IndexOf("drop") != -1 || s.ToLower().IndexOf("delete") != -1 || s.ToLower().IndexOf("alter") != -1 || s.ToLower().IndexOf("update") != -1)
            {
                Messages.Warning("Отказано в доступе", "Через консоль разрешено только просматривать данные.");
                return true;
            }
            return false;
        }
    }
}
