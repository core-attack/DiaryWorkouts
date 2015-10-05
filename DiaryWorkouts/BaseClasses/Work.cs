using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using DiaryWorkouts.DataBase;

namespace DiaryWorkouts.BaseClasses
{
    /// <summary>
    /// Работа
    /// </summary>
    class Work
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Идентификатор результата
        /// </summary>
        long _resultId;

        /// <summary>
        /// Идентификатор типа работы
        /// </summary>
        long _workTypeId;

        /// <summary>
        /// Идентификатор тренировки
        /// </summary>
        long _workoutId;

        /// <summary>
        /// Комментарий
        /// </summary>
        string _comment;

        #endregion

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Идентификатор результата
        /// </summary>
        public long resultId
        {
            get { return _resultId; }
            set { _resultId = value; }
        }

        /// <summary>
        /// Идентификатор типа работы
        /// </summary>
        public long workTypeId
        {
            get { return _workTypeId; }
            set { _workTypeId = value; }
        }

        /// <summary>
        /// Идентификатор тренировки
        /// </summary>
        public long workoutId
        {
            get { return _workoutId; }
            set { _workoutId = value; }
        }

        /// <summary>
        /// Комментарий 
        /// </summary>
        public string comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        /// Конструктор класса Work
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="resultId">Идентификатор результата</param>
        /// <param name="workTypeId">Идентификатор типа работы</param>
        /// <param name="comment">Комментарий</param>
        public Work(long resultId, long workoutId, long workTypeId, string comment)
        {
            this.resultId = resultId;
            this.workoutId = workoutId;
            this.workTypeId = workTypeId;
            this.comment = comment;
        }

        /// <summary>
        /// Конструктор класса Work
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="resultId">Идентификатор результата</param>
        /// <param name="workTypeId">Идентификатор типа работы</param>
        /// <param name="comment">Комментарий</param>
        public Work(long id, long resultId, long workoutId, long workTypeId, string comment)
        {
            this.id = id;
            this.resultId = resultId;
            this.workoutId = workoutId;
            this.workTypeId = workTypeId;
            this.comment = comment;
        }

        /// <summary>
        /// Конструктор класса Work
        /// </summary>
        public Work(DbDataReader reader)
        {
            this.id = reader.GetInt64(0);
            this.resultId = reader.GetValue(1).ToString() == "" ? 0 : reader.GetInt64(1);
            this.workoutId = reader.GetValue(2).ToString() == "" ? 0 : reader.GetInt64(2);
            this.workTypeId = reader.GetValue(3).ToString() == "" ? 0 : reader.GetInt64(3);
            this.comment = reader.GetValue(4).ToString() == "" ? "" : reader.GetString(4);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [resultId], [workoutId], [workTypeId], [comment]) VALUES ({1}, {2}, {3}, {4}, '{5}'); \" +",
                    SQLite.TABLE_WORK,
                    _id,
                    _resultId,
                    _workoutId,
                    _workTypeId,
                    _comment
                    );
        }
    }
}
