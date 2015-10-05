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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.DataBase;
using DiaryWorkouts.ReferenceBooks;

namespace DiaryWorkouts
{
    /// <summary>
    /// Горизонтальный календарь
    /// </summary>
    public partial class HorizontalCalenderUserControl : UserControl
    {
        /// <summary>
        /// Фон дня недели
        /// </summary>
        Brush WEEK_DAY_BACKGROUND = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        /// <summary>
        /// Фон дня месяца
        /// </summary>
        Brush THIS_MONTH_DAY_BACKGROUND = new SolidColorBrush(Color.FromRgb(240, 240, 240));
        /// <summary>
        /// Фон дня месяца
        /// </summary>
        Brush OTHER_MONTH_DAY_BACKGROUND = new SolidColorBrush(Color.FromRgb(227, 227, 227));
        /// <summary>
        /// Фон выделенного дня недели
        /// </summary>
        Brush SELECTED_WEEK_DAY_BACKGROUND = new SolidColorBrush(Color.FromRgb(185, 209, 234));
        /// <summary>
        /// Фон выделенного дня месяца
        /// </summary>
        Brush SELECTED_DAY_BACKGROUND = new SolidColorBrush(Color.FromRgb(185, 209, 234));
        /// <summary>
        /// Отступ справа от грида дня 
        /// </summary>
        double GRID_TEMPLATE_MARGIN_RIGHT = 38;
        /// <summary>
        /// Количество отображаемых дней
        /// </summary>
        byte COUNT_DAYS = 21;
        /// <summary>
        /// Количество прокручиваемых дней при скролле мыши
        /// </summary>
        byte COUNT_DAY_WHEN_MOUSE_SCROLL = 1;
        /// <summary>
        /// День, выбранный по умолчанию
        /// </summary>
        byte DEFAULT_SELECTED_DAY = 11;
        /// <summary>
        /// Левая скобка, выделяющая текущую дату
        /// </summary>
        char LABEL_BRACKET_LEFT = '[';
        /// <summary>
        /// Правая скобка, выделяющая текущую дату
        /// </summary>
        char LABEL_BRACHET_RIGHT = ']';
        /// <summary>
        /// Размер шрифта скобок, выделяющих текущую дату
        /// </summary>
        byte LABEL_BRAKET_FONT_SIZE = 100;
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        FontFamily DEFAULT_FONT;
        /// <summary>
        /// Размер шрифта по умолчанию
        /// </summary>
        double DEFAULT_FONT_SIZE;

        double LABEL_SELECTED_DAY_SPACE = 10;

        double LABEL_BRAKET_LEFT_SPACE = 5;
        double LABEL_BRAKET_RIGHT_SPACE = 16;
        double LABEL_BRAKET_MARGIN_LEFT = 17;
        double LABEL_BRAKET_MARGIN_TOP = -10;
        double LABEL_BRAKET_PADDING_RIGHT = 0;
        double LABEL_BRAKET_PADDING_BOTTOM = -8;
        Brush LABEL_BRAKET_COLOR = new SolidColorBrush(Color.FromRgb(166, 166, 164));
        /// <summary>
        /// Экземпляр БД
        /// </summary>
        SQLite sqlite = new SQLite();
        /// <summary>
        /// Последний добавленный грид на форму
        /// </summary>
        Grid lastGrid = null;
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        /// <summary>
        /// Разделитель идентификатора грида
        /// </summary>
        char gridIdSeparator;
        //Теперь то, к чему можно будет получить доступ из вне
        /// <summary>
        /// Дата кликнутой лейблы календаря
        /// </summary>
        public DateTime clickedDate;
        /// <summary>
        /// Текущая дата календаря
        /// </summary>
        public DateTime selectedDate = DateTime.Now;
        /// <summary>
        /// Другие параметры
        /// </summary>
        OtherMethods om = new OtherMethods();

        public HorizontalCalenderUserControl(FontFamily DEFAULT_FONT, double DEFAULT_FONT_SIZE, System.Globalization.NumberFormatInfo format, char gridIdSeparator)
        {
            InitializeComponent();
            this.format = format;
            this.gridIdSeparator = gridIdSeparator;
            this.DEFAULT_FONT = DEFAULT_FONT;
            this.DEFAULT_FONT_SIZE = DEFAULT_FONT_SIZE;
            Init();
        }
        public HorizontalCalenderUserControl(FontFamily DEFAULT_FONT, double DEFAULT_FONT_SIZE, System.Globalization.NumberFormatInfo format, char gridIdSeparator, DateTime selectedDate)
        {
            InitializeComponent();
            this.format = format;
            this.gridIdSeparator = gridIdSeparator;
            this.DEFAULT_FONT = DEFAULT_FONT;
            this.DEFAULT_FONT_SIZE = DEFAULT_FONT_SIZE;
            this.selectedDate = selectedDate;
            Init();
        }
        /// <summary>
        /// Инициализирует все пользовательские компоненты
        /// </summary>
        public void Init()
        {
            FontFamily = DEFAULT_FONT;
            FontSize = DEFAULT_FONT_SIZE;

            sqlite.Connect();

            gridDayTemplate.Visibility = System.Windows.Visibility.Hidden;
            FillAllCalenderLables(selectedDate);

            //buttonNext.MouseUp += ScrollCalenderDatesRight;
            //buttonPrevious.MouseUp += ScrollCalenderDatesLeft;
        }
        /// <summary>
        /// Определяет показывать один месяц или два (в подписи сверху календаря)
        /// </summary>
        public void SetMonthLabels(DateTime date)
        {
            byte delta = (byte)(COUNT_DAYS / 2);
            byte monthPrevious = (byte)date.Subtract(new TimeSpan(delta, 0, 0, 0)).Month;
            byte monthNext = (byte)date.Add(new TimeSpan(delta, 0, 0, 0)).Month;
            labelMonth.Content = string.Format("{0}", om.Monthes[monthPrevious]);
            labelMonthPrevious.Content = string.Format("{0} {1}", om.Monthes[monthPrevious], date.Year);
            labelMonthNext.Content = string.Format("{0} {1}", om.Monthes[monthNext], date.Year);
        }
        /// <summary>
        /// Заполняет все лейблы датами и днями недели
        /// </summary>
        /// <param name="date">Дата, находящаяся в середине календаря</param>
        public void FillAllCalenderLables(DateTime middleDate)
        {
            try
            {
                sqlite.Connect();
                Dictionary<long, Dictionary<string, bool>> datesOfWorkouts = sqlite.IsKardioOrHardWorkRecordExistBetweenDates(
                                                                                    middleDate.Subtract(new TimeSpan(DEFAULT_SELECTED_DAY, 0, 0, 0)),
                                                                                    middleDate.Add(new TimeSpan(DEFAULT_SELECTED_DAY, 0, 0, 0)));
                Dictionary<string, bool> currentWorkoutSportTypes;
                RemoveAllCalenderLabels();
                SetMonthLabels(middleDate);

                lastGrid = null;
                DateTime now = middleDate;
                selectedDate = middleDate;
                byte count = 1;
                //Скобки, выделяющие текущий день
                Label leftBraket = new Label();
                leftBraket.Content = LABEL_BRACKET_LEFT;
                leftBraket.FontSize = LABEL_BRAKET_FONT_SIZE;
                leftBraket.Foreground = LABEL_BRAKET_COLOR;
                

                Label rightBraket = new Label();
                rightBraket.Content = LABEL_BRACHET_RIGHT;
                rightBraket.FontSize = LABEL_BRAKET_FONT_SIZE;
                rightBraket.Foreground = LABEL_BRAKET_COLOR;

                gridCalender.Width = 0;
                while (count <= COUNT_DAYS)
                {
                    Grid newGrid = new Grid();
                    newGrid.Name = gridDayTemplate.Name + "_" + gridIdSeparator;
                    newGrid.Width = gridDayTemplate.Width;
                    newGrid.Height = gridDayTemplate.Height;
                    newGrid.HorizontalAlignment = gridDayTemplate.HorizontalAlignment;
                    newGrid.VerticalAlignment = gridDayTemplate.VerticalAlignment;
                    newGrid.Background = gridDayTemplate.Background;

                    newGrid.Margin = new Thickness(
                        lastGrid == null ? gridDayTemplate.Margin.Left : lastGrid.Margin.Left + GRID_TEMPLATE_MARGIN_RIGHT,
                        lastGrid == null ? gridDayTemplate.Margin.Top : lastGrid.Margin.Top,
                        lastGrid == null ? gridDayTemplate.Margin.Right : lastGrid.Margin.Right,
                        lastGrid == null ? gridDayTemplate.Margin.Bottom : lastGrid.Margin.Bottom);

                    Label monthDay = new Label();
                    Label weekDay = new Label();
                    Label kardioExist = new Label();
                    Label hardWorkExist = new Label();
                    Label eventExist = new Label();

                    weekDay.HorizontalContentAlignment = labelWeekDay.HorizontalContentAlignment;
                    weekDay.VerticalContentAlignment = labelWeekDay.VerticalContentAlignment;
                    monthDay.HorizontalContentAlignment = labelDay.HorizontalContentAlignment;
                    monthDay.VerticalContentAlignment = labelDay.VerticalContentAlignment;

                    DateTime date;

                    if (count < DEFAULT_SELECTED_DAY)
                    {
                        date = now.Subtract(new TimeSpan(DEFAULT_SELECTED_DAY - count, 0, 0, 0));
                    }
                    else if (count > DEFAULT_SELECTED_DAY)
                    {
                        date = now.Add(new TimeSpan(count - DEFAULT_SELECTED_DAY, 0, 0, 0));

                        if (count - 1 == DEFAULT_SELECTED_DAY)
                            newGrid.Margin = new Thickness(
                                lastGrid.Margin.Left + GRID_TEMPLATE_MARGIN_RIGHT + LABEL_SELECTED_DAY_SPACE,
                                lastGrid.Margin.Top,
                                lastGrid.Margin.Right,
                                lastGrid.Margin.Bottom);
                    }
                    else
                    {
                        date = now;

                        leftBraket.Margin = new Thickness(lastGrid.Margin.Left + LABEL_BRAKET_MARGIN_LEFT + LABEL_BRAKET_LEFT_SPACE, LABEL_BRAKET_MARGIN_TOP, LABEL_BRAKET_PADDING_RIGHT, LABEL_BRAKET_PADDING_BOTTOM);
                        gridCalender.Children.Add(leftBraket);

                        newGrid.Margin = new Thickness(
                                lastGrid.Margin.Left + GRID_TEMPLATE_MARGIN_RIGHT + LABEL_SELECTED_DAY_SPACE,
                                lastGrid.Margin.Top,
                                lastGrid.Margin.Right,
                                lastGrid.Margin.Bottom);


                        rightBraket.Margin = new Thickness(lastGrid.Margin.Left + LABEL_BRAKET_MARGIN_LEFT + newGrid.Width + LABEL_BRAKET_RIGHT_SPACE, LABEL_BRAKET_MARGIN_TOP, LABEL_BRAKET_PADDING_RIGHT, LABEL_BRAKET_PADDING_BOTTOM);
                        gridCalender.Children.Add(rightBraket);
                    }

                    monthDay.Name = labelDay.Name + "_" + gridIdSeparator;
                    monthDay.Content = OtherMethods.IntToBinaryString(date.Day);
                    monthDay.DataContext = date;
                    monthDay.Width = labelDay.Width;
                    monthDay.Height = labelDay.Height;
                    monthDay.Background = labelDay.Background;
                    monthDay.HorizontalAlignment = labelDay.HorizontalAlignment;
                    monthDay.VerticalAlignment = labelDay.VerticalAlignment;
                    monthDay.Margin = new Thickness(labelDay.Margin.Left, labelDay.Margin.Top, labelDay.Margin.Right, labelDay.Margin.Bottom);
                    monthDay.MouseLeftButtonUp += labelDay_MouseLeftButtonUp;
                    monthDay.MouseRightButtonUp += labelDay_MouseRightButtonUp;

                    weekDay.Name = labelWeekDay.Name + "_" + gridIdSeparator;
                    weekDay.Content = om.DaysWeek[date.DayOfWeek.ToString()];
                    weekDay.Width = labelWeekDay.Width;
                    weekDay.Height = labelWeekDay.Height;
                    weekDay.Background = labelWeekDay.Background;
                    weekDay.HorizontalAlignment = labelWeekDay.HorizontalAlignment;
                    weekDay.VerticalAlignment = labelWeekDay.VerticalAlignment;
                    weekDay.Margin = new Thickness(labelWeekDay.Margin.Left, labelWeekDay.Margin.Top, labelWeekDay.Margin.Right, labelWeekDay.Margin.Bottom);

                    kardioExist.Margin = new Thickness(labelKardioExist.Margin.Left - 17, labelKardioExist.Margin.Top - 8, labelKardioExist.Margin.Right, labelKardioExist.Margin.Bottom);
                    kardioExist.HorizontalContentAlignment = labelKardioExist.HorizontalAlignment;
                    kardioExist.VerticalContentAlignment = labelKardioExist.VerticalAlignment;
                    kardioExist.Background = labelKardioExist.Background;
                    kardioExist.Width = labelKardioExist.Width;
                    kardioExist.Height = labelKardioExist.Height;

                    hardWorkExist.Margin = new Thickness(labelHardWorkExist.Margin.Left, labelHardWorkExist.Margin.Top - 8, labelHardWorkExist.Margin.Right, labelHardWorkExist.Margin.Bottom);
                    hardWorkExist.HorizontalContentAlignment = labelHardWorkExist.HorizontalAlignment;
                    hardWorkExist.VerticalContentAlignment = labelHardWorkExist.VerticalAlignment;
                    hardWorkExist.Background = labelHardWorkExist.Background;
                    hardWorkExist.Width = labelHardWorkExist.Width;
                    hardWorkExist.Height = labelHardWorkExist.Height;

                    eventExist.Margin = new Thickness(labelEventExist.Margin.Left, labelEventExist.Margin.Top - 8, labelEventExist.Margin.Right, labelEventExist.Margin.Bottom);
                    eventExist.HorizontalContentAlignment = labelEventExist.HorizontalAlignment;
                    eventExist.VerticalContentAlignment = labelEventExist.VerticalAlignment;
                    eventExist.Background = labelEventExist.Background;
                    eventExist.Width = labelEventExist.Width;
                    eventExist.Height = labelEventExist.Height;
                    //sqlite.Connect();
                    //bool[] exists = sqlite.IsKardioOrHardWorkRecordExist(date);
                    long key = OtherMethods.GetDate(date);
                    if (datesOfWorkouts.ContainsKey(key))
                    {
                        currentWorkoutSportTypes = datesOfWorkouts[key];
                        if (currentWorkoutSportTypes["event"])
                        {
                            hardWorkExist.Visibility = System.Windows.Visibility.Hidden;
                            kardioExist.Visibility = System.Windows.Visibility.Hidden;
                        }
                        else
                        {
                            eventExist.Visibility = System.Windows.Visibility.Hidden;
                            if (currentWorkoutSportTypes["kardio"] && !currentWorkoutSportTypes["hardWork"])
                                hardWorkExist.Visibility = System.Windows.Visibility.Hidden;
                            else if (!currentWorkoutSportTypes["kardio"] && currentWorkoutSportTypes["hardWork"])
                                kardioExist.Visibility = System.Windows.Visibility.Hidden;
                            else if (!currentWorkoutSportTypes["kardio"] && !currentWorkoutSportTypes["hardWork"])
                            {
                                hardWorkExist.Visibility = System.Windows.Visibility.Hidden;
                                kardioExist.Visibility = System.Windows.Visibility.Hidden;
                            }
                        }
                    }
                    else
                    {
                        eventExist.Visibility = System.Windows.Visibility.Hidden;
                        hardWorkExist.Visibility = System.Windows.Visibility.Hidden;
                        kardioExist.Visibility = System.Windows.Visibility.Hidden;
                    }

                    
                    newGrid.Children.Add(weekDay);
                    newGrid.Children.Add(monthDay);
                    newGrid.Children.Add(eventExist);
                    newGrid.Children.Add(kardioExist);
                    newGrid.Children.Add(hardWorkExist);

                    lastGrid = newGrid;

                    gridCalender.Children.Add(newGrid);

                    gridCalender.Width = newGrid.Margin.Left + GRID_TEMPLATE_MARGIN_RIGHT;
                    count++;
                }
                //labelMonth.Margin = new Thickness(gridCalender.Width / 2, labelMonth.Margin.Top, labelMonth.Margin.Right, labelMonth.Margin.Bottom);
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }

        }
        /// <summary>
        /// Получить дату кликнутой лейблы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void labelDay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                FillAllCalenderLables((DateTime)((Label)sender).DataContext);

            }
        }
        /// <summary>
        /// Показывает содержимое тренировки дня, по которому кликнули два раза
        /// </summary>
        public void labelDay_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            clickedDate = (DateTime)((Label)sender).DataContext;
            //WorkoutViewWindow wvw = new WorkoutViewWindow(DEFAULT_FONT, DEFAULT_FONT_SIZE, format, gridIdSeparator, (DateTime)((Label)sender).DataContext);
            //wvw.ShowDialog();
        }
        /// <summary>
        /// Возвращает первую видимую дату календаря
        /// </summary>
        /// <returns></returns>
        public DateTime GetFirstCalenderDate()
        {
            foreach (object ob in gridCalender.Children)
            {
                if (ob is Grid)
                {
                    foreach (object o in ((Grid)ob).Children)
                    {
                        if (o is Label)
                        {
                            if (((Label)o).Name.IndexOf("labelDay_") != -1)
                            {
                                return (DateTime)((Label)o).DataContext;
                            }
                        }
                    }
                }
            }
            return new DateTime();
        }

        /// <summary>
        /// Прокручивает даты календая влево
        /// </summary>
        public void ScrollCalenderDatesLeft(object sender, MouseButtonEventArgs e)
        {
            //пока не придумал, насколько буду прокручивать, поэтому пусть будет на число выводимых дней,
            DateTime firstDate = GetFirstCalenderDate();
            firstDate = firstDate.Subtract(new TimeSpan(COUNT_DAYS, 0, 0, 0));
            FillAllCalenderLables(firstDate);
        }

        /// <summary>
        /// Прокручивает даты календая вправо
        /// </summary>
        public void ScrollCalenderDatesRight(object sender, MouseButtonEventArgs e)
        {
            //пока не придумал, насколько буду прокручивать, поэтому пусть будет на число выводимых дней
            DateTime firstDate = GetFirstCalenderDate();
            firstDate = firstDate.Add(new TimeSpan(COUNT_DAYS, 0, 0, 0));
            FillAllCalenderLables(firstDate);
        }
        /// <summary>
        /// Удаляет все даты календаря
        /// </summary>
        public void RemoveAllCalenderLabels()
        {
            for (int i = 0; i < gridCalender.Children.Count; i++)
            {
                if (gridCalender.Children[i] is Grid)
                {
                    if (((Grid)gridCalender.Children[i]).Name != "gridDayTemplate")
                    {
                        gridCalender.Children.Remove(((Grid)gridCalender.Children[i]));
                        i--;
                    }
                }
            }
        }
        /// <summary>
        /// При захождении курсора мыши в область календаря 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void gridCalender_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        public void gridCalender_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            selectedDate = e.Delta > 0 ? selectedDate.Add(new TimeSpan(COUNT_DAY_WHEN_MOUSE_SCROLL, 0, 0, 0)) : selectedDate.Subtract(new TimeSpan(COUNT_DAY_WHEN_MOUSE_SCROLL, 0, 0, 0));
            FillAllCalenderLables(selectedDate);
        }

        public void labelMonthNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(1.ToString());
        }

        /// <summary>
        /// Загружает всю работу, выполненную в кликнутый день
        /// </summary>
        /// <param name="date"></param>
        public void GetAllWorkOfThisWorkout(DateTime date, ListBox listBoxKardio, ListBox listBoxHardWork, Label labelWorkoutDate, Label labelMusclesGroup)
        {
            if (date != null)
            {
                listBoxHardWork.Items.Clear();
                listBoxKardio.Items.Clear();


                List<Workout> workouts = sqlite.GetWorkouts(date);
                if (workouts.Count != 0)
                {
                    labelWorkoutDate.Content = string.Format("{0} {1}.{2}.{3}", om.DaysWeek[date.DayOfWeek.ToString() + "_"], OtherMethods.IntToBinaryString(date.Day), date.Month, date.Year);
                    labelMusclesGroup.Content = string.Format("Группа мыщц: {0}", sqlite.GetValueById(SQLite.TABLE_MUSCLES_GROUP, workouts[0].musclesGroupId));

                    foreach (Workout workout in workouts)
                    {
                        List<Work> works = sqlite.GetWorks(workout.id);
                        listBoxKardio.Items.Add(string.Format("Работа, выполненная в период с {0} по {1}:", workout.timeBegin, workout.timeEnd));
                        listBoxHardWork.Items.Add(string.Format("Работа, выполненная в период с {0} по {1}:", workout.timeBegin, workout.timeEnd));
                        foreach (Work work in works)
                        {
                            WorkType workType = sqlite.GetWorkType(work.workTypeId);
                            Result result = sqlite.GetResult(work.resultId, format);
                            if (result != null)
                            {
                                if (workType.sportTypeId == 1)
                                {
                                    if (work.comment != "")
                                        listBoxKardio.Items.Add(string.Format("{0}: {1} ({2})", workType.value, result.time, work.comment));
                                    else
                                        listBoxKardio.Items.Add(string.Format("{0}: {1}", workType.value, result.time));
                                }
                                else //предусмотрели наличие не только двух типов видов спорта
                                {
                                    if (result.repeat != 0 && work.comment != "")
                                        listBoxHardWork.Items.Add(string.Format("{0}: {1} кг х{2} раз ({3})", workType.value, result.weight, result.repeat, work.comment));
                                    else
                                        listBoxHardWork.Items.Add(string.Format("{0}: {1} кг х{2} раз", workType.value, result.weight, result.repeat));
                                }
                            }
                            else
                            {
                                if (work.comment != "")
                                    listBoxKardio.Items.Add(string.Format("{0} ({2})", workType.value, work.comment));
                                else
                                    listBoxKardio.Items.Add(string.Format("{0}", workType.value));
                            }
                        }
                    }

                }
                else
                {

                    Messages.Warning("Информация отсутствует!", "На данную дату в базе данных отсутсвует какая-либо информация.");
                }
            }
        }

        private void labelWeekDay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void labelWeekDay_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void buttonNext_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void buttonNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ScrollCalenderDatesRight(sender, e);
        }
    }
}
