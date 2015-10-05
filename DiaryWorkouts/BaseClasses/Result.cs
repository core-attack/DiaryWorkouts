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
    /// Результат работы
    /// </summary>
    class Result
    {
        /// <summary>
        /// Маска представления результата времени
        /// </summary>
        public static string timeMask = "{0}:{1}:{2}.{3}";

        /// <summary>
        /// Результат времени по умолчанию
        /// </summary>
        public static string defaultTime = string.Format(timeMask, "00", "00", "00", "00");

        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Время выполнения (в т.ч. дистанции)
        /// </summary>
        long _time = 0;

        /// <summary>
        /// Вес
        /// </summary>
        float _weight = 0;

        /// <summary>
        /// Количество повторений
        /// </summary>
        Int16 _repeat = 0;

        /// <summary>
        /// Дальность в метрах
        /// </summary>
        float _distance = 0;

        /// <summary>
        /// Количество очков
        /// </summary>
        int _points = 0;

        /// <summary>
        /// Место среди всех участвующих в соревновании
        /// </summary>
        int _place = 0;

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
        /// Время выполнения (в т.ч. дистанции)
        /// </summary>
        public string time
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddMilliseconds(_time);
                return string.Format(timeMask, 
                    OtherMethods.IntToBinaryString(dt.Hour), 
                    OtherMethods.IntToBinaryString(dt.Minute), 
                    OtherMethods.IntToBinaryString(dt.Second), 
                    OtherMethods.IntToBinaryString(dt.Millisecond));
            }
            set
            {
                //"00:00:00.00"
                string[] values = value.Split(new char[] { '.', ':' });
                if (values.Length == 4)
                {
                    try
                    {
                        TimeSpan ts = new TimeSpan(0, 
                            Convert.ToInt32(values[0]), 
                            Convert.ToInt32(values[1]), 
                            Convert.ToInt32(values[2]), 
                            Convert.ToInt32(values[3] 
                            ));
                        _time = ts.Ticks / 10000;
                        string ss = time;
                    }
                    catch
                    {
                        _time = 0;
                    }
                }
                else
                {
                    _time = 0;
                }
            }
        }

        public long timeInt64
        {
            get
            {
                return _time;
            }
        }

        /// <summary>
        /// Вес
        /// </summary>
        public float weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /// <summary>
        /// Количество повторений
        /// </summary>
        public Int16 repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        /// <summary>
        /// Дальность
        /// </summary>
        public float distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// Количество очков
        /// </summary>
        public int points
        {
            get { return _points; }
            set { _points = value; }
        }

        /// <summary>
        /// Место среди всех участвующих в соревновании
        /// </summary>
        public int place
        {
            get { return _place; }
            set { _place = value; }
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
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        public Result(long id, string time)
        {
            this._id = id;
            this.time = time;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        /// <param name="comment">Комментарий</param>
        public Result(long id, string time, string comment)
        {
            this._id = id;
            this.time = time;
            this.comment = comment;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        public Result(long id, float weigth, byte repeat)
        {
            this._id = id;
            this._weight = weight;
            this._repeat = repeat;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="comment">Комментарий</param>
        public Result(long id, float weigth, byte repeat, string comment)
        {
            this._id = id;
            this._weight = weight;
            this._repeat = repeat;
            this.comment = comment;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        public Result(long id, string time, float weigth, byte repeat)
        {
            this._id = id;
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        public Result(long id, string time, float weigth, byte repeat, float distance)
        {
            this._id = id;
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        /// <param name="points">Количество очков</param>
        public Result(long id, string time, float weigth, byte repeat, float distance, int points)
        {
            this._id = id;
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
            this._points = points;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        /// <param name="points">Количество очков</param>
        /// <param name="place">Место среди всех участвующих в соревновании</param>
        public Result(long id, string time, float weigth, byte repeat, float distance, int points, int place)
        {
            this._id = id;
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
            this._points = points;
            this._place = place;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="id">Идентификатор результата</param>
        /// <param name="time">Время выполнения</param>
        public Result(string time)
        {
            this.time = time;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        public Result(float weight, byte repeat)
        {
            this._weight = weight;
            this._repeat = repeat;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        public Result(string time, float weight, byte repeat)
        {
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        public Result(string time, float weight, byte repeat, float distance)
        {
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        /// <param name="points">Количество очков</param>
        public Result(string time, float weight, byte repeat, float distance, int points)
        {
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
            this._points = points;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        /// <param name="time">Время выполнения</param>
        /// <param name="weigth">Вес</param>
        /// <param name="count">Количество повторений</param>
        /// <param name="distance">Дистанция в метрах</param>
        /// <param name="points">Количество очков</param>
        /// <param name="place">Место среди всех участвующих в соревновании</param>
        public Result(string time, float weight, byte repeat, float distance, int points, int place)
        {
            this.time = time;
            this._weight = weight;
            this._repeat = repeat;
            this._distance = distance;
            this._points = points;
            this._place = place;
        }

        /// <summary>
        /// Конструктор класса результата работы
        /// </summary>
        public Result(DbDataReader reader, System.Globalization.NumberFormatInfo format)
        {
            this._id = reader.GetInt64(0);
            if (this._id > 11967)
                this._time = reader.GetValue(1).ToString() == "" ? (long)0 : (long)reader.GetValue(1);
            this._time = reader.GetValue(1).ToString() == "" ? (long)0 : (long)reader.GetValue(1);
            this._weight = reader.GetValue(2).ToString() == "" ? (float)0 : float.Parse(reader.GetValue(2).ToString(), format);
            object o = reader.GetValue(3);
            this._repeat = reader.GetValue(3).ToString() == "" ? (Int16)0 : reader.GetInt16(3);
            this._distance = reader.GetValue(4).ToString() == "" ? (float)0 : float.Parse(reader.GetValue(4).ToString(), format);
            this._points = reader.GetValue(5).ToString() == "" ? 0 : reader.GetInt32(5);
            this._place = reader.GetValue(6).ToString() == "" ? 0 : reader.GetInt32(6);
            this._comment = reader.GetValue(7).ToString() == "" ? "" : reader.GetString(7);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [time], [weight], [repeat], [distance], [points], [place], [comment]) VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, '{8}'); \" +",
                    SQLite.TABLE_RESULT,
                    _id,
                    _time,
                    _weight,
                    _repeat,
                    _distance,
                    _points,
                    _place,
                    _comment
                    );
        }
    }
}
