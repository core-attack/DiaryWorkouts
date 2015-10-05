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
    /// Тренировка
    /// </summary>
    class Workout
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        /// <summary>
        /// Идентификатор спортсмена
        /// </summary>
        long _athleteId;

        /// <summary>
        /// Идентификатор типа тренировки
        /// </summary>
        Int16 _workoutTypeId = -1;

        /// <summary>
        /// Идентификатор группы мыщц
        /// </summary>
        Int16 _musclesGroupId = -1;

        /// <summary>
        /// Дата тренировки
        /// </summary>
        long _date;

        /// <summary>
        /// Время начала тренировки
        /// </summary>
        long _timeBegin;

        /// <summary>
        /// Время окончания тренировки
        /// </summary>
        long _timeEnd;

        /// <summary>
        /// Разминка
        /// </summary>
        byte _warmUp;

        /// <summary>
        /// Идентификатор плана тренировки
        /// </summary>
        long _workoutPlanId = -1;

        /// <summary>
        /// Идентификатор типа плана тренировки
        /// </summary>
        long _workoutPlanTypeId = -1;


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
        /// Идентификатор спортсмена
        /// </summary>
        public long athleteId
        {
            get { return _athleteId; }
            set { _athleteId = value; }
        }

        /// <summary>
        /// Идентификатор типа тренировки
        /// </summary>
        public Int16 workoutTypeId
        {
            get { return _workoutTypeId; }
            set { _workoutTypeId = value; }
        }

        /// <summary>
        /// Идентификатор плана тренировки
        /// </summary>
        public long workoutPlanId
        {
            get { return _workoutPlanId; }
            set { _workoutPlanId = value; }
        }

        /// <summary>
        /// Идентификатор типа плана тренировки
        /// </summary>
        public long workoutPlanTypeId
        {
            get { return _workoutPlanTypeId; }
            set { _workoutPlanTypeId = value; }
        }

        /// <summary>
        /// Идентификатор группы мыщц
        /// </summary>
        public Int16 musclesGroupId
        {
            get { return _musclesGroupId; }
            set { _musclesGroupId = value; }
        }

        /// <summary>
        /// Дата тренировки
        /// </summary>
        public string date
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(_date).ToLocalTime();
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
                        _date = dt.Ticks / 10000000;
                    }
                    catch
                    {
                        throw new Exception("Не удалось разобрать дату тренировки!");
                        //_date = 0;
                    }
                }
                else
                {
                    _date = 0;
                }
            }
        }

        /// <summary>
        /// Возвращает дату тренировки в виде числа
        /// </summary>
        public long dateInt64
        {
            get
            {
                return _date;
            }
        }

        /// <summary>
        /// Время начала тренировки
        /// </summary>
        public string timeBegin
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(_timeBegin);
                return string.Format("{0}:{1}", OtherMethods.IntToBinaryString(dt.Hour), OtherMethods.IntToBinaryString(dt.Minute), OtherMethods.IntToBinaryString(dt.Second));
            }
            set
            {
                //value = "08:30:00"
                string[] values = value.Split(new char[] { ':' });
                if (values.Length == 3)
                {
                    try
                    {
                        int h = Convert.ToInt32(values[0]);
                        int m = Convert.ToInt32(values[1]);
                        int s = Convert.ToInt32(values[2]);
                        TimeSpan dt = new TimeSpan(0, h, m, s, 0);
                        _timeBegin = dt.Ticks / 10000000;
                    }
                    catch
                    {
                        throw new Exception("Не удалось разобрать время начала тренировки!");
                        //_timeBegin = 0;
                    }
                }
                else
                {
                    _timeBegin = 0;
                }
            }
        }

        /// <summary>
        /// Возвращает время начала тренировки в виде числа
        /// </summary>
        public long timeBeginInt64
        {
            get
            {
                return _timeBegin;
            }
        }

        /// <summary>
        /// Время окончания тренировки
        /// </summary>
        public string timeEnd
        {
            get
            {
                System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dt = dt.AddSeconds(_timeEnd);
                return string.Format("{0}:{1}", OtherMethods.IntToBinaryString(dt.Hour), OtherMethods.IntToBinaryString(dt.Minute), OtherMethods.IntToBinaryString(dt.Second));
            }
            set
            {
                string[] values = value.Split(new char[] { ':' });
                if (values.Length == 3)
                {
                    try
                    {
                        int h = Convert.ToInt32(values[0]);
                        int m = Convert.ToInt32(values[1]);
                        int s = Convert.ToInt32(values[2]);
                        TimeSpan dt = new TimeSpan(0, h, m, s, 0);
                        _timeEnd = dt.Ticks / 10000000;
                    }
                    catch
                    {
                        throw new Exception("Не удалось разобрать время конца тренировки!");
                        //_timeEnd = 0;
                    }
                }
                else
                {
                    _timeEnd = 0;
                }
            }
        }

        /// <summary>
        /// Разминка
        /// </summary>
        public byte warmUp
        {
            get { return _warmUp; }
            set { _warmUp = value; }
        }

        /// <summary>
        /// Возвращает время завершение тренировки в виде числа
        /// </summary>
        public long timeEndInt64
        {
            get
            {
                return _timeEnd;
            }
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
                        throw new Exception("Не удалось разобрать дату записи тренировки!");
                        //_createDate = 0;
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
        /// Конструктор класса Workout
        /// </summary>
        /// <param name="athleteId">Идентификатор спортсмена</param>
        /// <param name="workoutTypeId">Идентификатор типа тренировки</param>
        /// <param name="musclesGroupsId">Идентификатор группы мыщц</param>
        /// <param name="date">Дата тренировки</param>
        /// <param name="timeBegin">Время начала тренировки</param>
        /// <param name="timeEnd">Время окончания тренировки</param>
        /// <param name="warmUp">Разминка (минуты)</param>
        /// <param name="workoutPlanId">Идентификатор плана тренировок</param>
        /// <param name="createDate">Дата записи</param>
        public Workout(long athleteId, Int16 workoutTypeId, Int16 musclesGroupId, DateTime date, TimeSpan timeBegin, TimeSpan timeEnd, byte warmUp, long workoutPlanId, DateTime createDate)
        {
            this._athleteId = athleteId;
            this._workoutTypeId = workoutTypeId;
            this._musclesGroupId = musclesGroupId;
            this.date = date.ToShortDateString();
            this.timeBegin = timeBegin.ToString();
            this.timeEnd = timeEnd.ToString();
            this._warmUp = warmUp;
            this.createDate = createDate.ToShortDateString();
            this._workoutPlanId = workoutPlanId;
        }

        /// <summary>
        /// Конструктор класса Workout
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="athleteId">Идентификатор спортсмена</param>
        /// <param name="workoutTypeId">Идентификатор типа тренировки</param>
        /// <param name="musclesGroupsId">Идентификатор группы мыщц</param>
        /// <param name="date">Дата тренировки</param>
        /// <param name="timeBegin">Время начала тренировки</param>
        /// <param name="timeEnd">Время окончания тренировки</param>
        /// <param name="warmUp">Разминка (минуты)</param>
        /// <param name="workoutPlanId">Идентификатор плана тренировок</param>
        /// <param name="createDate">Дата записи</param>
        public Workout(long id, long athleteId, Int16 workoutTypeId, Int16 musclesGroupsId, DateTime date, TimeSpan timeBegin, TimeSpan timeEnd, byte warmUp, long workoutPlanId, DateTime createDate)
        {
            this._id = id;
            this._athleteId = athleteId;
            this._workoutTypeId = workoutTypeId;
            this._musclesGroupId = musclesGroupsId;
            this.date = date.ToShortDateString();
            this.timeBegin = timeBegin.ToString();
            this.timeEnd = timeEnd.ToString();
            this._warmUp = warmUp;
            this.createDate = createDate.ToShortDateString();
            this._workoutPlanId = workoutPlanId;
        }

        /// <summary>
        /// Конструктор класса Workout
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="athleteId">Идентификатор спортсмена</param>
        /// <param name="workoutTypeId">Идентификатор типа тренировки</param>
        /// <param name="musclesGroupsId">Идентификатор группы мыщц</param>
        /// <param name="date">Дата тренировки</param>
        /// <param name="timeBegin">Время начала тренировки</param>
        /// <param name="timeEnd">Время окончания тренировки</param>
        /// <param name="warmUp">Разминка (минуты)</param>
        /// <param name="workoutPlanId">Идентификатор плана тренировок</param>
        /// <param name="createDate">Дата записи</param>
        public Workout(long id, long athleteId, Int16 workoutTypeId, Int16 musclesGroupsId, DateTime date, TimeSpan timeBegin, TimeSpan timeEnd, byte warmUp, long workoutPlanId, Int16 workoutPlanTypeId, DateTime createDate)
        {
            this._id = id;
            this._athleteId = athleteId;
            this._workoutTypeId = workoutTypeId;
            this._musclesGroupId = musclesGroupsId;
            this.date = date.ToShortDateString();
            this.timeBegin = timeBegin.ToString();
            this.timeEnd = timeEnd.ToString();
            this._warmUp = warmUp;
            this.createDate = createDate.ToShortDateString();
            this._workoutPlanId = workoutPlanId;
            this._workoutPlanTypeId = workoutPlanTypeId;
        }
        /// <summary>
        /// Конструктор класса Workout
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="athleteId">Идентификатор спортсмена</param>
        /// <param name="workoutTypeId">Идентификатор типа тренировки</param>
        /// <param name="musclesGroupsId">Идентификатор группы мыщц</param>
        /// <param name="date">Дата тренировки</param>
        /// <param name="timeBegin">Время начала тренировки</param>
        /// <param name="timeEnd">Время окончания тренировки</param>
        /// <param name="warmUp">Разминка (минуты)</param>
        /// <param name="workoutPlanId">Идентификатор плана тренировок</param>
        /// <param name="createDate">Дата записи</param>
        public Workout(long athleteId, Int16 workoutTypeId, Int16 musclesGroupsId, DateTime date, TimeSpan timeBegin, TimeSpan timeEnd, byte warmUp, long workoutPlanId, Int16 workoutPlanTypeId, DateTime createDate)
        {
            this._athleteId = athleteId;
            this._workoutTypeId = workoutTypeId;
            this._musclesGroupId = musclesGroupsId;
            this.date = date.ToShortDateString();
            this.timeBegin = timeBegin.ToString();
            this.timeEnd = timeEnd.ToString();
            this._warmUp = warmUp;
            this.createDate = createDate.ToShortDateString();
            this._workoutPlanId = workoutPlanId;
            this._workoutPlanTypeId = workoutPlanTypeId;
        }

        /// <summary>
        /// Конструктор класса Workout
        /// </summary>
        public Workout(DbDataReader reader)
        {
            this._id = reader.GetValue(0).ToString() == "" ? (long)0 : reader.GetInt64(0);
            this._date = reader.GetValue(1).ToString() == "" ? (long)0 : reader.GetInt64(1);
            this._timeBegin = reader.GetValue(2).ToString() == "" ? (long)0 : reader.GetInt64(2);
            this._timeEnd = reader.GetValue(3).ToString() == "" ? (long)0 : reader.GetInt64(3);
            this._warmUp = reader.GetValue(4).ToString() == "" ? (byte)0 : reader.GetByte(4);
            this._musclesGroupId = reader.GetValue(5).ToString() == "" ? (Int16)0 : reader.GetInt16(5);
            this._workoutPlanId = reader.GetValue(6).ToString() == "" ? (long)0 : reader.GetInt64(6);
            this._workoutTypeId = reader.GetValue(7).ToString() == "" ? (Int16)0 : reader.GetInt16(7);
            this._athleteId = reader.GetValue(8).ToString() == "" ? (long)0 : reader.GetInt64(8);
            this._createDate = reader.GetValue(9).ToString() == "" ? (long)0 : reader.GetInt64(9);
            this._workoutPlanTypeId = reader.GetValue(10).ToString() == "" ? (Int16)0 : reader.GetInt16(10);
        }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return string.Format(
                    "\"INSERT INTO {0} ([id], [date], [timeBegin], [timeEnd], [warmUp], [musclesGroupId], [workoutPlanId], [workoutTypeId], [athleteId], [createDate], [workoutPlanTypeId]) VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}); \" +",
                    SQLite.TABLE_WORKOUT,
                    _id,
                    _date,
                    _timeBegin,
                    _timeEnd,
                    _warmUp,
                    _musclesGroupId,
                    _workoutPlanId,
                    _workoutTypeId,
                    _athleteId,
                    _createDate,
                    _workoutPlanTypeId
                    );
        }
    }
}
