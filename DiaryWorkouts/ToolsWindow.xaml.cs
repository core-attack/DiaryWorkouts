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
using DiaryWorkouts.BaseClasses;
using DiaryWorkouts.DataBase;
using DiaryWorkouts.ReferenceBooks;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for ToolsWindow.xaml
    /// </summary>
    public partial class ToolsWindow : Window
    {
        /// <summary>
        /// Наименование заполняемых пользователем справочников (ниже вызываются по индексу!)
        /// </summary>
        string[] referencesNames = { "Виды спорта", "Группы мышц", "Типы работ", "Типы тренировок", "Типы планов тренировок"};
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        /// <summary>
        /// Значение по умолчанию в текстбоксе веса силовой работы
        /// </summary>
        string DEFAULT_VALUE_TEXTBOX_WEIGHT;
        /// <summary>
        /// Значение по умолчанию в текстбоксе повторений силовой работы
        /// </summary>
        string DEFAULT_VALUE_TEXTBOX_REPEAT;
        /// <summary>
        /// Значение по умолчанию в текстбоксе комментарий
        /// </summary>
        string DEFAULT_VALUE_TEXTBOX_COMMENT;
        /// <summary>
        /// Значение по умолчанию в текстбоксе время
        /// </summary>
        string DEFAULT_VALUE_TEXTBOX_TIME;
        /// <summary>
        /// Цвет подсказок в текстбоксах
        /// </summary>
        Color DEFAULT_TEXTBOX_COLOR_TOOLTIP = Color.FromRgb((byte)160, (byte)155, (byte)155);
        /// <summary>
        /// Цвет набираемого текста
        /// </summary>
        Color DEFAULT_TEXTBOX_COLOR_TEXT = Color.FromRgb((byte)0, (byte)0, (byte)0);
        /// <summary>
        /// Стиль подсказок в текстбоксах
        /// </summary>
        FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP = FontStyles.Italic;
        /// <summary>
        /// Стиль текста в текстбоксах
        /// </summary>
        FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TEXT = FontStyles.Normal;
        /// <summary>
        /// Разделитель весов тяжелых работ
        /// </summary>
        char weightsSeparator; 
        /// <summary>
        /// Разделитель повторов тяжелых работ
        /// </summary>
        char repeatsSeparator;
        /// <summary>
        /// Разделители составляющих времени кардио работы
        /// </summary>
        char[] timeSeparators;
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        /// <summary>
        /// Текст для лейблы при невыбранном плане тренировок
        /// </summary>
        const string NOT_SELECTED_WORKOUT_PLAN = "Тренировочный план не выбран";
        /// <summary>
        /// Текст для лейблы при выбранном плане тренировок
        /// </summary>
        const string SELECTED_WORKOUT_PLAN = "Загружается план {0}";
        /// <summary>
        /// Максимальное количество файлов в директории
        /// </summary>
        int MAX_COUNT_FILES_IN_DIR;

        SQLite sqlite = new SQLite();
        public ToolsWindow(long userId, 
                           System.Globalization.NumberFormatInfo format,
                           string DEFAULT_VALUE_TEXTBOX_WEIGHT,
                           string DEFAULT_VALUE_TEXTBOX_REPEAT,
                           string DEFAULT_VALUE_TEXTBOX_COMMENT,
                           string DEFAULT_VALUE_TEXTBOX_TIME,
                           char weightsSeparator,
                           char repeatsSeparator,
                           char[] timeSeparators,
                           Color DEFAULT_TEXTBOX_COLOR_TOOLTIP,
                           Color DEFAULT_TEXTBOX_COLOR_TEXT,
                           FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP,
                           FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TEXT,
                           int MAX_COUNT_FILES_IN_DIR)
        {
            InitializeComponent();

            this.userId = userId;
            this.format = format;
            this.DEFAULT_VALUE_TEXTBOX_WEIGHT = DEFAULT_VALUE_TEXTBOX_WEIGHT;
            this.DEFAULT_VALUE_TEXTBOX_REPEAT = DEFAULT_VALUE_TEXTBOX_REPEAT;
            this.DEFAULT_VALUE_TEXTBOX_COMMENT = DEFAULT_VALUE_TEXTBOX_COMMENT;
            this.DEFAULT_VALUE_TEXTBOX_TIME = DEFAULT_VALUE_TEXTBOX_TIME;
            this.weightsSeparator = weightsSeparator;
            this.repeatsSeparator = repeatsSeparator;
            this.timeSeparators = timeSeparators;
            this.DEFAULT_TEXTBOX_COLOR_TOOLTIP = DEFAULT_TEXTBOX_COLOR_TOOLTIP;
            this.DEFAULT_TEXTBOX_COLOR_TEXT = DEFAULT_TEXTBOX_COLOR_TEXT;
            this.DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
            this.DEFAULT_TEXTBOX_FONTSTYLE_TEXT = DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
            this.MAX_COUNT_FILES_IN_DIR = MAX_COUNT_FILES_IN_DIR;
            sqlite.Connect();
            SetAllReferenceBooks();
            comboBoxReferenceBook.SelectedIndex = 2;
            comboBoxReferenceBook.Text = referencesNames[2];
            SetAllValuesToListBox();
            List<SportType> sportTypes = sqlite.GetAllSportTypes();
            comboBoxSportTypes.Items.Clear();
            foreach (SportType st in sportTypes)
                comboBoxSportTypes.Items.Add(st.value);

            SetCurrentWorkoutPlan();

            textBoxDefaultStartTime.Text = sqlite.GetDefaultWorkoutsStartDate();
            textBoxDefaultEndTime.Text = sqlite.GetDefaultWorkoutsEndDate();
        }
        //выбирает текущий план тренировок
        private void SetCurrentWorkoutPlan()
        {
            WorkoutPlan wp = sqlite.GetCurrentWorkoutPlan();
            if (wp != null)
            {
                labelCurrentWorkoutPlan.Content = string.Format(SELECTED_WORKOUT_PLAN, wp.title);
                comboBoxCurrentWorkoutPlan.Text = wp.title;
            }
            else
                labelCurrentWorkoutPlan.Content = NOT_SELECTED_WORKOUT_PLAN;

            List<WorkoutPlan> wps = sqlite.GetAllWorkoutPlan();
            comboBoxCurrentWorkoutPlan.Items.Clear();
            foreach (WorkoutPlan w in wps)
                comboBoxCurrentWorkoutPlan.Items.Add(w.title);
        }

        private void buttonShowTables_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow(userId, format);
            adminWindow.Show();
        }

        private void buttonAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow(userId);
            addUserWindow.Show();
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow importWindow = new ImportWindow(userId);
            importWindow.Show();
        }

        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            SQLite sqlite = new SQLite();
            sqlite.Connect();
            sqlite.ExportDB("", format, MAX_COUNT_FILES_IN_DIR);
            sqlite.Disconnect();
        }
        /// <summary>
        /// Заполняет наименованиями всех справочников комбобокс
        /// </summary>
        private void SetAllReferenceBooks()
        {
            comboBoxReferenceBook.Items.Clear();
            foreach(string title in referencesNames)
                comboBoxReferenceBook.Items.Add(title);
        }
        /// <summary>
        /// Заполняет листбокс значениями из выбранного справочника 
        /// </summary>
        private void SetAllValuesToListBox()
        {
            listBoxReferenceBookValues.Items.Clear();
            comboBoxSportTypes.IsEnabled = false;
            if (comboBoxReferenceBook.Text.Equals(referencesNames[0]))
            {
                List<SportType> values = sqlite.GetAllSportTypes();
                foreach (SportType v in values)
                    listBoxReferenceBookValues.Items.Add(v.value);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[1]))
            {
                List<MusclesGroup> values = sqlite.GetAllMusclesGroups();
                foreach (MusclesGroup v in values)
                    listBoxReferenceBookValues.Items.Add(v.value);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[2]))
            {
                comboBoxSportTypes.IsEnabled = true;
                List<WorkType> values = sqlite.GetAllWorkTypes();
                foreach (WorkType v in values)
                    listBoxReferenceBookValues.Items.Add(v.value);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[3]))
            {
                List<WorkoutType> values = sqlite.GetAllWorkoutTypes();
                foreach (WorkoutType v in values)
                {
                    if (v.id != 4)
                        listBoxReferenceBookValues.Items.Add(v.value);
                }
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[4]))
            {
                List<WorkoutPlanType> values = sqlite.GetAllWorkoutPlanTypes();
                foreach (WorkoutPlanType v in values)
                {
                    listBoxReferenceBookValues.Items.Add(v.value);
                }
            }

        }

        private void comboBoxReferenceBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxReferenceBook.Text.Equals(referencesNames[0]))
            {
                SportType st = new SportType(0, textBoxReferenceBookAddValue.Text);
                sqlite.AddSportType(st);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[1]))
            {
                MusclesGroup mg = new MusclesGroup(0, textBoxReferenceBookAddValue.Text);
                sqlite.AddMusclesGroup(mg);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[2]))
            {
                byte sportTypeId = (byte)sqlite.GetIdByValue(SQLite.TABLE_SPORT_TYPE, comboBoxSportTypes.Text);
                WorkType wt = new WorkType(0, sportTypeId, textBoxReferenceBookAddValue.Text, userId);
                sqlite.AddWorkType(wt);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[3]))
            {
                WorkoutType wt = new WorkoutType(0, textBoxReferenceBookAddValue.Text);
                sqlite.AddWorkoutType(wt);
            }
            else if (comboBoxReferenceBook.Text.Equals(referencesNames[4]))
            {
                WorkoutPlanType wt = new WorkoutPlanType(0, textBoxReferenceBookAddValue.Text);
                sqlite.AddWorkoutPlanType(wt);
            }
            SetAllValuesToListBox();
        }

        private void comboBoxReferenceBook_GotFocus(object sender, RoutedEventArgs e)
        {
            SetAllValuesToListBox();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tb2.Text = OtherMethods.GetTime(OtherMethods.GetTime(tb1.Text));
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sqlite.InsertLastWorkouts();
        }

        private void comboBoxCurrentWorkoutPlan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void buttonCurrentWorkoutPlanSet_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCurrentWorkoutPlan.Text != "")
            {
                WorkoutPlan w = sqlite.GetWorkoutPlan(comboBoxCurrentWorkoutPlan.Text);
                if (w != null)
                {
                    labelCurrentWorkoutPlan.Content = string.Format(SELECTED_WORKOUT_PLAN, w.title);
                    sqlite.SetOptionCurrentWorkoutPlan(w.id);
                }
            }
            else
            {
                Messages.Warning("Ошибка", "Выберите тренировочный план!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //метод для быстрого изменения необходимых полей в БД (убрать когда будет релиз приложения)
            sqlite.createNewTable();
        }

        private void buttonSQLConsole_Click(object sender, RoutedEventArgs e)
        {
            SQLWindow sqlw = new SQLWindow();
            sqlw.ShowDialog();
        }

        private void textBoxDefaultEndTimeHour_KeyDown(object sender, KeyEventArgs e)
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

        private void buttonUpdateDefaultTimeOfWorkout_Click(object sender, RoutedEventArgs e)
        {
            sqlite.UpdateOptionWorkoutStartTime(textBoxDefaultStartTime.Text);
            sqlite.UpdateOptionWorkoutEndTime(textBoxDefaultEndTime.Text);
        }

        private void textBoxDefaultStartTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string mask = "0123456789";
            e.Handled = mask.IndexOf(e.Text) < 0;
        }
    }
}
