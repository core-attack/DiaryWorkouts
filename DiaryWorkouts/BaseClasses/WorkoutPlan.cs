using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using DiaryWorkouts.DataBase;

namespace DiaryWorkouts.BaseClasses
{
    class WorkoutPlan
    {
        #region [Скрытые поля]

        /// <summary>
        /// Идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Описание плана
        /// </summary>
        string _title;

        /// <summary>
        /// Период повторения в днях
        /// </summary>
        byte _period;

        /// <summary>
        /// Идентификатор создателя
        /// </summary>
        long _creatorId;

        #endregion

        /// <summary>
        /// Идентификатор плана тренировки
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        /// <summary>
        /// Наименование плана тренировки
        /// </summary>
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        
        /// <summary>
        /// Период плана тренировки в днях (будет повторяться через данное количество дней)
        /// </summary>
        public byte period
        {
            get { return _period; }
            set { _period = value; }
        }
        
        /// <summary>
        /// Идентификатор плана тренировки
        /// </summary>
        public long creatorId
        {
            get { return _creatorId; }
            set { _creatorId = value; }
        }

        public WorkoutPlan(DbDataReader reader)
        {
            _id = reader.GetInt64(0);
            _title = reader.GetValue(1).ToString() == "" ? "" : reader.GetString(1);
            _period = reader.GetValue(2).ToString() == "" ? (byte)0 : reader.GetByte(2);
            _creatorId = reader.GetValue(3).ToString() == "" ? 0 : reader.GetInt64(3);
        }

        public WorkoutPlan(string title, byte period, long creatorId)
        {
            _title = title;
            _period = period;
            _creatorId = creatorId;
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [title], [period], [creatorId]) VALUES ({1}, '{2}', '{3}', {4}); \" +",
                    SQLite.TABLE_WORKOUT_PLAN,
                    _id,
                    _title,
                    _period,
                    _creatorId
                    );
        }
    }
}
