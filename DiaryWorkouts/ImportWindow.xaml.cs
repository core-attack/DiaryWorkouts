using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using DiaryWorkouts.DataBase;
using DiaryWorkouts.BaseClasses;

namespace DiaryWorkouts
{
    /// <summary>
    /// Старый класс, описывающий тренировку
    /// </summary>
    public class OldWorkout
    {
        public string number = "";
        public string sheff = "";
        public string sportsmen = "";
        public string date = "";
        public string dayOfWeek = "";
        public string timeOfDay = "";
        public string warmUp = "";
        public string work = "";
        public string result = "";
    }

    /// <summary>
    /// Импорт базы данных из старых версий приложения
    /// </summary>
    public partial class ImportWindow : Window
    {
        /// <summary>
        /// Наименование папки, куда будут копироваться файлы баз данных
        /// </summary>
        const string directoryName = "import";
        /// <summary>
        /// Приставка для выходного файла
        /// </summary>
        const string outputFilePreName = "export file ";
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        bool ready = false;

        public ImportWindow(long userId)
        {
            InitializeComponent();
            labelDirectoryImport.Content = directoryName;
            if (!Directory.Exists(directoryName))
                try
                {
                    Directory.CreateDirectory(directoryName);
                }
                catch (Exception e)
                {
                    ErrorsHandler.ShowError(e);
                }
            this.userId = userId;
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            ImportDB();
            if (ready)
            {
                labelStatus.Content = "Файл создан.";
                buttonOpen.IsEnabled = true;
            }
            else 
            {
                labelStatus.Content = "Не удалось создать файл.";
                buttonOpen.IsEnabled = false;
            }
        }

        /// <summary>
        /// Ипмортирует базу данных
        /// </summary>
        public void ImportDB()
        {
            try
            {
                SQLite sqlite = new SQLite();
                sqlite.Connect();
                List<OldWorkout> list = new List<OldWorkout>();
                List<string> listInsertWorkouts = new List<string>();
                List<string> listInsertWorks = new List<string>();
                List<string> listInserResults = new List<string>();
                List<Work> listWorks = new List<Work>();
                List<Result> listResults = new List<Result>();
                string[] files = Directory.GetFiles(directoryName);
                foreach (string file in files)
                {
                    if (file.IndexOf(outputFilePreName) == -1)
                    {
                        StreamReader sr = new StreamReader(file);
                        OldWorkout workout = new OldWorkout();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line == "#begin")
                                workout = new OldWorkout();
                            else if (line.IndexOf("#number:") != -1)
                                workout.number = line.Replace("#number:", "");
                            else if (line.IndexOf("#date:") != -1)
                                workout.date = line.Replace("#date:", "");
                            else if (line.IndexOf("#dayOfWeek:") != -1)
                                workout.dayOfWeek = line.Replace("#dayOfWeek:", "");
                            else if (line.IndexOf("#timeOfDay:") != -1)
                                workout.timeOfDay = line.Replace("#timeOfDay:", "");
                            else if (line.IndexOf("#warmUp:") != -1)
                                workout.warmUp = line.Replace("#warmUp:", "");
                            else if (line.IndexOf("#work:") != -1)
                                workout.work = line.Replace("#work:", "");
                            else if (line.IndexOf("#result:") != -1)
                                workout.result = line.Replace("#result:", "");
                            else if (line.IndexOf("#sportsmen:") != -1)
                                workout.sportsmen = line.Replace("#sportsmen:", "");
                            else if (line.IndexOf("#sheff:") != -1)
                                workout.sheff = line.Replace("#sheff:", "");
                            else if (line.IndexOf("#end") != -1)
                                list.Add(workout);
                        }
                    }
                }

                /*
                 * command.CommandText = "CREATE TABLE IF NOT EXISTS '" + TABLE_WORKOUT + "' (" +
                            " [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                            " [date] INTEGER," +
                            " [timeBegin] INTEGER," +
                            " [timeEnd] INTEGER," +
                            " [warmUp] INTEGER," +
                            " [musclesGroupId] INTEGER," +
                            " [workoutPlanId] INTEGER," +
                            " [workoutTypeId] INTEGER," +
                            " [athleteId] INTEGER," +
                            " [createDate] INTEGER" +
                        ");";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS '" + TABLE_WORK + "' (" +
                            " [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                            " [resultId] INTEGER," +
                            " [workoutId] INTEGER," +
                            " [workTypeId] INTEGER," +
                            " [comment] VARCHAR(200)" +
                        ");";
                        command.ExecuteNonQuery();

                        command.CommandText = "CREATE TABLE IF NOT EXISTS '" + TABLE_RESULT + "' (" +
                            " [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                            " [time] INTEGER," + //время (800)
                            " [weight] REAL," + //вес (жим лёжа)
                            " [repeat] INTEGER," + //повторов
                            " [distance] REAL," + //дальность в метрах(копье)
                            " [points] INTEGER" + //очки (многоборье)
                            " [place] INTEGER" + //место среди всех участвовавших в соревнованиях
                        ");";
                 */
                string[] timesOfDay = { "Утро", "День", "Вечер", "Ночь" };

                Workout lastWorkout = sqlite.GetLastWorkout(userId);
                long lastWorkoutId = lastWorkout != null ? lastWorkout.id : 0;
                long workoutId = lastWorkoutId + 1;
                if (int.Parse(list[0].number) > int.Parse(list.Last().number)) //пусть в БД по порядку идентификаторы идут
                    list.Reverse();
                string t = DateTime.Now.ToShortTimeString().Replace(":", "-");
                string d = DateTime.Now.ToShortDateString();
                StreamWriter sw = new StreamWriter(directoryName + "/" + outputFilePreName + t + " " + d + ".txt");
                foreach (OldWorkout ow in list)
                {
                    //нужно в классе сделать метод toInsert, где бы можно было получить sql строку вставки
                    //здесь объявлять экземпляр класса, наполнять его необходимыми полями, дергать этот метод и получать готовую строку
                    DateTime date = new DateTime();
                    if (ow.date.Split('.').Length == 3)
                        date = new DateTime(int.Parse(ow.date.Split('.')[2]), int.Parse(ow.date.Split('.')[1]), int.Parse(ow.date.Split('.')[0]));
                    TimeSpan begin = new TimeSpan(GetBeginHourByTimeOfDay(ow.timeOfDay), 0, 0);
                    TimeSpan end = new TimeSpan(GetEndHourByTimeOfDay(ow.timeOfDay), 0, 0);
                    byte warmUp = ow.warmUp.IndexOf("Бег") != -1 ? ow.warmUp.Split(' ').Length > 1 ? byte.Parse(ow.warmUp.Split(' ')[1]) : (byte)0 : (byte)0;
                    Workout workout = new Workout(workoutId, userId, 0, 0, date, begin, end, warmUp, 0, DateTime.Now);
                    listInsertWorkouts.Add(workout.GetSQLInsert());
                    //работы отделяются друг от друга зяпятыми
                    string[] works = ow.work.Split(',');
                    //разделители количества повторов и работы
                    char[] sepCountWork = { 'x', 'X', 'х', 'Х' };
                    //разделители разных работ
                    char[] sepWorks = { '(', ')', '/' };
                    //разделители комментариев
                    char[] sepComment = { '(', ')' };
                    int count;
                    string distance;
                    string comment = "";
                    string[] resultsParts = ow.result.Split(';');//отделим результаты разных работ 
                    foreach (string result in resultsParts)
                    {
                        if (result.IndexOf('(') != -1)
                        {
                            comment += result.Substring(result.IndexOf('(') + 1, result.IndexOf(')') - result.IndexOf('(') - 1);
                        }
                        string[] results = result.Split(',');//отделим результаты каждого повтора работы
                        foreach (string r in results)
                        {
                            listResults.Add(new Result(r.Split('(')[0]));
                        }
                    }

                    foreach (Result res in listResults)
                        sw.WriteLine(res.GetSQLInsert());

                    sw.WriteLine("");

                    foreach (string work in works)
                    {
                        count = 0;
                        distance = "";
                        if (work.ToLower().IndexOf('x') != -1 || work.ToLower().IndexOf('х') != -1)
                        {
                            string[] parts = work.Split(sepCountWork);
                            if (parts.Length > 1)
                            {
                                try
                                {
                                    count = int.Parse(parts[0]);
                                }
                                catch (Exception e)
                                {
                                    count = 1;
                                    ErrorsHandler.ShowError(e);
                                }
                                if (parts[1].IndexOf('(') != -1)//(1500/600)
                                {
                                    string[] ws = parts[1].Split(sepWorks);
                                    foreach (string w in ws)
                                    {
                                        distance = GetFullNameOfDistance(w.Trim());
                                        listWorks.Add(new Work(0, workoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, distance), comment));

                                    }
                                }
                                else //8х400
                                {
                                    distance = GetFullNameOfDistance(parts[1].Trim());
                                    listWorks.Add(new Work(0, workoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, distance), comment));

                                }
                            }
                        }
                        else if (work.IndexOf('/') != -1)
                        {
                            count = 1;
                            string[] ws = work.Split('/');
                            foreach (string w in ws)
                            {
                                distance = GetFullNameOfDistance(w.Trim());
                                listWorks.Add(new Work(0, workoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, distance), comment));

                            }
                        }
                        else
                        {
                            count = 1;
                            distance = GetFullNameOfDistance(work.Trim());
                            listWorks.Add(new Work(0, workoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, distance), comment));
                        }
                    }

                    foreach (Work w in listWorks)
                        sw.WriteLine(w.GetSQLInsert());
                    /*
    Формат ввода работы: количество раз х дистанция; количество раз х дистанция;...
    Например, 8х400; 2х60.
    Или количество раз х (дистанция / другая дистанция).
    Например, 2х(1500/600).
    Количество раз можно не указывать, если выполнялся контрольный забег. 
    Например, 400 или 400/300/200/100.
    Если прошли соревнования, то сначала вводится дистанция, потом в скобках пояснения.
    Например, 8 км (соревнования).
    Пожелание: желательно писать все пояснения к дистанциям в скобках.
    Например, 8 км (темповый бег).
                     * 
    Формат ввода результата: время, время, ... 
    Например, 77'', 74'', 70'', 68''
    Разные виды работы отделяются друг от друга точкой с запятой. 
    Например, 400/200/100; пресс с блином 5 кг...
    Важно помнить, что результаты разных работ, введенных в окне "Работа" через запятую, отделяются друг от друга точками с запятой.
    Например: Работа: 5х400, прыжковые, силовst/
    Результат: 1.02, 1.01, 1.00, 59.04, 58.40, 57.10; прыжки через 9 барьеров (10 подходов); пресс... 
    Если результата для дистанции нет или он не фиксировался, то введите "х"
    Например, 22.02, х, 23.04...  


                     */


                    workoutId++;
                }
                sw.Close();
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }
        /// <summary>
        /// Возвращает число часов относительно времени суток для времени начала тренировки
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <returns></returns>
        private int GetBeginHourByTimeOfDay(string timeOfDay)
        {
            switch (timeOfDay)
            {
                case "Утро": return 9;
                case "День": return 12;
                case "Вечер": return 17;
                case "Ночь": return 0;
            }
            return -1;
        }
        /// <summary>
        /// Возвращает число часов относительно времени суток для времени завершения тренировки
        /// </summary>
        /// <param name="timeOfDay"></param>
        /// <returns></returns>
        private int GetEndHourByTimeOfDay(string timeOfDay)
        {
            switch (timeOfDay)
            {
                case "Утро": return 12;
                case "День": return 15;
                case "Вечер": return 20;
                case "Ночь": return 3;
            }
            return -1;
        }

        /// <summary>
        /// Возвращает полное наименование дистанции
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private string GetFullNameOfDistance(string distance)
        {
            int d = -1;
            try
            {
                d = int.Parse(distance);
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
                //MessageBox.Show(distance);
            }
            if (d > 0)
                if (d < 30)
                    return "Кросс " + distance;
                else
                    return "Бег " + distance + " м";
            return distance;
        }
    }
}
