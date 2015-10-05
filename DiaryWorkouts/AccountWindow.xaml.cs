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
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.DataBase;
using DiaryWorkouts.ReferenceBooks;


namespace DiaryWorkouts
{
    /// <summary>
    /// Окно информации о пользователе
    /// </summary>
    public partial class AccountWindow : Window
    {
        /// <summary>
        /// Экземпляр БД
        /// </summary>
        SQLite sqlite = new SQLite();
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        User user;
        Athlete athlete;
        public AccountWindow(long userId)
        {
            InitializeComponent();
            this.userId = userId;

            sqlite.Connect();


            List<SportType> spotrTypes = sqlite.GetAllSportTypes();
            foreach (SportType st in spotrTypes)
                cbSportType.Items.Add(st.value);

            List<SportCategory> sportCategories = sqlite.GetAllSportCategories();
            foreach (SportCategory sc in sportCategories)
                cbSportCategory.Items.Add(sc.value);

            LoadUser();

        }

        private void buttonOpenCalender_Click(object sender, RoutedEventArgs e)
        {
            if (calendar.Visibility == System.Windows.Visibility.Hidden)
            {
                calendar.Visibility = System.Windows.Visibility.Visible;
                buttonOpenCalender.Content = "<";
            }
            else
            {
                calendar.Visibility = System.Windows.Visibility.Hidden;
                buttonOpenCalender.Content = ">";
            }
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // ... Get nullable DateTime from SelectedDate.
                DateTime? date = calendar.SelectedDate;
                if (date != null)
                {
                    tbBDay.Text = date.Value.ToShortDateString();
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);                
            }
        }

        /// <summary>
        /// Загружает информацию о текущем пользователе в контролы
        /// </summary>
        private void LoadUser()
        {
            user = sqlite.GetUser(userId);
            athlete = sqlite.GetAthleteByUserId(user.id);

            tbName.Text = string.Format("{0} {1} {2}", user.name, user.lastName, user.fatherName);
            tbBDay.Text = user.bDay;
            tbAddress.Text = athlete.address;
            tbPhone.Text = user.phone;
            tbPlace.Text = athlete.place;
            cbSportCategory.Text = sqlite.GetValueById(SQLite.TABLE_SPORT_CATEGORY, athlete.sportCategoryId);
            cbSportType.Text = sqlite.GetValueById(SQLite.TABLE_SPORT_TYPE, athlete.sportTypeId);
            cbSex.Text = user.sex;
            tbLogin.Text = user.login;
            tbPassword.Password = user.password;
        }
        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            //Во время валидации данных произошла ошибка?
            bool isAborted = false;

            Dictionary<string, string> data = new Dictionary<string,string>();
            string[] name = tbName.Text.Split(' ');
            for (int i = 0; i < name.Length; i++ )
                switch (i)
                {
                    case 0: data.Add("name", name[i]); break;
                    case 1: data.Add("lastName", name[i]); break;
                    case 2: data.Add("fatherName", name[i]); break;

                }
            data.Add("phone", tbPhone.Text);
            data.Add("birthday", OtherMethods.GetDate(tbBDay.Text).ToString());
            data.Add("sex", cbSex.Text);

            if (!user.login.Equals(tbLogin.Text))
                if (!sqlite.CheckUserLogin(tbLogin.Text))
                    data.Add("login", tbLogin.Text);
                else
                {
                    isAborted = true;
                    Messages.Warning("Не удалось обновить информацию о пользователе!", "Пользователь с таким логином уже есть. Пожалуйста, выберите другой.");

                }
            if (!user.password.Equals(tbPassword.Password))
                if (tbPassword.Password == tbConfirmation.Password)
                    data.Add("password", tbPassword.Password);
                else
                {
                    isAborted = true;
                    Messages.Warning("Не удалось обновить информацию о пользователе!", "Пароль и подтверждение пароля не совпадают!");
                }
            if (!isAborted)
                sqlite.UpdateUser(user.id, data);

            data.Clear();
            data.Add("sportTypeId", sqlite.GetIdByValue(SQLite.TABLE_SPORT_TYPE, cbSportType.Text).ToString());
            data.Add("sportCategoryId", sqlite.GetIdByValue(SQLite.TABLE_SPORT_CATEGORY, cbSportCategory.Text).ToString());
            data.Add("address", tbAddress.Text);
            data.Add("place", tbPlace.Text);
            if (!isAborted)
                sqlite.UpdateAthlete(athlete.id, data);

            LoadUser();
        }
    }
}
