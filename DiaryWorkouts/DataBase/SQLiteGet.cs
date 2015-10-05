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
        /// <summary>
        /// Получает все наименования таблиц
        /// </summary>
        public string[] GetAllTableNames()
        {
            try
            {
                List<string> tableNames = new List<string>();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
                    DbDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetValue(0).ToString());
                    }
                }
                tableNames.Sort();
                return tableNames.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Проверяет наличие пользователя в БД
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUser(string login, string password)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                //command.CommandText = "SELECT * FROM " + TABLE_USER + " WHERE login = '" + login + "' AND password = '"+ password + "';";
                command.CommandText = "SELECT * FROM " + TABLE_USER + " WHERE login = @login AND password = @password;";

                command.Parameters.Add("@login", System.Data.DbType.String);
                command.Parameters.Add("@password", System.Data.DbType.String);

                command.Parameters["@login"].Value = login;
                command.Parameters["@password"].Value = password;

                if (logging)
                    OtherMethods.Debug(command.CommandText);
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Проверяет наличие пользовательского логина в БД
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool CheckUserLogin(string login)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_USER + " WHERE login = @login;";
                command.Parameters.Add("@login", System.Data.DbType.String);
                command.Parameters["@login"].Value = login;

                if (logging)
                    OtherMethods.Debug(command.CommandText);
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #region[получение всех колонок из таблиц]
        /// <summary>
        /// Получает иформацию о всех пользователях
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> result = new List<User>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_USER;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new User(reader));
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Получает иформацию о всех спортсменах
        /// </summary>
        /// <returns></returns>
        public List<Athlete> GetAllAthletes()
        {
            List<Athlete> result = new List<Athlete>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_ATHLETE;
                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Athlete(reader));
                }
            }
            return result;
        }
        /// <summary>
        /// Получает иформацию о всех спортсменах
        /// </summary>
        /// <returns></returns>
        public Athlete GetAthlete(long athleteId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_ATHLETE + " WHERE id = @id";
                command.Parameters.Add("@id", System.Data.DbType.Int64);
                command.Parameters["@id"].Value = athleteId;

                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    return new Athlete(reader);
                }
            }
            return null;
        }
        /// <summary>
        /// Получает иформацию о всех спортсменах
        /// </summary>
        /// <returns></returns>
        public Athlete GetAthleteByUserId(long userId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " +
                    TABLE_ATHLETE +
                    " WHERE userId = @id";
                command.Parameters.Add("@id", System.Data.DbType.Int64);
                command.Parameters["@id"].Value = userId;

                try
                {
                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        return new Athlete(reader);
                    }
                }
                catch (Exception e)
                {
                    ErrorsHandler.ShowError(e);
                }



            }
            return null;
        }
        /// <summary>
        /// Получает иформацию о всех тренерах
        /// </summary>
        /// <returns></returns>
        public List<Coach> GetAllCoaches()
        {
            List<Coach> result = new List<Coach>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_COACH;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Coach(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех группах мышц
        /// </summary>
        /// <returns></returns>
        public List<MusclesGroup> GetAllMusclesGroups()
        {
            List<MusclesGroup> result = new List<MusclesGroup>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_MUSCLES_GROUP;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new MusclesGroup(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех результатах
        /// </summary>
        /// <returns></returns>
        public List<Result> GetAllResults(System.Globalization.NumberFormatInfo format)
        {
            List<Result> result = new List<Result>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_RESULT;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Result(reader, format));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех пользовательских ролях
        /// </summary>
        /// <returns></returns>
        public List<UserRole> GetAllUserRoles()
        {
            List<UserRole> result = new List<UserRole>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_USER_ROLE;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new UserRole(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех спортивных разрядах
        /// </summary>
        /// <returns></returns>
        public List<SportCategory> GetAllSportCategories()
        {
            List<SportCategory> result = new List<SportCategory>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_SPORT_CATEGORY;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new SportCategory(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех видах спорта
        /// </summary>
        /// <returns></returns>
        public List<SportType> GetAllSportTypes()
        {
            List<SportType> result = new List<SportType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_SPORT_TYPE;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new SportType(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает все опции
        /// </summary>
        /// <returns></returns>
        public List<Option> GetAllOptions()
        {
            List<Option> result = new List<Option>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_OPTIONS;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Option(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает план тренировок
        /// </summary>
        /// <returns></returns>
        public WorkoutPlan GetWorkoutPlan(long workoutPlanId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT_PLAN + " WHERE id = " + workoutPlanId.ToString();
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new WorkoutPlan(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Получает значение опции, посвященной плану тренировок
        /// </summary>
        /// <returns></returns>
        public Option GetOptionForWorkoutPlan()
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_OPTIONS + " WHERE id = 1";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Option(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Получает текущий план тренировок
        /// </summary>
        /// <returns></returns>
        public WorkoutPlan GetCurrentWorkoutPlan()
        {
            Option opt = GetOptionForWorkoutPlan();
            if (opt != null)
            {
                long workoutPlanId = Convert.ToInt64(opt.value);
                return GetWorkoutPlan(workoutPlanId);
            }
            return null;
        }

        /// <summary>
        /// Получает иформацию о всех работах
        /// </summary>
        /// <returns></returns>
        public List<Work> GetAllWorks()
        {
            List<Work> result = new List<Work>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Work(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех типах работ
        /// </summary>
        /// <returns></returns>
        public List<WorkType> GetAllWorkTypes()
        {
            List<WorkType> result = new List<WorkType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK_TYPE;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkType(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех тренировках
        /// </summary>
        /// <returns></returns>
        public List<Workout> GetAllWorkouts()
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT + " ORDER BY [ID] DESC";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Workout(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех типах тренировок
        /// </summary>
        /// <returns></returns>
        public List<WorkoutType> GetAllWorkoutTypes()
        {
            List<WorkoutType> result = new List<WorkoutType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT_TYPE;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkoutType(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех планах тренировок
        /// </summary>
        /// <returns></returns>
        public List<WorkoutPlan> GetAllWorkoutPlan()
        {
            List<WorkoutPlan> result = new List<WorkoutPlan>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT_PLAN + " ORDER BY [ID] DESC";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkoutPlan(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает иформацию о всех типах планов тренировок
        /// </summary>
        /// <returns></returns>
        public List<WorkoutPlanType> GetAllWorkoutPlanTypes()
        {
            List<WorkoutPlanType> result = new List<WorkoutPlanType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT_PLAN_TYPE + " ORDER BY [ID] DESC";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkoutPlanType(reader));
                    }
                }
            }
            return result;
        }

        #endregion

        #region[получение некоторых колонок из таблиц]
        /// <summary>
        /// Получает иформацию о всех легкоатлетических работах
        /// </summary>
        /// <returns></returns>
        public List<WorkType> GetAllWorkTypesKardio()
        {
            List<WorkType> result = new List<WorkType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK_TYPE + " WHERE sportTypeId = 1 GROUP BY value";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkType(reader));
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Получает иформацию о всех тяжелоатлетических работах
        /// </summary>
        /// <returns></returns>
        public List<WorkType> GetAllWorkTypesHardWork()
        {
            List<WorkType> result = new List<WorkType>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK_TYPE + " WHERE sportTypeId = 2 GROUP BY value";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new WorkType(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получает указанное количество последних тренировок
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetLastWorkouts(long athleteId, int count, int offset, System.Globalization.NumberFormatInfo format)
        {
            Disconnect();
            Connect();
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                "SELECT wrkt.date, wrkt.id, wt.value FROM "
                + TABLE_WORKOUT + " wrkt, "
                + TABLE_WORKOUT_TYPE + " wt "
                + "WHERE wrkt.athleteId = " + athleteId
                + " AND wt.id = wrkt.workoutTypeId "
                + " ORDER BY wrkt.id DESC LIMIT " + count
                + " OFFSET " + offset;


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
        /// Возвращает количество последних тренировок
        /// </summary>
        /// <param name="requestAfterFrom"></param>
        /// <returns></returns>
        public int GetCountLastWorkouts(long athleteId)
        {
            Disconnect();
            Connect();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                "SELECT count(*) FROM "
                + TABLE_WORKOUT + " wrkt, "
                + TABLE_WORKOUT_TYPE + " wt "
                + "WHERE wrkt.athleteId = " + athleteId
                + " AND wt.id = wrkt.workoutTypeId ";

                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// Возвращает всю работу c результатами определенной тренировки
        /// </summary>
        /// <param name="athleteId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GetWorksAndResultsOnWorkout(long workoutId, System.Globalization.NumberFormatInfo format)
        {
            string result = "";
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                "SELECT wrk_tp.value, wrk.comment, wrk.resultId FROM "
                + TABLE_WORKOUT + " wrkt, "
                + TABLE_WORK + " wrk, "
                + TABLE_WORK_TYPE + " wrk_tp "
                + "WHERE wrkt.id = " + workoutId
                + " AND wrk.workoutId = wrkt.id"
                + " AND wrk.workTypeId = wrk_tp.id"
                + " ORDER BY wrk.id ASC";

                DbDataReader reader = command.ExecuteReader();
                string comment = "";
                long resultId = 0;
                string lastWork = "";
                bool firstWork = true;
                List<float> weights = new List<float>();
                List<Int16> repeats = new List<Int16>();
                List<string> times = new List<string>();
                byte count = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        lastWork = reader.GetValue(0).ToString() == "" ? "" : reader.GetString(0);
                        if (result.IndexOf(lastWork) == -1)
                        {
                            if (weights.Count > 1 && repeats.Count > 1)
                            {
                                weights.Sort();
                                repeats.Sort();
                                float wMin = weights[0];
                                float wMax = weights[weights.Count - 1];
                                Int16 rMin = repeats[0];
                                Int16 rMax = repeats[repeats.Count - 1];

                                string w = wMin != wMax ? string.Format("{0}-{1}", wMin, wMax) : wMin.ToString();
                                string r = rMin != rMax ? string.Format("{0}-{1}", rMin, rMax) : rMin.ToString();
                                if (weights.Count != 1 && repeats.Count != 1)
                                    result += string.Format(" {0} {1}x({2}), {3}", w, count, r, lastWork);
                                else if (weights.Count == 1 && repeats.Count != 1)
                                {
                                    result += string.Format(" {0} {1}x({2}), {3}", wMin, count, r, lastWork);
                                }
                                else if (weights.Count != 1 && repeats.Count == 1)
                                {
                                    result += string.Format(" {0} {1}x({2}), {3}", w, count, rMin, lastWork);
                                }
                                else
                                {
                                    result += string.Format(" {0} {1}x({2}), {3}", wMin, count, rMin, lastWork);
                                }
                                count = 0;
                                weights.Clear();
                                repeats.Clear();
                            }
                            else
                            {
                                if (times.Count != 0)
                                {
                                    string t = " (";
                                    foreach (string s in times)
                                        t += s + ", ";
                                    t = t.Substring(0, t.Length - 2);
                                    t += "), ";
                                    result += result != "" ? t + lastWork : lastWork;
                                }
                                else
                                    result += result != "" ? "," + lastWork : lastWork;

                            }
                            firstWork = true;
                        }
                        else
                            firstWork = false;
                        comment = reader.GetValue(1).ToString() == "" ? "" : reader.GetString(1);
                        object o = reader.GetValue(2).ToString();
                        resultId = reader.GetValue(2).ToString() == "" ? 0 : reader.GetInt64(2);
                        Result res = GetResult(resultId, format);
                        if (res != null)
                        {
                            if (res.time != Result.defaultTime)
                            {
                                times.Add(res.time);
                            }
                            else if (res.weight != 0 && res.repeat != 0)
                            {
                                weights.Add(res.weight);
                                repeats.Add(res.repeat);
                                count++;
                            }
                        }
                    }
                    if (times.Count != 0)
                    {
                        string t = " (";
                        foreach (string s in times)
                            t += s + ", ";
                        t = t.Substring(0, t.Length - 2);
                        t += "), ";
                        result += t;
                    }
                    result = result.IndexOf(",") != -1 ? result.Substring(0, result.Length - 2) : result;//удаляем последнюю запятую
                }
            }
            return result;
        }
        /// <summary>
        /// Возвращает тип работы 
        /// </summary>
        /// <param name="workTypeId"></param>
        /// <returns></returns>
        public WorkType GetWorkType(long workTypeId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK_TYPE + " WHERE id = " + workTypeId;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new WorkType(reader);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Возвращает результат по id
        /// </summary>
        /// <returns></returns>
        public Result GetResult(long resultId, System.Globalization.NumberFormatInfo format)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_RESULT + " WHERE id = " + resultId;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Result(reader, format);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Возвращает результат по id
        /// </summary>
        /// <returns></returns>
        public List<Result> GetResults(long resultId, long workTypeId, System.Globalization.NumberFormatInfo format)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                List<Result> result = new List<Result>();
                command.CommandText = "SELECT * FROM " + TABLE_RESULT + " r, "
                    + TABLE_WORK + " w, "
                    + TABLE_WORK_TYPE + " wt"
                    + " WHERE r.id = " + resultId
                    + " AND w.resultId = r.id "
                    + " AND w.workTypeId = wt.id "
                    + " AND wt.id = " + workTypeId
                    + " ORDER BY r.id";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Result(reader, format));
                    }
                }
                return result;
            }
            //return null;
        }
        /// <summary>
        /// Возвращает работу по id
        /// </summary>
        /// <returns></returns>
        public Work GetWork(long workId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK + " WHERE id = " + workId;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Work(reader);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Возвращает список работ по id тренировки
        /// </summary>
        /// <returns></returns>
        public List<Work> GetWorks(long workoutId)
        {
            List<Work> works = new List<Work>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORK + " WHERE workoutId = " + workoutId;
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        works.Add(new Work(reader));
                    }
                }
            }
            return works;
        }
        /// <summary>
        /// Возвращает тренировку по id
        /// </summary>
        /// <returns></returns>
        public Workout GetWorkout(long workoutId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_WORKOUT + " w, "
                    + TABLE_WORKOUT_TYPE + " wt "
                    + " WHERE wt.id <> 4 "
                    + " AND id = " + workoutId
                    + " AND w.workoutTypeId = wt.id";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Workout(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает тренировки по дате 
        /// </summary>
        /// <returns></returns>
        public List<Workout> GetWorkouts(DateTime workoutDate)
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_WORKOUT + " w, "
                    + TABLE_WORKOUT_TYPE + " wt "
                    + " WHERE wt.id <> 4 "
                    + " AND w.workoutTypeId = wt.id"
                    + " AND w.date = " + OtherMethods.GetDate(string.Format("{0}.{1}.{2}", workoutDate.Day, workoutDate.Month, workoutDate.Year)).ToString();
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Workout(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает тренировки из плана тренировок 
        /// </summary>
        /// <returns></returns>
        public List<Workout> GetWorkouts(long workoutPlanId, short workoutPlanTypeId, short musclesGroupId)
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_WORKOUT + " w, "
                    + TABLE_WORKOUT_TYPE + " wt "
                    + " WHERE wt.id <> 4 "
                    + " AND w.workoutTypeId = wt.id"
                    + " AND w.workoutPlanId = '" + workoutPlanId + "'"
                    + " AND w.workoutPlanTypeId = '" + workoutPlanTypeId + "'"
                    + " AND w.musclesGroupId = '" + musclesGroupId + "'";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Workout(reader));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Возвращает тренировки из плана тренировок 
        /// </summary>
        /// <returns></returns>
        public List<Workout> GetWorkoutsFromWorkoutPlan(long workoutPlanId, short workoutPlanTypeId, short musclesGroupId)
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_WORKOUT + " w, "
                    + TABLE_WORKOUT_TYPE + " wt "
                    + " WHERE wt.id = 4 "
                    + " AND w.workoutTypeId = wt.id"
                    + " AND w.workoutPlanId = '" + workoutPlanId + "'"
                    + " AND w.workoutPlanTypeId = '" + workoutPlanTypeId + "'"
                    + " AND w.musclesGroupId = '" + musclesGroupId + "'";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new Workout(reader));
                    }
                }
            }
            return result;
        }
        #endregion

        #region[получение id по value]
        /// <summary>
        /// Возвращает id по value из указанного справочника
        /// </summary>
        /// <param name="tableName">Имя таблицы-справочника</param>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public long GetIdByValue(string tableName, string value)
        {
            if (value == "")
                return 0;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM " + tableName + " WHERE value = '" + value.Trim() + "';";
                //параметризованным запросом сделать

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Возвращает план по наименованию
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public WorkoutPlan GetWorkoutPlan(string title)
        {
            if (title == "")
                return null;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + SQLite.TABLE_WORKOUT_PLAN + " WHERE title = '" + title.Trim() + "';";
                //параметризованным запросом сделать

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new WorkoutPlan(reader);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Возвращает идентификатор плана по наименованию
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public long GetWorkoutPlanId(string title)
        {
            if (title == "")
                return 0;
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM " + SQLite.TABLE_WORKOUT_PLAN + " WHERE title = '" + title.Trim() + "';";
                //параметризованным запросом сделать

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Возвращает значение по id из справочника
        /// </summary>
        /// <param name="tableName">Имя таблицы-справочника</param>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public string GetValueById(string tableName, long id)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT value FROM " + tableName + " WHERE id = " + id.ToString() + ";";
                //параметризованным запросом сделать

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetValue(0).ToString();
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// Возвращает id пользователя по всей его информации
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public long GetIdByUserInfo(User user)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT id FROM {0} WHERE name = '{1}' AND lastName = '{2}' AND fatherName = '{3}' AND phone = '{4}' AND birthday = {5} AND sex = '{6}' AND password = '{7}' AND createDate = {8};",
                    TABLE_USER, user.name, user.lastName, user.fatherName, user.phone, user.birthDayInt64, user.sex, user.password, user.createDateInt64);
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// Возвращает идентификатор пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public long GetUserId(string login, string password)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM " + TABLE_USER + " WHERE login = @login AND password = @password;";

                command.Parameters.Add("@login", System.Data.DbType.String);
                command.Parameters.Add("@password", System.Data.DbType.String);

                command.Parameters["@login"].Value = login;
                command.Parameters["@password"].Value = password;

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Возвращает идентификатор пользователя с таким логином
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public long GetUserByLogin(string login)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT id FROM " + TABLE_USER + " WHERE login = @login;";

                command.Parameters.Add("@login", System.Data.DbType.String);

                command.Parameters["@login"].Value = login;

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }


        /// <summary>
        /// Возвращает идентификатор пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public User GetUser(long userId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_USER + " WHERE id = @id";
                command.Parameters.Add("@id", System.Data.DbType.Int64);
                command.Parameters["@id"].Value = userId;

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new User(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает идентификатор последней тренировки данного спортсмена
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Workout GetLastWorkout(long athleteId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT * FROM {0} WHERE athleteId = {1} ORDER BY id DESC LIMIT 1;",
                    TABLE_WORKOUT, athleteId);

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return new Workout(reader);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает идентификатор последней тренировки данного спортсмена
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public long GetLastResultId()
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format("SELECT r.id FROM {0} r ORDER BY r.id DESC LIMIT 1;",
                    TABLE_RESULT);

                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt64(0);
                    }
                }
            }
            return -1;
        }

        #endregion

        /// <summary>
        /// Возвращает количество последних тренировок
        /// </summary>
        /// <param name="requestAfterFrom"></param>
        /// <returns></returns>
        public int GetCountSearchedWorkouts(string request, string workoutType, bool kardio, bool hardwork, string musclesGroup, long athleteId)
        {
            Disconnect();
            Connect();
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
                command.CommandText = "SELECT DISTINCT count(*) FROM "
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
                                        + workTypeScript;


                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Возвращает время начала тренировки по умолчанию
        /// </summary>
        /// <returns></returns>
        public string GetDefaultWorkoutsStartDate()
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_OPTIONS + " op "
                    + " WHERE op.id == '" + OPTION_DEFAULT_START_TIME_ID + "'";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Option op = new Option(reader);
                        if (op != null)
                            return op.value;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Возвращает время окончания тренировки по умолчанию
        /// </summary>
        /// <returns></returns>
        public string GetDefaultWorkoutsEndDate()
        {
            List<Workout> result = new List<Workout>();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM "
                    + TABLE_OPTIONS + " op "
                    + " WHERE op.id == '" + OPTION_DEFAULT_END_TIME_ID + "'";
                DbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Option op = new Option(reader);
                        if (op != null)
                            return op.value;
                    }
                }
            }
            return "";
        }
    }
}
