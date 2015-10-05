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
    /// Спортсмен
    /// </summary>
    class Athlete
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Идентификатор вида спорта
        /// </summary>
        byte _sportTypeId;

        /// <summary>
        /// Идентификатор спортивного разряда
        /// </summary>
        byte _sportCategoryId;

        /// <summary>
        /// Адрес
        /// </summary>
        string _address;

        /// <summary>
        /// Место учебы/работы
        /// </summary>
        string _place;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long _userId;

        /// <summary>
        /// Дата создания записи
        /// </summary>
        long _createDate;

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
        /// Идентификатор вида спорта
        /// </summary>
        public byte sportTypeId
        {
            get { return _sportTypeId; }
            set { _sportTypeId = value; }
        }

        /// <summary>
        /// Идентификатор спортивного разряда
        /// </summary>
        public byte sportCategoryId
        {
            get { return _sportCategoryId; }
            set { _sportCategoryId = value; }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Место учебы/работы
        /// </summary>
        public string place
        {
            get { return _place; }
            set { _place = value; }
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        /// Дата создания записи
        /// </summary>
        public string createDate
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(_createDate).ToLocalTime();
                return dt.Day + "." + dt.Month + "." + dt.Year;
            }
            set
            {
                string[] values = value.Split(new char[] { '.' });
                if (values.Length == 3)
                {
                    try
                    {
                        System.DateTime dt = new DateTime(
                            Convert.ToInt32(values[2]),
                            Convert.ToInt32(values[1]),
                            Convert.ToInt32(values[0]),
                            0, 0, 0, 0, System.DateTimeKind.Utc);
                        _createDate = dt.Ticks / 10000000;
                    }
                    catch
                    {
                        _createDate = 0;
                    }
                }
                else
                {
                    _createDate = 0;
                }
            }
        }

        /// <summary>
        /// Возвращает дату создания записи в виде числа
        /// </summary>
        public long createDateInt64
        {
            get
            {
                return _createDate;
            }
        }

        /// <summary>
        /// Конструктор класса Athlete
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Фамилия</param>
        /// <param name="lastName">Имя</param>
        /// <param name="fatherName">Отчество</param>
        /// <param name="password">Пароль</param>
        /// <param name="phone">Контактный телефон</param>
        /// <param name="sex">Пол</param>
        /// <param name="createDate">Дата создания записи спортсмена в БД</param>
        /// <param name="sportTypeId">Идентификатор вида спорта</param>
        /// <param name="sportCategory">Идентификатор спортивного разряда</param>
        /// <param name="address">Адрес</param>
        /// <param name="place">Место учебы/работы</param>
        public Athlete(long id, byte sportTypeId, byte sportCategory, string address, string place, long userId, string createDate)
        {
            this._id = id;
            this._sportTypeId = sportTypeId;
            this._sportCategoryId = sportCategory;
            this._address = address;
            this._place = place;
            this._userId = userId;
            this.createDate = createDate;
        }

        /// <summary>
        /// Конструктор класса Athlete
        /// </summary>
        /// <param name="record"></param>
        public Athlete(DbDataReader record)
        {
            this._id = record.GetInt64(0);
            this._sportTypeId = record.GetValue(1).ToString() == "" ? (byte)0 : record.GetByte(1);
            this._sportCategoryId = record.GetValue(2).ToString() == "" ? (byte)0 : record.GetByte(2);
            this._address = record.GetValue(3).ToString() == "" ? "" : record.GetString(3);
            this._place = record.GetValue(4).ToString() == "" ? "" : record.GetString(4);
            this._userId = record.GetValue(5).ToString() == "" ? 0 : record.GetInt64(5);
            this._createDate = record.GetValue(6).ToString() == "" ? 0 : record.GetInt64(6);
        }

        public override string ToString()
        {
            return string.Format("id={0} sportTypeId={1} sportCategoryId={2} address={3} place={4} userId={5} createDate={6}", id, sportTypeId, sportCategoryId, address, place, userId, createDate);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [sportTypeId], [sportCategoryId], [address], [place], [userId], [createDate]) VALUES ({1}, {2}, {3}, '{4}', '{5}', {6}, {7}); \" + ",
                    SQLite.TABLE_ATHLETE,
                    _id,
                    _sportTypeId,
                    _sportCategoryId,
                    _address,
                    _place,
                    _userId,
                    _createDate
                    );
        }
    }
}
