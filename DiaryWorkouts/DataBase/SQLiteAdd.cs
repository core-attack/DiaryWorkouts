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
        #region[Добавление записей в БД]
        /// <summary>
        /// Добавляет указанного пользователя в базу данных
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public void AddUser(User user)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("name", user.name);
                data.Add("lastName", user.lastName);
                data.Add("fatherName", user.fatherName);
                data.Add("phone", user.phone);
                data.Add("birthday", user.birthDayInt64.ToString());
                data.Add("sex", user.sex);
                data.Add("password", user.password);
                data.Add("createDate", user.createDateInt64.ToString());

                bool ok = Insert(cmd, TABLE_USER, data);

                if (!ok)
                    throw new Exception("Не удалось добавить  пользователя!");
            }
        }

        /// <summary>
        /// Удаляет пользователя по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_USER, id);

                if (!ok)
                    throw new Exception("Не удалось удалить пользователя!");
            }
        }

        /// <summary>
        /// Добавляет указанного спортсмена в базу данных
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        public void AddAthlete(Athlete athlete)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("sportTypeId", athlete.sportTypeId.ToString());
                data.Add("sportCategoryId", athlete.sportCategoryId.ToString());
                data.Add("address", athlete.address);
                data.Add("place", athlete.place);
                data.Add("userId", athlete.userId.ToString());
                data.Add("createDate", athlete.createDateInt64.ToString());

                bool ok = Insert(cmd, TABLE_ATHLETE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить спортсмена!");
            }
        }

        /// <summary>
        /// Удаляет спортсмена по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAthlete(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_ATHLETE, id);

                if (!ok)
                    throw new Exception("Не удалось удалить спортсмена!");
            }
        }
        /// <summary>
        /// Добавление тренировки в БД
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWorkout(Workout workout)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("athleteId", workout.athleteId.ToString());
                data.Add("createDate", workout.createDateInt64.ToString());
                data.Add("date", workout.dateInt64.ToString());
                data.Add("musclesGroupId", workout.musclesGroupId.ToString());
                data.Add("timeBegin", workout.timeBeginInt64.ToString());
                data.Add("timeEnd", workout.timeEndInt64.ToString());
                data.Add("workoutPlanId", workout.workoutPlanId.ToString());
                data.Add("workoutTypeId", workout.workoutTypeId.ToString());
                data.Add("warmUp", workout.warmUp.ToString());
                data.Add("workoutPlanTypeId", workout.workoutPlanTypeId.ToString());

                bool ok = Insert(cmd, TABLE_WORKOUT, data);

                //if (!ok)
                //{
                //    cmd.Transaction.Rollback();
                //    throw new Exception("Не удалось записать тренировку!");
                //}
                //else
                //{
                //    cmd.Transaction.Commit();
                //}

                if (!ok)
                    throw new Exception("Не удалось записать тренировку!");
            }
        }
        /// <summary>
        /// Удаляет тренировку по id со всеми ссылающимися на неё работами
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorkout(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                List<Work> works = GetWorks(id);
                foreach (Work w in works)
                    DeleteWork(w.id);
                bool ok = Delete(cmd, TABLE_WORKOUT, id);

                if (!ok)
                    throw new Exception("Не удалось удалить тренировку!");
            }
        }
        /// <summary>
        /// Добавление работы в БД
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWork(Work work)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("workoutId", work.workoutId.ToString());
                data.Add("resultId", work.resultId.ToString());
                data.Add("workTypeId", work.workTypeId.ToString());
                if (work.comment != "")
                    data.Add("comment", work.comment.ToString());

                bool ok = Insert(cmd, TABLE_WORK, data);

                if (!ok)
                    throw new Exception("Не удалось записать работу!");
            }
        }
        /// <summary>
        /// Удаляет работу по id и связанный с ней результат
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWork(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Work w = GetWork(id);
                long resultId = w != null ? w.resultId : 0;
                DeleteResult(resultId);
                bool ok = Delete(cmd, TABLE_WORK, id);
                if (!ok)
                    throw new Exception("Не удалось удалить работу!");
            }
        }
        /// <summary>
        /// Добавление опции в БД
        /// </summary>
        /// <param name="athlete"></param>
        public void AddOption(Option option)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("name", option.name.ToString());
                data.Add("value", option.value.ToString());

                bool ok = Insert(cmd, TABLE_OPTIONS, data);

                if (!ok)
                    throw new Exception("Не удалось записать опцию!");
            }
        }
        /// <summary>
        /// Удаляет запись опции по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOption(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_OPTIONS, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись опции!");
            }
        }
        /// <summary>
        /// Добавление результата в БД
        /// </summary>
        /// <param name="athlete"></param>
        public void AddResult(Result result)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                if (result.time != "")
                    data.Add("time", result.timeInt64.ToString());
                if (result.distance != 0f)
                    data.Add("distance", result.distance.ToString());
                if (result.weight != 0f)
                    data.Add("weight", result.weight.ToString());
                if (result.repeat != 0)
                    data.Add("repeat", result.repeat.ToString());
                if (result.place != 0)
                    data.Add("place", result.place.ToString());
                if (result.points != 0)
                    data.Add("points", result.points.ToString());

                bool ok = Insert(cmd, TABLE_RESULT, data);

                if (!ok)
                    throw new Exception("Не удалось записать результат!");
            }
        }
        /// <summary>
        /// Удаляет результат по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteResult(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_RESULT, id);

                if (!ok)
                    throw new Exception("Не удалось удалить результат!");
            }
        }
        /// <summary>
        /// Пополнение справочника видов спорта
        /// </summary>
        /// <param name="athlete"></param>
        public void AddSportType(SportType st)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", st.value.ToString());

                bool ok = Insert(cmd, TABLE_SPORT_TYPE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник видов спорта!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике видов спорта по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSportType(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_SPORT_TYPE, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике видов спорта!");
            }
        }
        /// <summary>
        /// Пополнение справочника групп мышц
        /// </summary>
        /// <param name="athlete"></param>
        public void AddMusclesGroup(MusclesGroup mg)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", mg.value.ToString());

                bool ok = Insert(cmd, TABLE_SPORT_TYPE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник мышечных групп!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике групп мышц по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMusclesGroup(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_MUSCLES_GROUP, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике групп мышц!");
            }
        }
        /// <summary>
        /// Пополнение справочника спортивных разрядов
        /// </summary>
        /// <param name="athlete"></param>
        public void AddSportCategory(SportCategory sc)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", sc.value.ToString());
                data.Add("shortValue", sc.value.ToString());


                bool ok = Insert(cmd, TABLE_SPORT_CATEGORY, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник спортивных разрядов!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике спортивных разрядов по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSportCategory(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_SPORT_CATEGORY, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике спортивных разрядов!");
            }
        }
        /// <summary>
        /// Пополнение справочника видов работ
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWorkType(WorkType wt)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", wt.value.ToString());
                data.Add("sportTypeId", wt.sportTypeId.ToString());
                data.Add("count", wt.count.ToString());
                data.Add("userId", wt.userId.ToString());

                bool ok = Insert(cmd, TABLE_WORK_TYPE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник видов работ!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике видов работ по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorkType(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_WORK_TYPE, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике видов работ!");
            }
        }
        /// <summary>
        /// Пополнение справочника видов работ
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWorkoutType(WorkoutType wt)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", wt.value.ToString());

                bool ok = Insert(cmd, TABLE_WORK_TYPE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник типов тренировок!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике типов тренировок по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorkoutType(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_WORKOUT_TYPE, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике типов тренировок!");
            }
        }
        /// <summary>
        /// Пополнение справочника типов планов тренировок
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWorkoutPlanType(WorkoutPlanType wt)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("value", wt.value.ToString());

                bool ok = Insert(cmd, TABLE_WORKOUT_PLAN_TYPE, data);

                if (!ok)
                    throw new Exception("Не удалось добавить значение в справочник типов планов тренировок!");
            }
        }
        /// <summary>
        /// Удаляет запись в справочнике типов планов тренировок по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorkoutPlanType(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_WORKOUT_PLAN_TYPE, id);

                if (!ok)
                    throw new Exception("Не удалось удалить запись в справочнике типов планов тренировок!");
            }
        }
        /// <summary>
        /// Добавить план тренировок
        /// </summary>
        /// <param name="athlete"></param>
        public void AddWorkoutPlan(WorkoutPlan wt)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                Dictionary<String, String> data = new Dictionary<string, string>();
                data.Add("title", wt.title.ToString());
                data.Add("period", wt.period.ToString());
                data.Add("creatorId", wt.creatorId.ToString());

                bool ok = Insert(cmd, TABLE_WORKOUT_PLAN, data);

                if (!ok)
                    throw new Exception("Не удалось добавить план тренировок!");
            }
        }
        /// <summary>
        /// Удаляет план тренировок по id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorkoutPlan(long id)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Delete(cmd, TABLE_WORKOUT_PLAN, id);

                if (!ok)
                    throw new Exception("Не удалось удалить план тренировок!");
            }
        }
        /// <summary>
        /// Проверяет наличие плана тренировок с таким названием в БД
        /// </summary>
        /// <param name="id"></param>
        public bool CheckWorkoutPlanTitle(string title)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT_PLAN + " WHERE title = @title;";
                command.Parameters.Add("@title", System.Data.DbType.String);
                command.Parameters["@title"].Value = title;

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
        /// Проверяет наличие тренировки плана тренировок с такими параметрами
        /// </summary>
        /// <param name="id"></param>
        public bool IsWorkoutOfWorkoutPlanExist(long workoutPlanId, Int16 musclesGroupId, Int16 workoutPlanTypeId)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM " + TABLE_WORKOUT + " WHERE workoutPlanId = @workoutPlanId AND musclesGroupId = @musclesGroupId AND workoutPlanTypeId = @workoutPlanTypeId;";
                command.Parameters.Add("@workoutPlanId", System.Data.DbType.Int64);
                command.Parameters.Add("@musclesGroupId", System.Data.DbType.Int16);
                command.Parameters.Add("@workoutPlanTypeId", System.Data.DbType.Int16);
                command.Parameters["@workoutPlanId"].Value = workoutPlanId;
                command.Parameters["@musclesGroupId"].Value = musclesGroupId;
                command.Parameters["@workoutPlanTypeId"].Value = workoutPlanTypeId;

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
        /// Обновление полей записи атлета
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateAthlete(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_ATHLETE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи атлета!");
            }
        }

        /// <summary>
        /// Обновление полей записи поверхности тренировки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateGround(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_GROUND, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи поверхности тренировки!");
            }
        }

        /// <summary>
        /// Обновление полей записи мышечной группы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateMusclesGroup(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_MUSCLES_GROUP, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи мышечной группы!");
            }
        }

        /// <summary>
        /// Обновление полей записи результата
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateResult(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_RESULT, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи результата!");
            }
        }

        /// <summary>
        /// Обновление полей записи опции
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateOption(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_OPTIONS, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля опции!");
            }
        }

        /// <summary>
        /// Обновление полей записи спортивной категории
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateSportCategory(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_SPORT_CATEGORY, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи спортивной категории!");
            }
        }

        /// <summary>
        /// Обновление полей записи вида спорта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateSportType(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_SPORT_TYPE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи вида спорта!");
            }
        }

        /// <summary>
        /// Обновление полей записи пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateUser(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_USER, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи пользователя!");
            }
        }

        /// <summary>
        /// Обновление полей записи пользовательской роли
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateUserRole(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_USER_ROLE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи пользовательской роли!");
            }
        }

        /// <summary>
        /// Обновление полей записи работы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWork(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORK, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи работы!");
            }
        }

        /// <summary>
        /// Обновление полей записи тренировки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWorkout(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORKOUT, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи тренировки!");
            }
        }

        /// <summary>
        /// Обновление полей записи вида работы
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWorkType(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORK_TYPE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи вида работы!");
            }
        }

        /// <summary>
        /// Обновление полей записи плана тренировок
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWorkoutPlan(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORKOUT_PLAN, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи плана тренировок!");
            }
        }

        /// <summary>
        /// Обновление полей записи типа плана тренировок
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWorkoutPlanType(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORKOUT_PLAN_TYPE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи типа плана тренировок!");
            }
        }

        /// <summary>
        /// Обновление полей записи типа тренировки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void UpdateWorkoutType(long id, Dictionary<String, String> data)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_WORKOUT_TYPE, id, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи типа тренировки!");
            }
        }

        #endregion
    }
}
