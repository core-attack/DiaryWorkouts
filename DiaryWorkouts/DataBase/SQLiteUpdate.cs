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
        public void UpdateOptionWorkoutStartTime(string value)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("value", value);
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_OPTIONS, OPTION_DEFAULT_START_TIME_ID, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи пользовательской роли!");
            }
        }

        public void UpdateOptionWorkoutEndTime(string value)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("value", value);
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                bool ok = Update(cmd, TABLE_OPTIONS, OPTION_DEFAULT_END_TIME_ID, data);
                if (!ok)
                    throw new Exception("Не удалось обновить поля записи пользовательской роли!");
            }
        }
    }
}
