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
    /// Абстрактный класс пользователя
    /// </summary>
    class User
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Фамилия 
        /// </summary>
        string _name;

        /// <summary>
        /// Имя
        /// </summary>
        string _lastName;

        /// <summary>
        /// Отчество
        /// </summary>
        string _fatherName;

        /// <summary>
        /// Логин пользователя
        /// </summary>
        string _login;

        /// <summary>
        /// Зашифрованный пароль
        /// </summary>
        string _password;

        /// <summary>
        /// Контактный телефон
        /// </summary>
        string _phone;

        /// <summary>
        /// День рождения
        /// </summary>
        long _bDay;

        /// <summary>
        /// Пол
        /// </summary>
        string _sex;

        /// <summary>
        /// Пользовательская роль
        /// </summary>
        Int16 _userRoleId;

        /// <summary>
        /// Дата создания записи в БД
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
        /// Фамилия 
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string lastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string fatherName
        {
            get { return _fatherName; }
            set { _fatherName = value; }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public string login
        {
            get { return _login; }
            set { _login = value; }
        }

        /// <summary>
        /// Зашифрованный пароль
        /// </summary>
        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        /// <summary>
        /// Пол
        /// </summary>
        public string sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        /// <summary>
        /// Пользовательская роль
        /// </summary>
        public Int16 userRole
        {
            get { return _userRoleId; }
            set { _userRoleId = value; }
        }

        /// <summary>
        /// Дата создания записи в БД
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
        /// Дата создания записи в БД
        /// </summary>
        public string bDay
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(_bDay).ToLocalTime();
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
                        _bDay = dt.Ticks / 10000000;
                    }
                    catch
                    {
                        _bDay = 0;
                    }
                }
                else
                {
                    _bDay = 0;
                }
            }
        }

        /// <summary>
        /// Возвращает дату рождения в виде числа
        /// </summary>
        public long birthDayInt64
        {
            get
            {
                return _bDay;
            }
        }

        /// <summary>
        /// Конструктор класса User
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Фамилия</param>
        /// <param name="lastName">Имя</param>
        /// <param name="fatherName">Отчество</param>
        /// <param name="password">Пароль</param>
        /// <param name="phone">Контактный телефон</param>
        /// <param name="sex">Пол</param>
        /// <param name="createDate">Дата создания записи спортсмена в БД</param>
        public User(long id, string name, string lastName, string fatherName, string login, string password, string phone, string sex, string createDate, string bDay)
        {
            this._id = id;
            this._name = name;
            this._lastName = lastName;
            this._fatherName = fatherName;
            this._login = login;
            this._password = password;
            this._phone = phone;
            this._sex = sex;
            this.createDate = createDate;
            this.bDay = bDay;
        }

        public User(DbDataReader record)
        {
            this._id = record.GetInt64(0);
            this._name = record.GetValue(1).ToString() == "" ? "" : record.GetString(1);
            this._lastName = record.GetValue(2).ToString() == "" ? "" : record.GetString(2);
            this._fatherName = record.GetValue(3).ToString() == "" ? "" : record.GetString(3);
            this._phone = record.GetValue(4).ToString() == "" ? "" : record.GetString(4);
            this._bDay = record.GetValue(5).ToString() == "" ? 0 : record.GetInt64(5);
            this._sex = record.GetValue(6).ToString() == "" ? "" : record.GetString(6);
            this._login = record.GetValue(7).ToString() == "" ? "" : record.GetString(7);
            this._password = record.GetValue(8).ToString() == "" ? "" : record.GetString(8);
            this._createDate = record.GetValue(9).ToString() == "" ? 0 : record.GetInt64(9);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [name], [lastName], [fatherName], [phone], [bDay], [sex], [login], [password], [createDate], [userRole]) VALUES ({1}, '{2}', '{3}', '{4}', '{5}', {6}, '{7}', '{8}', '{9}', {10}, {11});\" + ",
                    SQLite.TABLE_USER,
                    _id,
                    _name,
                    _lastName,
                    _fatherName,
                    _phone,
                    _bDay,
                    _sex,
                    _login,
                    _password,
                    _createDate,
                    _userRoleId
                    );    
        }
    }
}
