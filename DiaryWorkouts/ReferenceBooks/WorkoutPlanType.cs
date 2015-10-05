using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using DiaryWorkouts.DataBase;

namespace DiaryWorkouts.ReferenceBooks
{
    /// <summary>
    /// Тип тренировки
    /// </summary>
    class WorkoutPlanType
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        Int16 _id;

        /// <summary>
        /// Значение 
        /// </summary>
        string _value;

        #endregion

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public Int16 id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Значение 
        /// </summary>
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Конструктор класса WorkoutPlanType
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="value">Значение</param>
        public WorkoutPlanType(Int16 id, string value)
        {
            this.id = id;
            this.value = value;
        }

        /// <summary>
        /// Конструктор класса WorkoutPlanType
        /// </summary>
        public WorkoutPlanType(DbDataReader reader)
        {
            this._id = reader.GetInt16(0);
            this._value = reader.GetString(1);
        }

        public override string ToString()
        {
            return string.Format("id={0}, value={1}", id, value);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [value]) VALUES ({1}, '{2}');\" + ",
                    SQLite.TABLE_WORKOUT_PLAN_TYPE,
                    _id,
                    _value
                    );
        }
    }
}
