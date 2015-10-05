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

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for CalenderUserControl.xaml
    /// </summary>
    public partial class CalenderUserControl : UserControl
    {
        /// <summary>
        /// Ширина клетки календаря
        /// </summary>
        const int CELL_WIDTH = 27;
        /// <summary>
        /// Высота клетки календаря
        /// </summary>
        const int CELL_HEIGHT = 27;
        /// <summary>
        /// Отступ от клетки справа
        /// </summary>
        const int CELL_PADDING_RIGHT = 27;
        /// <summary>
        /// Отступ от клетки сверху
        /// </summary>
        const int CELL_PADDING_TOP = 27;
        /// <summary>
        /// Количество клеток в строке
        /// </summary>
        const byte COUNT_CELLS_IN_ROW = 7;
        /// <summary>
        /// Количество клеток в столбце 
        /// </summary>
        const byte COUNT_CELLS_IN_COLUMN = 7;
        /// <summary>
        /// Текущий день
        /// </summary>
        Brush COLOR_CURRENT_DAY = new SolidColorBrush(Color.FromRgb(203, 232, 246));
        /// <summary>
        /// Записанные тренировки
        /// </summary>
        Brush COLOR_EXISTING_WORKOUT = new SolidColorBrush(Color.FromRgb(214, 252, 131));
        /// <summary>
        /// Подсветка дня при наведении курсора
        /// </summary>
        Brush COLOR_MOUSE_UP = new SolidColorBrush(Color.FromRgb(229, 243, 251));
        /// <summary>
        /// Подсветка дня при наведении курсора
        /// </summary>
        Brush COLOR_WEEKEND_DAY = new SolidColorBrush(Color.FromRgb(252, 194, 131));
        /// <summary>
        /// Кириллические месяцы
        /// </summary>
        Dictionary<byte, string> Monthes = new Dictionary<byte, string>();
        /// <summary>
        /// Кириллические дни недели
        /// </summary>
        Dictionary<string, string> DaysWeek = new Dictionary<string, string>();
        /// <summary>
        /// Разделители составляющих времени кардио работы
        /// </summary>
        char[] timeSeparators = new char[] { '.', ':' };
        /// <summary>
        /// Индексы, начиная с нуля, дней недели
        /// </summary>
        byte[] weekendDays = { 3, 6 };
        /// <summary>
        /// Текущая дата календаря
        /// </summary>
        public DateTime selectedDate;
        public CalenderUserControl()
        {
            InitializeComponent();
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
            DaysWeek.Add("Monday_", "Понедельник");
            DaysWeek.Add("Tuesday_", "Вторник");
            DaysWeek.Add("Wednesday_", "Среда");
            DaysWeek.Add("Thursday_", "Четверг");
            DaysWeek.Add("Friday_", "Пятница");
            DaysWeek.Add("Saturday_", "Суббота");
            DaysWeek.Add("Sunday_", "Воскресенье");

            DaysWeek.Add("Monday", "Пн");
            DaysWeek.Add("Tuesday", "Вт");
            DaysWeek.Add("Wednesday", "Ср");
            DaysWeek.Add("Thursday", "Чт");
            DaysWeek.Add("Friday", "Пт");
            DaysWeek.Add("Saturday", "Сб");
            DaysWeek.Add("Sunday", "Вс");

            buttonAfterMonth.Click += buttonAfterMonth_Click;
            buttonBeforeMonth.Click += buttonBeforeMonth_Click;

            FillAllCalenderCells(DateTime.Now);
            UpdateSelectedDate();
        }
        /// <summary>
        /// Прокрутка календаря на месяц назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAfterMonth_Click(object sender, EventArgs e)
        {
            if (gridDays.DataContext is DateTime)
                FillAllCalenderCells(((DateTime)gridDays.DataContext).AddMonths(1));//(new TimeSpan(GetCountOfMonth(((DateTime)gridDays.DataContext)), 0, 0, 0)));
            else
                FillAllCalenderCells(DateTime.Now);
        }
        /// <summary>
        /// Прокрутка календаря на месяц вперед
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBeforeMonth_Click(object sender, EventArgs e)
        {
            if (gridDays.DataContext is DateTime)
                FillAllCalenderCells(((DateTime)gridDays.DataContext).AddMonths(-1)); //Subtract(new TimeSpan(GetCountOfMonth(((DateTime)gridDays.DataContext)), 0, 0, 0)));
            else
                FillAllCalenderCells(DateTime.Now);
        }
        /// <summary>
        /// Обновляет текущую дату
        /// </summary>
        public void UpdateSelectedDate()
        {
            DateTime now = DateTime.Now;
            selectedDate = now;
            LabelMonth.Content = Monthes[(byte)now.Month];
            LabelDayDate.Content = now.Day;
            LabelYear.Content = now.Year;
            LabelDayWeekDate.Content = DaysWeek[now.DayOfWeek.ToString() + "_"];

        }
        /// <summary>
        /// Обновляет текущую дату
        /// </summary>
        public void UpdateSelectedDate(DateTime date)
        {
            selectedDate = date;
            LabelMonth.Content = Monthes[(byte)date.Month];
            LabelDayDate.Content = date.Day;
            LabelYear.Content = date.Year;
            LabelDayWeekDate.Content = DaysWeek[date.DayOfWeek.ToString() + "_"];
        }
        /// <summary>
        /// Инкрементирование и декрементирование минут в текстбоксе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (sender is TextBox)
                {
                    if (((TextBox)sender).Text.Length <= 5)
                    {
                        //00:00
                        switch (((TextBox)sender).Text.Length)
                        {
                            case 2: { ((TextBox)sender).Text += ":"; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
                                break;
                        }

                        //увеличение соотвествующей составляющей времени
                        if (e.Key == Key.Up)
                        {
                            int caretIndex = ((TextBox)sender).CaretIndex;
                            string[] values = ((TextBox)sender).Text.Split(timeSeparators);
                            int value = 0;
                            if (caretIndex <= 2)//часы
                            {
                                value = int.Parse(values[0]);
                                if (value < 23)
                                {
                                    value++;
                                    values[0] = OtherMethods.IntToBinaryString(value);
                                }
                                else
                                    values[0] = "00";
                            }
                            else if (caretIndex <= 5)//минуты
                            {
                                value = int.Parse(values[1]);
                                if (value < 59)
                                {
                                    value += 1;
                                    values[1] = OtherMethods.IntToBinaryString(value);
                                }
                                else
                                    values[1] = "00";
                            }
                            ((TextBox)sender).Text = string.Format("{0}:{1}", values[0], values[1]);
                            ((TextBox)sender).CaretIndex = caretIndex;
                        }
                        else if (e.Key == Key.Down)
                        {
                            int caretIndex = ((TextBox)sender).CaretIndex;
                            string[] values = ((TextBox)sender).Text.Split(timeSeparators);
                            int value = 0;
                            if (caretIndex <= 2)//часы
                            {
                                value = int.Parse(values[0]);
                                if (value > 0)
                                {
                                    value--;
                                    values[0] = OtherMethods.IntToBinaryString(value);
                                }
                                else
                                    values[0] = "23";
                            }
                            else if (caretIndex <= 5)//минуты
                            {
                                value = int.Parse(values[1]);
                                if (value > 0)
                                {
                                    value -= 1;
                                    values[1] = OtherMethods.IntToBinaryString(value);
                                }
                                else
                                    values[1] = "59";
                            }
                            ((TextBox)sender).Text = string.Format("{0}:{1}", values[0], values[1]);
                            ((TextBox)sender).CaretIndex = caretIndex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        /// <summary>
        /// Заполняет все клетки календаря
        /// </summary>
        /// <param name="currentDate">Дата, которая считается текущей</param>
        private void FillAllCalenderCells(DateTime currentDate)
        {
            //UpdateDate(currentDate);
            RemoveAllLables();
            gridDays.DataContext = currentDate;
            DateTime first = new DateTime(currentDate.Year, currentDate.Month, 1);
            byte column = 0;
            column = GetNumberOfWeekDay(first);
            //Первый день календаря получается при вычитании из общего количества дней текущей даты
            int days = 7 - (7 - column) - 1;
            DateTime firstDay = first.Subtract(new TimeSpan(days == 0 ? 7 : days, 0, 0, 0));
            int count = 0;
            for (int i = 0; i < COUNT_CELLS_IN_COLUMN; i++)
            {
                for (int j = 0; j < COUNT_CELLS_IN_ROW; j++)
                {
                    Label newLabel = new Label();
                    newLabel.Name = "day_" + count;
                    newLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    newLabel.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    newLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    newLabel.Width = CELL_WIDTH;
                    newLabel.Height = CELL_HEIGHT;
                    newLabel.Margin = new Thickness(j * CELL_PADDING_RIGHT, i * CELL_PADDING_TOP, 0, 0);
                    
                    if (j == 3 || j == 6)
                        newLabel.Background = COLOR_WEEKEND_DAY;

                    if (i == 0) // наименование дней недели
                    {
                        switch (j)
                        {
                            case 0: { newLabel.Content = DaysWeek["Monday"]; } break;
                            case 1: { newLabel.Content = DaysWeek["Tuesday"]; } break;
                            case 2: { newLabel.Content = DaysWeek["Wednesday"]; } break;
                            case 3: { newLabel.Content = DaysWeek["Thursday"]; } break;
                            case 4: { newLabel.Content = DaysWeek["Friday"]; } break;
                            case 5: { newLabel.Content = DaysWeek["Saturday"]; } break;
                            case 6: { newLabel.Content = DaysWeek["Sunday"]; } break;

                        }
                    }
                    else
                    {
                        DateTime date = firstDay.Add(new TimeSpan(count, 0, 0, 0));
                        newLabel.Content = OtherMethods.IntToBinaryString(date.Day);
                        newLabel.DataContext = date;
                        newLabel.MouseUp += CellMouseClick;

                        DateTime n = DateTime.Now;
                        if (date.Day == n.Day && date.Month == n.Month && date.Year == n.Year)
                        {
                            newLabel.Background = COLOR_CURRENT_DAY;
                        }
                        count++;

                    }

                    gridDays.Children.Add(newLabel);

                }
            }
        }
        /// <summary>
        /// Реакция на клик по лейбле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellMouseClick(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                UpdateSelectedDate(((DateTime)((Label)sender).DataContext));
            }
        }

        private byte GetNumberOfWeekDay(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday: { return 1; } 
                case DayOfWeek.Tuesday: { return 2; } 
                case DayOfWeek.Wednesday: { return 3; } 
                case DayOfWeek.Thursday: { return 4; } 
                case DayOfWeek.Friday: { return 5; } 
                case DayOfWeek.Saturday: { return 6; } 
                case DayOfWeek.Sunday: { return 7; }
                default: return 0;
            }
        }
        /// <summary>
        /// Возвращает количество дней в текущем месяце
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetCountOfMonth(DateTime date)
        {
            switch (date.Month)
            {
                case 1: return 31; 
                case 2: return isViskonti(date.Year) ? 29 : 28; 
                case 3: return 31; 
                case 4: return 30; 
                case 5: return 31; 
                case 6: return 30; 
                case 7: return 31; 
                case 8: return 31; 
                case 9: return 30; 
                case 10: return 31;
                case 11: return 30;
                case 12: return 31;
                default: return 0;
            }
        }
        /// <summary>
        /// Проверка года на високосность
        /// </summary>
        /// <returns></returns>
        private bool isViskonti(int year)
        {
            if (year % 4 == 0)
            {
                if (year % 400 == 0)
                    return true;
                else if (year % 100 == 0 )
                    return false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Удаляет клетки дат с календаря
        /// </summary>
        private void RemoveAllLables()
        {
            for (int i = 0; i < gridDays.Children.Count; i++)
            {
                if (gridDays.Children[i] is Label)
                {
                    gridDays.Children.Remove(((Label)gridDays.Children[i]));
                    i--;
                }
            }
        }
    }
}
