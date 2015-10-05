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
    /// Настройка программы 
    /// </summary>
    class Option
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        byte _id;

        /// <summary>
        /// Наименование опции
        /// </summary>
        string _name;

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
        /// Записавший пользователь
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
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
        /// Конструктор класса Option
        /// </summary>
        public Option(string name, string value)
        {
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Конструктор класса Option
        /// </summary>
        public Option(byte id, string name, string value)
        {
            this._id = id;
            this._name = name;
            this._value = value;
        }

        /// <summary>
        /// Конструктор класса Option
        /// </summary>
        public Option(DbDataReader reader)
        {
            this._id = reader.GetByte(0);
            this._name = reader.GetString(1);
            this._value = reader.GetString(2);
        }

        public override string ToString()
        {
            return string.Format("id={0}, name={1} value={2}", id, name, value);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [name], [value]) VALUES ({1}, '{2}', '{3}');\" +",
                    SQLite.TABLE_OPTIONS,
                    _id,
                    _name,
                    _value
                    );
        }
    }
}
