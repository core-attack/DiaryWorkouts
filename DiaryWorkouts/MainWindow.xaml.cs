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
using DiaryWorkouts.DataBase;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        SQLite sqlite = new SQLite();
        public LoginWindow()
        {
            InitializeComponent();
            sqlite.Connect();

            //временно, чтобы не мучаться с авторизацией
            MainWindow mainWindow = new MainWindow(sqlite.GetUserId(textBoxLogin.Text, passwordBoxPassword.Password));
            mainWindow.ShowDialog();
            Close();
        }

        private void buttonSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (sqlite.CheckUser(textBoxLogin.Text, passwordBoxPassword.Password))
            {
                MainWindow mainWindow = new MainWindow(sqlite.GetUserId(textBoxLogin.Text, passwordBoxPassword.Password));
                Close();
                mainWindow.ShowDialog();
                
            }
        }

        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
