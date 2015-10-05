using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaryWorkouts
{
    class OtherMethods
    {
        /// <summary>
        /// Кириллические месяцы
        /// </summary>
        public Dictionary<byte, string> Monthes = new Dictionary<byte, string>();
        /// <summary>
        /// Кириллические сокращенные дни недели
        /// </summary>
        public Dictionary<string, string> DaysWeek = new Dictionary<string, string>();
        public OtherMethods()
        {
            Monthes.Clear();
            Monthes.Add(1, "Январь");
            Monthes.Add(2, "Февраль");
            Monthes.Add(3, "Март");
            Monthes.Add(4, "Апрель");
            Monthes.Add(5, "Май");
            Monthes.Add(6, "Июнь");
            Monthes.Add(7, "Июль");
            Monthes.Add(8, "Август");
            Monthes.Add(9, "Сентябрь");
            Monthes.Add(10, "Октябрь");
            Monthes.Add(11, "Ноябрь");
            Monthes.Add(12, "Декабрь");

            DaysWeek.Clear();
            DaysWeek.Add("Monday", "ПН");
            DaysWeek.Add("Tuesday", "ВТ");
            DaysWeek.Add("Wednesday", "СР");
            DaysWeek.Add("Thursday", "ЧТ");
            DaysWeek.Add("Friday", "ПТ");
            DaysWeek.Add("Saturday", "СБ");
            DaysWeek.Add("Sunday", "ВС");

            //_ потому что лень новый список создавать
            DaysWeek.Add("Monday_", "Понедельник");
            DaysWeek.Add("Tuesday_", "Вторник");
            DaysWeek.Add("Wednesday_", "Среда");
            DaysWeek.Add("Thursday_", "Четверг");
            DaysWeek.Add("Friday_", "Пятница");
            DaysWeek.Add("Saturday_", "Суббота");
            DaysWeek.Add("Sunday_", "Воскресенье");
        }

        /// <summary>
        /// Логирует в консоль
        /// </summary>
        /// <param name="s"></param>
        public static void Debug(string s)
        {
            Console.WriteLine(s);
        }

        /// <summary>
        /// Приводит число к двусимвольной строке
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string IntToBinaryString(int i)
        {
            return i < 10 ? "0" + i.ToString() : i.ToString();
        }

        /// <summary>
        /// Приводит число к трехсимвольной строке
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string IntToThirdString(int i)
        {
            if (i < 10)
                return "00" + i.ToString();
            else if (i < 100)
                return "0" + i.ToString();
            else
                return i.ToString();
        }

        /// <summary>
        /// Преобразует дату из целочисленного формата в строчный
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDate(long date)
        {
            System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dt = dt.AddSeconds(date).ToLocalTime();
            return IntToBinaryString(dt.Day) + "." + IntToBinaryString(dt.Month) + "." + dt.Year;
        }

        /// <summary>
        /// Преобразует дату из целочисленного формата в строчный
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateAsDateTime(long date)
        {
            System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dt = dt.AddSeconds(date).ToLocalTime();
            return dt;
        }

        /// <summary>
        /// Преобразует дату из строчного формата в целочисленный
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetDate(string date)
        {
            string[] values = date.Split(new char[] { '.' });
            if (values.Length == 3)
            {
                try
                {
                    if (values[2].Length == 4)
                    {
                        System.DateTime dt = new DateTime(
                            Convert.ToInt32(values[2]),
                            Convert.ToInt32(values[1]),
                            Convert.ToInt32(values[0]),
                            0, 0, 0, 0, System.DateTimeKind.Utc);
                        return dt.Ticks / 10000000;
                    }
                    else
                    {
                        System.DateTime dt = new DateTime(
                            Convert.ToInt32(values[0]),
                            Convert.ToInt32(values[1]),
                            Convert.ToInt32(values[2]),
                            0, 0, 0, 0, System.DateTimeKind.Utc);
                        return dt.Ticks / 10000000;
                    }
                }
                catch(Exception e)
                {
                    throw e;
                    //return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Преобразует дату из строчного формата в целочисленный
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetDate(DateTime date)
        {
            try
            {
                System.DateTime dt = new DateTime(
                    date.Year, date.Month, date.Day,
                    0, 0, 0, 0, System.DateTimeKind.Utc);
                return dt.Ticks / 10000000;
        }
            catch (Exception e)
            {
                throw e;
                //return 0;
            }
        }
        /// <summary>
        /// Преобразует время из целочисленного формата в строчный
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTime(long time)
        {
            System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dt = dt.AddMilliseconds(time);
            return string.Format(BaseClasses.Result.timeMask,
                OtherMethods.IntToBinaryString(dt.Hour),
                OtherMethods.IntToBinaryString(dt.Minute),
                OtherMethods.IntToBinaryString(dt.Second),
                OtherMethods.IntToBinaryString(dt.Millisecond));
        }
        /// <summary>
        /// Преобразует время из строчного формата в целочисленный
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTime(string time)
        {
            string[] values = time.Split(new char[] { ':', '.' });
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
                    return ts.Ticks / 10000;
                }
                catch
                {
                    throw new Exception("Не удалось разобрать время!");
                    //return 0;

                }
            }
            else if (values.Length == 3)
            {
                if (time.IndexOf(".") != -1) //значит есть милисекунды
                {
                    TimeSpan ts = new TimeSpan(0,
                            0,
                            Convert.ToInt32(values[0]),
                            Convert.ToInt32(values[1]),
                            Convert.ToInt32(values[2]));
                    return ts.Ticks / 10000;
                }
                else
                {
                    TimeSpan ts = new TimeSpan(0,
                            Convert.ToInt32(values[0]),
                            Convert.ToInt32(values[1]),
                            Convert.ToInt32(values[2]));
                    return ts.Ticks / 10000;
                }
            }
            else if (values.Length == 2)
            {
                if (time.IndexOf(":") != -1)//значит минуты и секунды (эта ветвь условия не используется, если будет использоваться, то сделать проверку количества минут и секунд, чтобы отсекало часы итп)
                {
                    TimeSpan ts = new TimeSpan(0, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
                    return ts.Ticks / 10000;
                }
                else //иначе секунды и милисекунды
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
                    return ts.Ticks / 10000;
                }
            }
            else if (values.Length == 1)
            {
                int s = Convert.ToInt32(values[0]);
                int m = s / 60;
                int h = m / 60;
                TimeSpan ts = new TimeSpan(h, m, s - h * 3600 - m * 60);
                return ts.Ticks / 10000;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Преобразует время из целочисленного формата в строчный c милисекундами
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        //public static string GetTimeMs(long time)
        //{
        //    System.DateTime dt = new DateTime(1, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //    dt = dt.AddMilliseconds(time);
        //    return string.Format("{0}:{1}:{2}.{3}",
        //        OtherMethods.IntToBinaryString(dt.Hour),
        //        OtherMethods.IntToBinaryString(dt.Minute),
        //        OtherMethods.IntToBinaryString(dt.Second),
        //        OtherMethods.IntToBinaryString(dt.Millisecond));
        //}
        /// <summary>
        /// Возвращает правильное склонение слова "день"
        /// </summary>
        /// <param name="countDay">Количество дней цифрами</param>
        /// <returns></returns>
        public static string getCurrentFormOfWordDay(string countDay)
        {
            char lastChar = countDay.Last();
            if (countDay == "2" || countDay == "3" || countDay == "4")
                return countDay + " дня";
            switch (lastChar)
            {
                case '1': return countDay + " день"; 
                default: return countDay + " дней"; 
            }
            //return "";
        }
        /// <summary>
        /// Приводит строку к читабельному виду
        /// </summary>
        /// <param name="s">строка, содержащая перечисления</param>
        /// <param name="separator">разделитель частей строки</param>
        /// <param name="equalRecordsSeparator">разделитель одинаковых работ/повторов</param>
        public static string NormalizeStringComponents(string s, char separator, char equalRecordsSeparator)
        {
            string[] parts = s.Split(separator);
            string normString = "";
            string previous = "";
            string current = "";
            string next = "";
            int count = 1;
            for (int i = 0; i < parts.Length; i++)
            {
                current = parts[i].Trim();
                next = "";
                if (i + 1 < parts.Length)
                    next = parts[i + 1].Trim();

                if (current.Equals(next))
                {
                    count++;
                    previous = current;
                }
                else if (next != "")
                {
                    if (count != 1)
                        normString += string.Format(normString == "" ? "{0}{1}{2}" : "{3} {0}{1}{2}", count, equalRecordsSeparator, previous, separator);
                    else
                        normString += string.Format(normString == "" ? "{0}" : "{1} {0}", current, separator);
                    count = 1;
                }
                else
                {
                    if (count != 1)
                        normString += string.Format(normString == "" ? "{0}{1}{2}" : "{3} {0}{1}{2}", count, equalRecordsSeparator, previous, separator);
                    else
                        normString += string.Format(normString == "" ? "{0}" : "{1} {0}", current, separator); ;
                }
            }
            return normString;
        }
    }
}
