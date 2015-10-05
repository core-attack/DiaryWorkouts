using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite.EF6;
using System.Threading.Tasks;
using System.Windows;
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.ReferenceBooks;
using System.IO;
using System.Data.SQLite;

namespace DiaryWorkouts.DataBase
{
    partial class SQLite
    {
        public const string EXPORT_DIRECTORY_NAME = "export files/";
        public const string EXPORT_FILE_NAME_PREFIX = EXPORT_DIRECTORY_NAME + "Data Base Export at ";
        public const string SQL_LOG_DIRECTORY = "logs/";
        public const string SQL_LOG_FILE_NAME = "sql log at ";

        private const int OPTION_DEFAULT_START_TIME_ID = 2;
        private const int OPTION_DEFAULT_END_TIME_ID = 3;

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public const string DB_NAME = "SQLite.db";
        /// <summary>
        /// Разделители поискового запроса
        /// </summary>
        public string[] SEARCH_REQUEST_SEPARATORS = { ",", ";" };

        #region[Имена таблиц]
        /// <summary>
        /// Таблица спортсменов
        /// </summary>
        public const string TABLE_ATHLETE = "athlete";
        /// <summary>
        /// Таблица тренеров
        /// </summary>
        public const string TABLE_COACH = "coach";
        /// <summary>
        /// Таблица типов поверхностей
        /// </summary>
        public const string TABLE_GROUND = "ground";
        /// <summary>
        /// Таблица групп мыщц
        /// </summary>
        public const string TABLE_MUSCLES_GROUP = "muscles_group";
        /// <summary>
        /// Таблица настроек приложения
        /// </summary>
        public const string TABLE_OPTIONS = "option";
        /// <summary>
        /// Таблица результатов
        /// </summary>
        public const string TABLE_RESULT = "result";
        /// <summary>
        /// Таблица спортивных разрядов
        /// </summary>
        public const string TABLE_SPORT_CATEGORY = "sport_category";
        /// <summary>
        /// Таблица видов спорта
        /// </summary>
        public const string TABLE_SPORT_TYPE = "sport_type";
        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public const string TABLE_USER = "user";
        /// <summary>
        /// Таблица пользовательских ролей
        /// </summary>
        public const string TABLE_USER_ROLE = "user_role";
        /// <summary>
        /// Таблица работ
        /// </summary>
        public const string TABLE_WORK = "work";
        /// <summary>
        /// Таблица типов работ
        /// </summary>
        public const string TABLE_WORK_TYPE = "work_type";
        /// <summary>
        /// Таблица тренировок
        /// </summary>
        public const string TABLE_WORKOUT = "workout";
        /// <summary>
        /// Таблица типов тренировок
        /// </summary>
        public const string TABLE_WORKOUT_TYPE = "workout_type";
        /// <summary>
        /// Таблица типов планов тренировок
        /// </summary>
        public const string TABLE_WORKOUT_PLAN_TYPE = "workout_plan_type";
        /// <summary>
        /// Таблица планов тренировок
        /// </summary>
        public const string TABLE_WORKOUT_PLAN = "workout_plan";

        #endregion

        /// <summary>
        /// Соединение с базой данных
        /// </summary>
        private SQLiteConnection connection;
        /// <summary>
        /// Выводить в консоль?
        /// </summary>
        private bool logging = true;

        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public SQLite() {
            //лог нужен только для отладки проги, потом его нужно удалить
            //if (!Directory.Exists(SQL_LOG_DIRECTORY))
                //Directory.CreateDirectory(SQL_LOG_DIRECTORY);
        }

        /// <summary>
        /// Устанавливает соединение с базой данных
        /// </summary>
        public void Connect(){
            connection = new SQLiteConnection();
            connection.ConnectionString = "Data Source=" + DB_NAME;
            connection.Open();
            CheckDataBaseTables();
        }

        /// <summary>
        /// Закрывает соединение с базой данных
        /// </summary>
        public void Disconnect(){
            if( connection != null )
                connection.Dispose();
        }

        /// <summary>
        /// Проверяет соединение с базой данных
        /// </summary>
        /// <returns>True, если база данных подключена</returns>
        public bool CheckDB() {
            return connection != null;
        }

        #region[добавление, обновление записей в БД]
        /// <summary>
        /// Функция добавления новой записи в базу данных
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Insert(SQLiteCommand cmd, String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                cmd.CommandText = String.Format("insert into {0}({1}) values({2});", tableName, columns, values);

                if (logging)
                    OtherMethods.Debug(cmd.CommandText);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                returnCode = false;
                ErrorsHandler.ShowError(e);
                //throw e;
            }
            return returnCode;
        }

        /// <summary>
        /// Функция обновления записи в базе данных
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Update(SQLiteCommand cmd, String tableName, long id, Dictionary<String, String> data)
        {
            String columns = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0}='{1}',", val.Key.ToString(), val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            try
            {
                cmd.CommandText = String.Format("update {0} set {1} where [ID]={2};", tableName, columns, id);

                if (logging)
                    OtherMethods.Debug(cmd.CommandText);

                cmd.ExecuteNonQuery();
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        /// Функция обновления записи в базе данных
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Update(SQLiteCommand cmd, String tableName, Int16 id, Dictionary<String, String> data)
        {
            String columns = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0}='{1}',", val.Key.ToString(), val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            try
            {
                cmd.CommandText = String.Format("update {0} set {1} where [ID]={2};", tableName, columns, id);

                if (logging)
                    OtherMethods.Debug(cmd.CommandText);

                cmd.ExecuteNonQuery();
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        /// Функция обновления записи в базе данных
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Update(SQLiteCommand cmd, String tableName, byte id, Dictionary<String, String> data)
        {
            String columns = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0}='{1}',", val.Key.ToString(), val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            try
            {
                cmd.CommandText = String.Format("update {0} set {1} where [ID]={2};", tableName, columns, id);

                if (logging)
                    OtherMethods.Debug(cmd.CommandText);

                cmd.ExecuteNonQuery();

            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        /// Функция удаления записи из базы данных
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Delete(SQLiteCommand cmd, String tableName, long id)
        {
            Boolean returnCode = true;
            try
            {
                cmd.CommandText = String.Format("delete from {0} where id = {1};", tableName, id);

                if (logging)
                    OtherMethods.Debug(cmd.CommandText);

                cmd.ExecuteNonQuery();
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }
        #endregion
        
        /// <summary>
        /// Записывает в БД выбранный для загрузки план тренировок
        /// </summary>
        /// <param name="id"></param>
        public void SetOptionCurrentWorkoutPlan(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("name", "activeWorkoutPlanId");
                data.Add("value", id.ToString());
                bool ok = Update(cmd, TABLE_OPTIONS, 1, data);
                if (!ok)
                    throw new Exception("Не удалось обновить опцию!");
            }
        }

        /// <summary>
        /// Выгружает из БД определение существования записей кардио и силовых работ на определенный период
        /// </summary>
        /// <param name="beginDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата (включительно)</param>
        /// <returns>Дата тренировки, есть ли кардио, есть ли силовая</returns>
        public Dictionary<long, Dictionary<string, bool>> IsKardioOrHardWorkRecordExistBetweenDates(DateTime beginDate, DateTime endDate)
        {
            string kardioKey = "kardio";
            string hardWorkKey = "hardWork";
            string eventKey = "event";
            Dictionary<long, Dictionary<string, bool>> dic = new Dictionary<long, Dictionary<string, bool>>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "select distinct w.date, st.id, wrkttp.id from " 
                    + TABLE_WORKOUT + " w, " 
                    + TABLE_WORK + " work, " 
                    + TABLE_WORK_TYPE + " wt, " 
                    + TABLE_SPORT_TYPE + " st, " 
                    + TABLE_WORKOUT_TYPE + " wrkttp"
                + " where work.workoutId = w.id"
                + " and wt.id = work.workTypeId"
                + " and wrkttp.id = w.workoutTypeId"
                + " and wrkttp.id <> 4"
                + " and st.id = wt.sportTypeId"
                + " and w.date >= " + OtherMethods.GetDate(beginDate)
                + " and w.date <= " + OtherMethods.GetDate(endDate);
                DbDataReader reader = command.ExecuteReader();

                long currentDate = 0;
                bool kardio = false;
                bool hardWork = false;
                bool hasEvent = false;
                Dictionary<string, bool> values = new Dictionary<string, bool>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        if (currentDate != reader.GetInt64(0))
                        {
                            kardio = false;
                            hardWork = false;
                            hasEvent = false;
                            currentDate = reader.GetInt64(0);
                            if (!dic.ContainsKey(currentDate))
                                dic.Add(currentDate, new Dictionary<string, bool>());
                            else
                                dic[currentDate] = new Dictionary<string, bool>();
                        }

                        if (reader.GetByte(2) == 3)
                            hasEvent = true;
                        if (reader.GetByte(1) == 1)
                            kardio = true;
                        else if (reader.GetByte(1) == 2)
                            hardWork = true;


                        if (!dic[currentDate].ContainsKey(eventKey))
                            dic[currentDate].Add(eventKey, hasEvent);
                        else
                            dic[currentDate][eventKey] = hasEvent;
                        if (!dic[currentDate].ContainsKey(kardioKey))
                            dic[currentDate].Add(kardioKey, kardio);
                        else
                            dic[currentDate][kardioKey] = kardio;
                        if (!dic[currentDate].ContainsKey(hardWorkKey))
                            dic[currentDate].Add(hardWorkKey, hardWork);
                        else
                            dic[currentDate][hardWorkKey] = hardWork;

                        
                    }
                }
            }
            

            return dic;
        }

        /// <summary>
        /// Отправляет произвольный скрип в БД
        /// </summary>
        /// <returns></returns>
        public DbDataReader SendUserRequest(string request)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = request;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    return reader;
                }
            }
            return null;
        }

        
        /// <summary>
        /// Экспортирует данные из БД
        /// </summary>
        /// <param name="fileNamePart">Часть имени выходного файла</param>
        public void ExportDB(string fileNamePart, System.Globalization.NumberFormatInfo format, int MAX_COUNT_FILES_IN_DIR)
        {
            try
            {
                List<string> result = new List<string>();
                List<Athlete> list1 = GetAllAthletes();
                List<Coach> list2 = GetAllCoaches();
                List<MusclesGroup> list3 = GetAllMusclesGroups();
                List<Result> list4 = GetAllResults(format);
                List<SportCategory> list5 = GetAllSportCategories();
                List<SportType> list6 = GetAllSportTypes();
                List<UserRole> list7 = GetAllUserRoles();
                List<User> list8 = GetAllUsers();
                List<WorkoutPlan> list9 = GetAllWorkoutPlan();
                List<Workout> list10 = GetAllWorkouts();
                List<WorkoutType> list11 = GetAllWorkoutTypes();
                List<Work> list12 = GetAllWorks();
                List<WorkType> list13 = GetAllWorkTypes();
                List<Option> list14 = GetAllOptions();

                foreach (Athlete a in list1)
                    result.Add(a.GetSQLInsert());
                foreach (Coach a in list2)
                    result.Add(a.GetSQLInsert());
                foreach (MusclesGroup a in list3)
                    result.Add(a.GetSQLInsert());
                foreach (Result a in list4)
                    result.Add(a.GetSQLInsert());
                foreach (SportCategory a in list5)
                    result.Add(a.GetSQLInsert());
                foreach (SportType a in list6)
                    result.Add(a.GetSQLInsert());
                foreach (UserRole a in list7)
                    result.Add(a.GetSQLInsert());
                foreach (User a in list8)
                    result.Add(a.GetSQLInsert());
                foreach (WorkoutPlan a in list9)
                    result.Add(a.GetSQLInsert());
                foreach (Workout a in list10)
                    result.Add(a.GetSQLInsert());
                foreach (WorkoutType a in list11)
                    result.Add(a.GetSQLInsert());
                foreach (Work a in list12)
                    result.Add(a.GetSQLInsert());
                foreach (WorkType a in list13)
                    result.Add(a.GetSQLInsert());
                foreach (Option a in list14)
                    result.Add(a.GetSQLInsert());
                DateTime now = DateTime.Now;

                if (!Directory.Exists(EXPORT_DIRECTORY_NAME))
                    Directory.CreateDirectory(EXPORT_DIRECTORY_NAME);

                string[] files = Directory.GetFiles(EXPORT_DIRECTORY_NAME);
                if (files.Length > MAX_COUNT_FILES_IN_DIR)
                {
                    Directory.Delete(EXPORT_DIRECTORY_NAME, true);
                }

                string path = string.Format("{0} {1}-{2}-{3} {4}h {5}m {6}s {7}.txt", EXPORT_FILE_NAME_PREFIX, now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond, fileNamePart);
                File.WriteAllLines(path, result.ToArray());
                MessageBox.Show("Наименование файла: " + path, "Экспорт данных завершен успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }
        /// <summary>
        /// Поиск по тренировкам (вывод через лейблы в строки)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> Search(string request, int count, int offset, string workoutType, bool kardio, bool hardwork, string musclesGroup, long athleteId, System.Globalization.NumberFormatInfo format)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                string kardioOrHardworkScript = "";
                if (kardio && !hardwork)
                    kardioOrHardworkScript = " AND wrk_type.sportTypeId = 1 ";
                else if (!kardio && hardwork)
                    kardioOrHardworkScript = "  AND wrk_type.sportTypeId = 2 ";
                else if (!kardio && !hardwork)
                    kardioOrHardworkScript = " AND wrk_type.sportTypeId > 2 ";
                string workoutTypeScript = "";
                if (workoutType != SearchWindow.DEFAULT_SEARCH_COMBOBOX_WORKOUT_TYPE)
                    workoutTypeScript = string.Format(" AND wt.id = {0} ", GetIdByValue(TABLE_WORKOUT_TYPE, workoutType));
                string musclesGroupScript = "";
                if (musclesGroup != SearchWindow.DEFAULT_SEARCH_COMBOBOX_MUSCLES_GROUPS_TEXT)
                    musclesGroupScript = string.Format(" AND mg.id = {0} ", GetIdByValue(TABLE_MUSCLES_GROUP, musclesGroup));
                string workTypeScript = "";
                if (request != "")
                    workTypeScript = string.Format(" AND (wrk_type.value LIKE '%{0}%' )", request);//OR wrk.comment LIKE '%{0}%'
                command.CommandText = "SELECT DISTINCT wrkt.date, wrkt.id, wt.value FROM "
                                        + TABLE_WORKOUT + " wrkt, "
                                        + TABLE_WORKOUT_TYPE + " wt, "
                                        + TABLE_WORK_TYPE + " wrk_type, "
                                        + TABLE_WORK + " wrk, "
                                        + TABLE_MUSCLES_GROUP + " mg "
                                        + " WHERE wrkt.athleteId = " + athleteId
                                        + " AND wrkt.workoutTypeId = wt.id "
                                        + " AND (wrkt.musclesGroupId = mg.id OR wrkt.musclesGroupId == 0)"
                                        + " AND wrk.workoutId = wrkt.id "
                                        + " AND wrk.workTypeId = wrk_type.id "
                                        + kardioOrHardworkScript
                                        + workoutTypeScript
                                        + musclesGroupScript
                                        + workTypeScript
                                        + " ORDER BY wrkt.date DESC LIMIT " + count
                                        + " OFFSET " + offset; 
                if (logging)
                    OtherMethods.Debug(command.CommandText);

                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        long date = reader.GetInt64(0);
                        long workoutId = reader.GetInt64(1);
                        string wt = reader.GetString(2);
                        string workAndResult = GetWorksAndResultsOnWorkout(workoutId, format);

                        Dictionary<string, string> workout = new Dictionary<string, string>();
                        workout.Add("id", workoutId.ToString());
                        workout.Add("date", OtherMethods.GetDate(date));
                        workout.Add("worksAndResults", workAndResult);
                        workout.Add("workoutType", wt);
                        result.Add(workout);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Поиск по тренировкам (вывод через datagrid)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DbDataReader Search2(string request, int count, int offset, string workoutType, bool kardio, bool hardwork, string musclesGroup, long athleteId, System.Globalization.NumberFormatInfo format)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                string kardioOrHardworkScript = "";
                if (kardio && !hardwork)
                    kardioOrHardworkScript = " AND wrk_type.sportTypeId = 1 ";
                else if (!kardio && hardwork)
                    kardioOrHardworkScript = "  AND wrk_type.sportTypeId = 2 ";
                else if (!kardio && !hardwork)
                    kardioOrHardworkScript = " AND wrk_type.sportTypeId > 2 ";
                string workoutTypeScript = "";
                if (workoutType != SearchWindow.DEFAULT_SEARCH_COMBOBOX_WORKOUT_TYPE)
                    workoutTypeScript = string.Format(" AND wt.id = {0} ", GetIdByValue(TABLE_WORKOUT_TYPE, workoutType));
                string musclesGroupScript = "";
                if (musclesGroup != SearchWindow.DEFAULT_SEARCH_COMBOBOX_MUSCLES_GROUPS_TEXT)
                    musclesGroupScript = string.Format(" AND mg.id = {0} ", GetIdByValue(TABLE_MUSCLES_GROUP, musclesGroup));
                string workTypeScript = "";
                if (request != "")
                {
                    string[] requests = request.Split(SEARCH_REQUEST_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
                    workTypeScript = " AND (";
                    string subs = " wrk_type.value LIKE '%{0}%' ";
                    for (int i = 0; i < requests.Length; i++)
                    {
                        if (i == 0)
                            workTypeScript += string.Format(subs, requests[i]);
                        else if (i > 0)
                            workTypeScript += string.Format(" OR " + subs, requests[i]);

                    }
                    workTypeScript += ")";

                }
                command.CommandText = "SELECT DISTINCT wrkt.id, wrkt.date, wt.value, wrk_type.value, r.time, r.weight, r.repeat FROM "
                                        + TABLE_WORKOUT + " wrkt, "
                                        + TABLE_WORKOUT_TYPE + " wt, "
                                        + TABLE_WORK_TYPE + " wrk_type, "
                                        + TABLE_WORK + " wrk, "
                                        + TABLE_MUSCLES_GROUP + " mg, "
                                        + TABLE_RESULT + " r "
                                        + " WHERE wrkt.athleteId = " + athleteId
                                        + " AND wrkt.workoutTypeId = wt.id "
                                        + " AND (wrkt.musclesGroupId = mg.id OR wrkt.musclesGroupId == 0)"
                                        + " AND wrk.workoutId = wrkt.id "
                                        + " AND wrk.workTypeId = wrk_type.id "
                                        + " AND r.id = wrk.resultId "
                                        + kardioOrHardworkScript
                                        + workoutTypeScript
                                        + musclesGroupScript
                                        + workTypeScript
                                        + " ORDER BY wrkt.date DESC LIMIT " + count
                                        + " OFFSET " + offset;
                if (logging)
                    OtherMethods.Debug(command.CommandText);

                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return reader;
                }
            }
            return null;
        }
        

        
    }
    
}
