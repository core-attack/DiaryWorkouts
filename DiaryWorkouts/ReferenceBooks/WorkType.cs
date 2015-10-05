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
    /// Тип работы 
    /// </summary>
    class WorkType
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        Int16 _id;

        /// <summary>
        /// Записавший пользователь
        /// </summary>
        long _userId;

        /// <summary>
        /// Количество данных работ в тренировках
        /// </summary>
        int _count;

        /// <summary>
        /// Значение 
        /// </summary>
        string _value;

        /// <summary>
        /// Идентификатор вида спорта
        /// </summary>
        byte _sportTypeId;

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
        /// Записавший пользователь
        /// </summary>
        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// Количество данных работ в тренировках
        /// </summary>
        public int count
        {
            get { return _count; }
            set { _count = value; }
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
        /// Идентификатор вида спорта
        /// </summary>
        public byte sportTypeId
        {
            get { return _sportTypeId; }
            set { _sportTypeId = value; }
        }

        /// <summary>
        /// Конструктор класса WorkType
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="value">Значение</param>
        public WorkType(byte sportType, string value, long userId)
        {
            this._sportTypeId = sportType;
            this._value = value;
            this._userId = userId;
        }

        /// <summary>
        /// Конструктор класса WorkType
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="value">Значение</param>
        public WorkType(Int16 id, byte sportType, string value, long userId)
        {
            this._id = id;
            this._sportTypeId = sportType;
            this._value = value;
            this._userId = userId;
        }

        /// <summary>
        /// Конструктор класса WorkType
        /// </summary>
        public WorkType(DbDataReader reader)
        {
            this._id = reader.GetInt16(0);
            this._sportTypeId = reader.GetByte(1);
            this._userId = reader.GetInt64(2);
            this._count = reader.GetInt32(3);
            this._value = reader.GetString(4);
        }

        public override string ToString()
        {
            return string.Format("id={0}, sportTypeId={1} value={2}", id, sportTypeId, value);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [sportTypeId], [userId], [count], [value]) VALUES ({1}, {2}, {3}, {4}, '{5}');\" +",
                    SQLite.TABLE_WORK_TYPE,
                    _id,
                    _sportTypeId,
                    _userId,
                    _count,
                    _value
                    );
        }
    }
}
