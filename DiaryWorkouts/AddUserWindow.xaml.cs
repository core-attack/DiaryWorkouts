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
using DiaryWorkouts.ReferenceBooks;
using DiaryWorkouts.DataBase;
using System.Security.Cryptography;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        SQLite sqlite = new SQLite();
        public AddUserWindow(long userId)
        {
            InitializeComponent();
            sqlite.Connect();

            List<UserRole> roles = sqlite.GetAllUserRoles();
            foreach (UserRole ur in roles)
                comboBoxUserRole.Items.Add(ur.value);

            List<SportType> spotrTypes = sqlite.GetAllSportTypes();
            foreach (SportType st in spotrTypes)
                comboBoxSportType.Items.Add(st.value);

            List<SportCategory> sportCategories = sqlite.GetAllSportCategories();
            foreach (SportCategory sc in sportCategories)
                comboBoxSportCategory.Items.Add(sc.value);

            this.userId = userId;
        }
        /// <summary>
        /// Get MD5 hash
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verify a hash against a string.
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void buttonCreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pass1.Password.Equals(pass2.Password))
                {
                    string pass = ""; 
                    using (MD5 md5Hash = MD5.Create())
                    {
                        pass = GetMd5Hash(md5Hash, pass1.Password);
                    }
                    string[] names = tbName.Text.Split(' ');
                    string name = names[0];
                    string lastName = names.Length > 1 ? names[1] : "";
                    string fatherName = names.Length > 2 ? names[2] : "";
                    string sex = radioMen.IsChecked ?? true ? "М" : "Ж";
                    string[] bDayArr = tbBDay.Text.Split('.');
                    DateTime bDay = new DateTime(int.Parse(bDayArr[2]), int.Parse(bDayArr[1]), int.Parse(bDayArr[0]));
                    string phone = tbPhone.Text;
                    long userRoleId = sqlite.GetIdByValue(SQLite.TABLE_USER_ROLE, comboBoxUserRole.Text);
                    long sportTypeId = sqlite.GetIdByValue(SQLite.TABLE_SPORT_TYPE, comboBoxSportType.Text);
                    long sportCategoryId = sqlite.GetIdByValue(SQLite.TABLE_SPORT_CATEGORY, comboBoxSportCategory.Text);
                    string address = tbAddress.Text;
                    string place = tbPlace.Text;
                    string login = "";
                    User u = new User(0, name, lastName, fatherName, login, pass, phone, sex, DateTime.Now.ToShortDateString(), bDay.ToShortDateString());
                    sqlite.AddUser(u);
                    long userId = sqlite.GetIdByUserInfo(u);
                    switch (userRoleId)
                    {
                        case 1: { }
                            break;
                        case 2: { sqlite.AddAthlete(new Athlete(0, (byte)sportTypeId, (byte)sportCategoryId, address, place, userId, DateTime.Now.ToShortDateString())); }
                            break;
                        case 3: { }
                            break;
                    }
                }
                else
                    MessageBox.Show("Пароли не совпадают!", "Повторите ввод", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
    }
}
