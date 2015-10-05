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
    /// Спортивный разряд/категория
    /// </summary>
    class SportCategory
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

        /// <summary>
        /// Сокращение
        /// </summary>
        string _shortValue;

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
        /// Сокращение 
        /// </summary>
        public string shortValue
        {
            get { return _shortValue; }
            set { _shortValue = value; }
        }

        /// <summary>
        /// Конструктор класса SportCategory
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="value">Значение</param>
        public SportCategory(byte id, string value, string shortValue)
        {
            this.id = id;
            this.value = value;
            this.shortValue = shortValue;
        }

        /// <summary>
        /// Конструктор класса SportCategory
        /// </summary>
        public SportCategory(DbDataReader reader)
        {
            this._id = reader.GetByte(0);
            this._value = reader.GetString(1);
            this._shortValue = reader.GetString(2);
        }

        public override string ToString()
        {
            return string.Format("id={0}, value={1} shortValue={2}", id, value, shortValue);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [value], [shortValue]) VALUES ({1}, '{2}', '{3}'); \" +",
                    SQLite.TABLE_SPORT_CATEGORY,
                    _id,
                    _value,
                    _shortValue
                    );
        }
    }
}
