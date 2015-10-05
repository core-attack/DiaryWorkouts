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
    /// Вид спорта
    /// </summary>
    class SportType
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        byte _id;

        /// <summary>
        /// Значение 
        /// </summary>
        string _value;

        #endregion

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public byte id
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
        /// Конструктор класса SportType
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="value">Значение</param>
        public SportType(byte id, string value)
        {
            this.id = id;
            this.value = value;
        }

        /// <summary>
        /// Конструктор класса SportType
        /// </summary>
        public SportType(DbDataReader reader)
        {
            this._id = reader.GetByte(0);
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
                    "\"INSERT INTO {0} ([id], [value]) VALUES ({1}, '{2}'); \" +",
                    SQLite.TABLE_SPORT_TYPE,
                    _id,
                    _value
                    );
        }
    }
}
