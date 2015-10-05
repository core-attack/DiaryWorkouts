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
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.ReferenceBooks;
using System.Data;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        SQLite sqlite = new SQLite();
        /// <summary>
        /// Идентификатор выбранной записи в таблице
        /// </summary>
        long selectedId = -1;
        /// <summary>
        /// Идентификаторы выбранных записей в таблице (мультивыборка)
        /// </summary>
        List<long> selectedIds = new List<long>();
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        public AdminWindow(long userId, System.Globalization.NumberFormatInfo format)
        {
            InitializeComponent();
            sqlite.Connect();
            initialize();
            this.format = format;
            this.userId = userId;

            comboBoxTables.Focus();
        }

        /// <summary>
        /// Первичная инициализация контролов
        /// </summary>
        private void initialize()
        {
            try
            {
                string[] tableNames = sqlite.GetAllTableNames();
                foreach (string s in tableNames)
                    comboBoxTables.Items.Add(s.ToString());

                tbSearch.IsEnabled = false;
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }

        /// <summary>
        /// Заполняет таблицу данными из БД,соответствующей выбранному индексу выпадающего списка
        /// </summary>
        private void LoadAllRecords()
        {
            try
            {
                if (comboBoxTables.SelectedIndex >= 0)
                {
                    tbSearch.IsEnabled = true;
                    tbSearch.Focus();
                    TableDB.ItemsSource = null;
                    switch (comboBoxTables.Items[comboBoxTables.SelectedIndex].ToString())
                    {
                        case "athlete":
                            {
                                List<Athlete> athletes = sqlite.GetAllAthletes();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("sportTypeId");
                                dt.Columns.Add("sportCategoryId");
                                dt.Columns.Add("address");
                                dt.Columns.Add("place");
                                dt.Columns.Add("userId");
                                dt.Columns.Add("createDate");
                                foreach (Athlete athlete in athletes)
                                    dt.Rows.Add(athlete.id, athlete.sportTypeId, athlete.sportCategoryId, athlete.address, athlete.place, athlete.userId, athlete.createDate);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "coach":
                            {
                                List<Coach> coaches = sqlite.GetAllCoaches();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                foreach (Coach coach in coaches)
                                    dt.Rows.Add(coach.id);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "muscles_group":
                            {
                                List<MusclesGroup> musclesGroups = sqlite.GetAllMusclesGroups();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("value");
                                foreach (MusclesGroup mg in musclesGroups)
                                    dt.Rows.Add(mg.id, mg.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "option":
                            {
                                List<Option> opts = sqlite.GetAllOptions();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("name");
                                dt.Columns.Add("value");
                                foreach (Option o in opts)
                                    dt.Rows.Add(o.id, o.name, o.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "result":
                            {
                                List<Result> results = sqlite.GetAllResults(format);
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("time");
                                dt.Columns.Add("weight");
                                dt.Columns.Add("repeat");
                                dt.Columns.Add("distance");
                                dt.Columns.Add("points");
                                dt.Columns.Add("place");
                                foreach (Result result in results)
                                    dt.Rows.Add(result.id, result.time, result.weight, result.repeat, result.distance, result.points, result.place);
                                TableDB.ItemsSource = dt.DefaultView;

                            }
                            break;
                        case "sport_category":
                            {
                                List<SportCategory> scs = sqlite.GetAllSportCategories();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("value");
                                dt.Columns.Add("shortValue");
                                foreach (SportCategory sc in scs)
                                    dt.Rows.Add(sc.id, sc.value, sc.shortValue);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "sport_type":
                            {
                                List<SportType> sts = sqlite.GetAllSportTypes();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("value");
                                foreach (SportType st in sts)
                                    dt.Rows.Add(st.id, st.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "user":
                            {
                                List<User> users = sqlite.GetAllUsers();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("name");
                                dt.Columns.Add("lastName");
                                dt.Columns.Add("fatherName");
                                dt.Columns.Add("phone");
                                dt.Columns.Add("birthday");
                                dt.Columns.Add("sex");
                                dt.Columns.Add("login");
                                dt.Columns.Add("password");
                                dt.Columns.Add("createDate");
                                foreach (User user in users)
                                    dt.Rows.Add(user.id, user.name, user.lastName, user.fatherName, user.phone, user.bDay, user.sex, user.login, user.password, user.createDate);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "work":
                            {
                                List<Work> works = sqlite.GetAllWorks();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("workoutId");
                                dt.Columns.Add("resultId");
                                dt.Columns.Add("workTypeId");
                                dt.Columns.Add("comment");
                                foreach (Work work in works)
                                    dt.Rows.Add(work.id, work.workoutId, work.resultId, work.workTypeId, work.comment);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "work_type":
                            {
                                List<WorkType> wts = sqlite.GetAllWorkTypes();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("sportTypeId");
                                dt.Columns.Add("value");
                                foreach (WorkType wt in wts)
                                    dt.Rows.Add(wt.id, wt.sportTypeId, wt.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "workout":
                            {
                                List<Workout> workouts = sqlite.GetAllWorkouts();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("date");
                                dt.Columns.Add("timeBegin");
                                dt.Columns.Add("timeEnd");
                                dt.Columns.Add("warmUp");
                                dt.Columns.Add("musclesGroupId");
                                dt.Columns.Add("workoutPlanId");
                                dt.Columns.Add("workoutPlanTypeId");
                                dt.Columns.Add("workoutTypeId");
                                dt.Columns.Add("athleteId");
                                dt.Columns.Add("createDate");
                                foreach (Workout workout in workouts)
                                    dt.Rows.Add(workout.id, workout.date, workout.timeBegin, workout.timeEnd, workout.warmUp, workout.musclesGroupId, workout.workoutPlanId, workout.workoutPlanTypeId, workout.workoutTypeId, workout.athleteId, workout.createDate);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "workout_type":
                            {
                                List<WorkoutType> wts = sqlite.GetAllWorkoutTypes();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("value");
                                foreach (WorkoutType wt in wts)
                                    dt.Rows.Add(wt.id, wt.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "workout_plan":
                            {
                                List<WorkoutPlan> wps = sqlite.GetAllWorkoutPlan();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("title");
                                dt.Columns.Add("period");
                                dt.Columns.Add("creatorId");
                                foreach (WorkoutPlan wp in wps)
                                    dt.Rows.Add(wp.id, wp.title, wp.period, wp.creatorId);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                        case "workout_plan_type":
                            {
                                List<WorkoutPlanType> wps = sqlite.GetAllWorkoutPlanTypes();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("id");
                                dt.Columns.Add("value");
                                foreach (WorkoutPlanType wp in wps)
                                    dt.Rows.Add(wp.id, wp.value);
                                TableDB.ItemsSource = dt.DefaultView;
                            }
                            break;
                    }
                }
                else
                {
                    tbSearch.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }


        /// <summary>
        /// Заполняет таблицу данными из БД при выборе соответствующего индекса в выпадающем списке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAllRecords();
        }

        private void TableDB_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show("Бинго!");    
        }

        private void TableDB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
        }

        /// <summary>
        /// Добавляет в выбранную таблицу запись
        /// </summary>
        private void AddRow()
        {
            switch (comboBoxTables.Text)
            {
                case "athlete":
                    {
                    }
                    break;
                case "coach":
                    {
                    }
                    break;
                case "muscles_group":
                    {
                    }
                    break;
                case "result":
                    {
                    }
                    break;
                case "sport_category":
                    {
                    }
                    break;
                case "sport_type":
                    {
                    }
                    break;
                case "user":
                    {
                    }
                    break;
                case "work":
                    {
                    }
                    break;
                case "work_type":
                    {
                    }
                    break;
                case "workout":
                    {
                    }
                    break;
                case "workout_type":
                    {
                    }
                    break;
                case "workout_plan":
                    {
                    }
                    break;
            }

        }
        /// <summary>
        /// Считывание в консоль данных выделенной строки таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableDB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TableDB.SelectedItems.Count > 1)
                {
                    selectedId = 0;
                    foreach (object ob in TableDB.SelectedItems)
                    {
                        string s = ob.GetType().FullName;
                        if (ob.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                        {
                            selectedIds.Add(Convert.ToInt64(((DataRowView)ob).Row.ItemArray[0]));
                        }
                    }
                }
                else
                {
                    selectedIds.Clear();
                    switch (comboBoxTables.Text)
                    {
                        case "athlete":
                            {
                                byte sportTypeId = 0;
                                byte sportCategoryId = 0;
                                string address = "-1";
                                string place = "-1";
                                long userId = -1;
                                long createDate = 0;
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "sportTypeId": { sportTypeId = Convert.ToByte(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "sportCategoryId": { sportCategoryId = Convert.ToByte(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "address": { address = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "place": { place = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "userId": { userId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "createDate": { createDate = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} sportTypeId = {1} sportCategoryId = {2} address = {3} place = {4} userId = {5} createDate = {6}",
                                //    selectedId, sportTypeId, sportCategoryId, address, place, userId, createDate));
                            }
                            break;
                        case "coach":
                            {

                            }
                            break;
                        case "muscles_group":
                            {
                                string value = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                OtherMethods.Debug(string.Format("id = {0} value = {1}", selectedId, value));
                            }
                            break;
                        case "result":
                            {
                                string time = "-1";
                                float weight = -1;
                                byte repeat = 0;
                                float distance = -1;
                                int points = -1;
                                int place = -1;
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "time": { time = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "weight": { weight = float.Parse(Convert.ToString(((DataRowView)rows).Row.ItemArray[i]), format); }
                                                    break;
                                                case "repeat": { repeat = Convert.ToByte(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "distance": { distance = float.Parse(Convert.ToString(((DataRowView)rows).Row.ItemArray[i]), format); }
                                                    break;
                                                case "points": { points = Convert.ToInt32(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "place": { place = Convert.ToInt32(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} time = {1} weight = {2} repeat = {3} points = {4} place = {5}",
                                //    selectedId, time, weight, repeat, distance, points, place));

                            }
                            break;
                        case "option":
                            {
                                string name = "-1";
                                string value = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "name": { name = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                OtherMethods.Debug(string.Format("id = {0} name = {1} value = {2}", selectedId, name, value));
                            }
                            break;
                        case "sport_category":
                            {
                                string value = "-1";
                                string shortValue = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "shortValue": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                OtherMethods.Debug(string.Format("id = {0} value = {1} shortValue = {2}", selectedId, value, shortValue));
                            }
                            break;
                        case "sport_type":
                            {
                                string value = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} value = {1}", selectedId, value));
                            }
                            break;
                        case "user":
                            {
                                string name = "";
                                string lastName = "";
                                string fatherName = "";
                                string phone = "";
                                string birthday = "";
                                string sex = "";
                                string login = "";
                                string password = "";
                                string createDate = "";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "name": { name = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "lastName": { lastName = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "fatherName": { fatherName = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "phone": { phone = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "birthday": { birthday = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "sex": { sex = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "login": { login = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "password": { password = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "createDate": { createDate = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} name = {1} lastName = {2} fatherName = {3} phone = {4} birthday = {5} sex = {6} login = {7} password = {8} createDate = {9}",
                                //    selectedId, name, lastName, fatherName, phone, birthday, sex, login, password, createDate));
                            }
                            break;
                        case "work":
                            {
                                long workoutId = -1;
                                long resultId = -1;
                                Int16 workTypeId = -1;
                                string comment = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "workoutId": { workoutId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "resultId": { resultId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "workTypeId": { workTypeId = Convert.ToInt16(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "comment": { comment = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} workoutId = {1} resultId = {2} workTypeId = {3} comment = {4}", selectedId, workoutId, resultId, workTypeId, comment));
                            }
                            break;
                        case "work_type":
                            {
                                string value = "-1";
                                byte sportTypeId = 0;
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "sportTypeId": { sportTypeId = Convert.ToByte(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} value = {1} sportTypeId = {2}", selectedId, value, sportTypeId));
                            }
                            break;
                        case "workout":
                            {
                                string date = "-1";
                                string beginTime = "-1";
                                string endTime = "-1";
                                byte warmUp = 0;
                                short musclesGroupId = 0;
                                long workoutPlanId = 0;
                                short workoutTypeId = 0;
                                long athleteId = -1;
                                string createDate = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "date": { date = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "timeBegin": { beginTime = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "timeEnd": { endTime = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "warmUp": { warmUp = Convert.ToByte(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "musclesGroupId": { musclesGroupId = Convert.ToInt16(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "workoutPlanId": { workoutPlanId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "workoutTypeId": { workoutTypeId = Convert.ToInt16(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "athleteId": { athleteId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "createDate": { createDate = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} date = {1} beginTime = {2} endTime = {3} musclesGroupId = {4} workoutPlanId = {5} workoutTypeId = {6} athleteId = {7} createDate = {8}",
                                //    selectedId, date, beginTime, endTime, musclesGroupId, workoutPlanId, workoutTypeId, athleteId, createDate));
                            }
                            break;
                        case "workout_type":
                            {
                                string value = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} value = {1}", selectedId, value));
                            }
                            break;
                        case "workout_plan":
                            {
                                string title = "-1";
                                string period = "-1";
                                long createrId = -1;
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "title": { title = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "period": { period = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "createrId": { createrId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} title = {1} = period = {2} createrId = {3}", selectedId, title, period, createrId));
                            }
                            break;
                        case "workout_plan_type":
                            {
                                string value = "-1";
                                foreach (object rows in e.AddedItems)
                                {
                                    if (rows.GetType().FullName.Equals("System.Data.DataRowView")) //последняя пустая строка вызывает исключение, поэтому смотрим тип строки
                                    {
                                        DataColumnCollection columns = ((DataRowView)rows).Row.Table.Columns;
                                        for (int i = 0; i < ((DataRowView)rows).Row.ItemArray.Length; i++)
                                            switch (columns[i].ColumnName)
                                            {
                                                case "id": { selectedId = Convert.ToInt64(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                                case "value": { value = Convert.ToString(((DataRowView)rows).Row.ItemArray[i]); }
                                                    break;
                                            }
                                    }
                                }
                                //OtherMethods.Debug(string.Format("id = {0} title = {1} = period = {2} createrId = {3}", selectedId, title, period, createrId));
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        /// <summary>
        /// Изменение полей таблицы с сохранением в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TableDB_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try {
                Dictionary<string, string> data = new Dictionary<string, string>();
                DataGridColumn column = TableDB.Columns[e.Column.DisplayIndex];
                string newValue = ((TextBox)e.Column.GetCellContent(e.Row)).Text;
                switch (comboBoxTables.Text)
                {
                    case "athlete":
                        {
                            byte sportTypeId = 0;
                            byte sportCategoryId = 0;
                            string address = "-1";
                            string place = "-1";
                            long userId = -1;
                            long createDate = 0;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "sportTypeId":
                                    {
                                        sportTypeId = Convert.ToByte(newValue);
                                        data.Add(column.Header.ToString(), sportTypeId.ToString());
                                    }
                                    break;
                                case "sportCategoryId":
                                    {
                                        sportCategoryId = Convert.ToByte(newValue);
                                        data.Add(column.Header.ToString(), sportCategoryId.ToString());
                                    }
                                    break;
                                case "address":
                                    {
                                        address = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), address.ToString());
                                    }
                                    break;
                                case "place":
                                    {
                                        place = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), place.ToString());
                                    }
                                    break;
                                case "userId":
                                    {
                                        userId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), userId.ToString());
                                    }
                                    break;
                                case "createDate":
                                    {
                                        createDate = OtherMethods.GetDate(newValue);
                                        data.Add(column.Header.ToString(), createDate.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateAthlete(selectedId, data);
                        }
                        break;
                    case "coach":
                        {

                        }
                        break;
                    case "muscles_group":
                        {
                            string value = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateMusclesGroup(selectedId, data);
                        }
                        break;
                    case "option":
                        {
                            string name = "-1";
                            string value = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "name":
                                    {
                                        name = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), name.ToString());
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateOption(selectedId, data);
                        }
                        break;
                    case "result":
                        {
                            long time = -1;
                            float weight = -1;
                            byte repeat = 0;
                            float distance = -1;
                            int points = -1;
                            int place = -1;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "time":
                                    {
                                        time = OtherMethods.GetTime(newValue);
                                        data.Add(column.Header.ToString(), time.ToString());
                                    }
                                    break;
                                case "weight":
                                    {
                                        weight = float.Parse(newValue);
                                        data.Add(column.Header.ToString(), weight.ToString());
                                    }
                                    break;
                                case "repeat":
                                    {
                                        repeat = Convert.ToByte(newValue);
                                        data.Add(column.Header.ToString(), repeat.ToString());
                                    }
                                    break;
                                case "distance":
                                    {
                                        distance = float.Parse(newValue);
                                        data.Add(column.Header.ToString(), distance.ToString());
                                    }
                                    break;
                                case "points":
                                    {
                                        points = Convert.ToInt32(newValue);
                                        data.Add(column.Header.ToString(), points.ToString());
                                    }
                                    break;
                                case "place":
                                    {
                                        place = Convert.ToInt32(newValue);
                                        data.Add(column.Header.ToString(), place.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateResult(selectedId, data);

                        }
                        break;
                    case "sport_category":
                        {
                            string value = "-1";
                            string shortValue = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                                case "shortValue":
                                    {
                                        shortValue = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), shortValue.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateSportCategory(selectedId, data);
                        }
                        break;
                    case "sport_type":
                        {
                            string value = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateSportType(selectedId, data);
                        }
                        break;
                    case "user":
                        {
                            string name = "";
                            string lastName = "";
                            string fatherName = "";
                            string phone = "";
                            long birthday = -1;
                            string sex = "";
                            string login = "";
                            string password = "";
                            long createDate = -1;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "name":
                                    {
                                        name = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), name.ToString());
                                    }
                                    break;
                                case "lastName":
                                    {
                                        lastName = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), lastName.ToString());
                                    }
                                    break;
                                case "fatherName":
                                    {
                                        fatherName = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), fatherName.ToString());
                                    }
                                    break;
                                case "phone":
                                    {
                                        phone = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), phone.ToString());
                                    }
                                    break;
                                case "birthday":
                                    {
                                        birthday = OtherMethods.GetDate(newValue);
                                        data.Add(column.Header.ToString(), birthday.ToString());
                                    }
                                    break;
                                case "sex":
                                    {
                                        sex = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), sex.ToString());
                                    }
                                    break;
                                case "login":
                                    {
                                        login = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), login.ToString());
                                    }
                                    break;
                                case "password":
                                    {
                                        password = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), password.ToString());
                                    }
                                    break;
                                case "createDate":
                                    {
                                        createDate = OtherMethods.GetDate(newValue);
                                        data.Add(column.Header.ToString(), createDate.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateUser(selectedId, data);
                        }
                        break;
                    case "work":
                        {
                            OtherMethods.Debug("column: " + e.Column.DisplayIndex + "row: " + e.Row.GetIndex() + "content: " + ((TextBox)e.Column.GetCellContent(e.Row)).Text);

                            long workoutId = -1;
                            long resultId = -1;
                            Int16 workTypeId = -1;
                            string comment = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "workoutId":
                                    {
                                        workoutId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), workoutId.ToString());
                                    }
                                    break;
                                case "resultId":
                                    {
                                        resultId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), resultId.ToString());
                                    }
                                    break;
                                case "workTypeId":
                                    {
                                        workTypeId = Convert.ToInt16(newValue);
                                        data.Add(column.Header.ToString(), workTypeId.ToString());
                                    }
                                    break;
                                case "comment":
                                    {
                                        comment = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), comment.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWork(selectedId, data);
                        }
                        break;
                    case "work_type":
                        {

                            string value = "-1";
                            byte sportTypeId = 0;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                                case "sportTypeId":
                                    {
                                        sportTypeId = Convert.ToByte(newValue);
                                        data.Add(column.Header.ToString(), sportTypeId.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWorkType(selectedId, data);
                        }
                        break;
                    case "workout":
                        {
                            long date = -1;
                            long beginTime = -1;
                            long endTime = -1;
                            short musclesGroupId = 0;
                            long workoutPlanId = 0;
                            short workoutTypeId = 0;
                            byte warmUp = 0;
                            long athleteId = -1;
                            long createDate = -1;
                            short workoutPlanTypeId = -1;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "date":
                                    {
                                        date = OtherMethods.GetDate(newValue);
                                        data.Add(column.Header.ToString(), date.ToString());
                                    }
                                    break;
                                case "timeBegin":
                                    {
                                        beginTime = OtherMethods.GetTime(newValue);
                                        data.Add(column.Header.ToString(), beginTime.ToString());
                                    }
                                    break;
                                case "timeEnd":
                                    {
                                        endTime = OtherMethods.GetTime(newValue);
                                        data.Add(column.Header.ToString(), endTime.ToString());
                                    }
                                    break;
                                case "warmUp":
                                    {
                                        warmUp = Convert.ToByte(newValue);
                                        data.Add(column.Header.ToString(), warmUp.ToString());
                                    }
                                    break;
                                case "musclesGroupId":
                                    {
                                        musclesGroupId = Convert.ToInt16(newValue);
                                        data.Add(column.Header.ToString(), musclesGroupId.ToString());
                                    }
                                    break;
                                case "workoutPlanId":
                                    {
                                        workoutPlanId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), workoutPlanId.ToString());
                                    }
                                    break;
                                case "workoutPlanTypeId":
                                    {
                                        workoutPlanTypeId = Convert.ToInt16(newValue);
                                        data.Add(column.Header.ToString(), workoutPlanTypeId.ToString());
                                    }
                                    break;
                                case "workoutTypeId":
                                    {
                                        workoutTypeId = Convert.ToInt16(newValue);
                                        data.Add(column.Header.ToString(), workoutTypeId.ToString());
                                    }
                                    break;
                                case "athleteId":
                                    {
                                        athleteId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), athleteId.ToString());
                                    }
                                    break;
                                case "createDate":
                                    {
                                        createDate = OtherMethods.GetDate(newValue);
                                        data.Add(column.Header.ToString(), createDate.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWorkout(selectedId, data);
                        }
                        break;
                    case "workout_type":
                        {

                            string value = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWorkoutType(selectedId, data);
                        }
                        break;
                    case "workout_plan":
                        {
                            string title = "-1";
                            string period = "-1";
                            long createrId = -1;
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "title":
                                    {
                                        title = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), title.ToString());
                                    }
                                    break;
                                case "period":
                                    {
                                        period = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), period.ToString());
                                    }
                                    break;
                                case "createrId":
                                    {
                                        createrId = Convert.ToInt64(newValue);
                                        data.Add(column.Header.ToString(), createrId.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWorkoutPlan(selectedId, data);
                        }
                        break;
                    case "workout_plan_type":
                        {
                            string value = "-1";
                            switch (column.Header.ToString())
                            {
                                case "id":
                                    {
                                        MessageBox.Show("Изменить идентификатор записи невозможно!", "Справка", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ((TextBox)e.Column.GetCellContent(e.Row)).Text = selectedId.ToString();
                                    }
                                    break;
                                case "value":
                                    {
                                        value = Convert.ToString(newValue);
                                        data.Add(column.Header.ToString(), value.ToString());
                                    }
                                    break;
                            }
                            if (data.Count > 0)
                                sqlite.UpdateWorkoutPlanType(selectedId, data);
                        }
                        break;
                }
                
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        /// <summary>
        /// Удаляет запись в БД по идентификатору
        /// </summary>
        /// <param name="selectedId"></param>
        private void DeleteRecord(long selectedId)
        {
            switch (comboBoxTables.Text)
            {
                case "athlete":
                    {
                        sqlite.DeleteAthlete(selectedId);
                    }
                    break;
                case "coach":
                    {
                    }
                    break;
                case "muscles_group":
                    {
                        sqlite.DeleteMusclesGroup(selectedId);
                    }
                    break;
                case "option":
                    {
                        sqlite.DeleteOption(selectedId);
                    }
                    break;
                case "result":
                    {
                        sqlite.DeleteResult(selectedId);
                    }
                    break;
                case "sport_category":
                    {
                        sqlite.DeleteSportCategory(selectedId);
                    }
                    break;
                case "sport_type":
                    {
                        sqlite.DeleteSportType(selectedId);
                    }
                    break;
                case "user":
                    {
                        sqlite.DeleteUser(selectedId);
                    }
                    break;
                case "work":
                    {
                        sqlite.DeleteWork(selectedId);
                    }
                    break;
                case "work_type":
                    {
                        sqlite.DeleteWorkType(selectedId);
                    }
                    break;
                case "workout":
                    {
                        sqlite.DeleteWorkout(selectedId);
                    }
                    break;
                case "workout_type":
                    {
                        sqlite.DeleteWorkoutType(selectedId);
                    }
                    break;
                case "workout_plan":
                    {
                        sqlite.DeleteWorkoutPlan(selectedId);
                    }
                    break;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIds.Count > 0)
            {
                foreach (long id in selectedIds)
                    DeleteRecord(id);
            }
            else
                DeleteRecord(selectedId);
            LoadAllRecords();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                LoadAllRecords();
                DataView dv = (DataView)TableDB.ItemsSource;
                if (dv != null)
                {
                    
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        bool includeRequest = false;
                        foreach (object cell in dv.Table.Rows[i].ItemArray)
                        {
                            if (cell != null)
                                if (cell.ToString().ToLower().IndexOf(tbSearch.Text.ToLower()) != -1)
                                {
                                    includeRequest = true;
                                    break;
                                }
                        }
                        if (!includeRequest)
                        {
                            dv.Table.Rows.Remove(dv.Table.Rows[i]);
                            i--;
                        }
                    }
                    TableDB.ItemsSource = dv;
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }

        private void tbSearch_GotMouseCapture(object sender, MouseEventArgs e)
        {
            tbSearch.SelectAll();
        }

        private void tbSearch_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            tbSearch.SelectAll();
        }

        private void comboBoxTables_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            comboBoxTables.IsDropDownOpen = true;

        }

        private void comboBoxTables_GotMouseCapture(object sender, MouseEventArgs e)
        {
            comboBoxTables.IsDropDownOpen = true;

        }

        
    }
}
