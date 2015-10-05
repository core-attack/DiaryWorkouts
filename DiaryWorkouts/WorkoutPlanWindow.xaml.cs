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
using System.IO;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for WorkoutPlanWindow.xaml
    /// </summary>
    public partial class WorkoutPlanWindow : Window
    {
        /// <summary>
        /// Папка с планами тренировок
        /// </summary>
        const string EXPORT_WORKOUT_PLAN_DIRECTORY_NAME = "workout plan export";
        /// <summary>
        /// Шаг отступа при добавлении новой силовой работы
        /// </summary>
        const double HARD_WORK_STEP = 25;
        /// <summary>
        /// Шаг прокрутки работ
        /// </summary>
        const double HARD_WORK_SCROLL_STEP = 25;
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
        /// Разделитель одинаковых работ (6х40)
        /// </summary>
        char[] equalRecordsSeparator = { 'x', 'х' };
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        /// <summary>
        /// Количество силовых работ
        /// </summary>
        int countHardWorks;
        /// <summary>
        /// Последняя добавленная силовая
        /// </summary>
        Grid lastAddedHardWork = null;
        /// <summary>
        /// Текст кнопки добавления силовой работы
        /// </summary>
        string BUTTON_ADD_HARD_WORK_CONTENT;
        /// <summary>
        /// Текст кнопки удаления силовой работы
        /// </summary>
        string BUTTON_DELETE_HARD_WORK_CONTENT;
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId = 0;
        /// <summary>
        /// Максимальное колчество файлов в директориях
        /// </summary>
        int MAX_COUNT_FILES_IN_DIR;
        /// <summary>
        /// Разделитель идетификатора грида от его имени
        /// </summary>
        char gridIdSeparator;
        /// <summary>
        /// Прошла ли инициализация
        /// </summary>
        bool isInit = false;
        SQLite sqlite = new SQLite();
        public WorkoutPlanWindow(long userId, 
                           System.Globalization.NumberFormatInfo format,
                           string DEFAULT_VALUE_TEXTBOX_WEIGHT,
                           string DEFAULT_VALUE_TEXTBOX_REPEAT,
                           string DEFAULT_VALUE_TEXTBOX_COMMENT,
                           string DEFAULT_VALUE_TEXTBOX_TIME,
                           char weightsSeparator,
                           char repeatsSeparator,
                           char[] equalRecordsSeparator,
                           char[] timeSeparators,
                           char gridIdSeparator,
                           Color DEFAULT_TEXTBOX_COLOR_TOOLTIP,
                           Color DEFAULT_TEXTBOX_COLOR_TEXT,
                           FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP,
                           FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TEXT,
                           string BUTTON_ADD_HARD_WORK_CONTENT,
                           string BUTTON_DELETE_HARD_WORK_CONTENT,
                           int MAX_COUNT_FILES_IN_DIR)
        {
            InitializeComponent();
            isInit = true;

            this.userId = userId;
            this.format = format;
            this.DEFAULT_VALUE_TEXTBOX_WEIGHT = DEFAULT_VALUE_TEXTBOX_WEIGHT;
            this.DEFAULT_VALUE_TEXTBOX_REPEAT = DEFAULT_VALUE_TEXTBOX_REPEAT;
            this.DEFAULT_VALUE_TEXTBOX_COMMENT = DEFAULT_VALUE_TEXTBOX_COMMENT;
            this.DEFAULT_VALUE_TEXTBOX_TIME = DEFAULT_VALUE_TEXTBOX_TIME;
            this.weightsSeparator = weightsSeparator;
            this.repeatsSeparator = repeatsSeparator;
            this.equalRecordsSeparator = equalRecordsSeparator;
            this.timeSeparators = timeSeparators;
            this.gridIdSeparator = gridIdSeparator;
            this.DEFAULT_TEXTBOX_COLOR_TOOLTIP = DEFAULT_TEXTBOX_COLOR_TOOLTIP;
            this.DEFAULT_TEXTBOX_COLOR_TEXT = DEFAULT_TEXTBOX_COLOR_TEXT;
            this.DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
            this.DEFAULT_TEXTBOX_FONTSTYLE_TEXT = DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
            this.BUTTON_ADD_HARD_WORK_CONTENT = BUTTON_ADD_HARD_WORK_CONTENT;
            this.BUTTON_DELETE_HARD_WORK_CONTENT = BUTTON_DELETE_HARD_WORK_CONTENT;
            this.MAX_COUNT_FILES_IN_DIR = MAX_COUNT_FILES_IN_DIR;
            sqlite.Connect();

            Init();
        }
        /// <summary>
        /// Инициализация пользовательских элементов управления
        /// </summary>
        private void Init()
        {
            buttonAddWorkoutPlanRecord.Content = BUTTON_ADD_HARD_WORK_CONTENT;
            buttonDeleteWorkoutPlanRecord.Content = BUTTON_DELETE_HARD_WORK_CONTENT;

            gridWorkoutPlanRecordTemplate.Visibility = System.Windows.Visibility.Hidden;

            List<MusclesGroup> musclesGroups = sqlite.GetAllMusclesGroups();
            comboBoxWorkoutPlanMusclesGroup.Items.Clear();
            foreach (MusclesGroup mg in musclesGroups)
                comboBoxWorkoutPlanMusclesGroup.Items.Add(mg.value);

            List<WorkType> workTypes = sqlite.GetAllWorkTypesHardWork();
            comboBoxWorkoutPlanRecordHardWorks.Items.Clear();
            foreach (WorkType wt in workTypes)
                comboBoxWorkoutPlanRecordHardWorks.Items.Add(wt.value);

            List<WorkoutPlanType> workoutPlanTypes = sqlite.GetAllWorkoutPlanTypes();
            comboBoxWorkoutPlanType.Items.Clear();
            foreach (WorkoutPlanType wt in workoutPlanTypes)
                comboBoxWorkoutPlanType.Items.Add(wt.value);

            AddControlsOnHardWorkGrid("", "", "", "");
            LoadAllWorkoutPlans();

        }
        /// <summary>
        /// Загружает планы тренировок 
        /// </summary>
        private void LoadAllWorkoutPlans()
        {
            List<WorkoutPlan> wps = sqlite.GetAllWorkoutPlan();
            comboBoxWorkoutPlanTitle.Items.Clear();
            listBoxAllWorkoutPlans.Items.Clear();
            comboBoxWorkoutPlanTitleDelete.Items.Clear();

            foreach (WorkoutPlan w in wps)
            {
                comboBoxWorkoutPlanTitle.Items.Add(w.title);
                comboBoxWorkoutPlanTitleDelete.Items.Add(w.title);
                listBoxAllWorkoutPlans.Items.Add(string.Format("{0}. {1}. Периодичность: {2}.", w.id, w.title, OtherMethods.getCurrentFormOfWordDay(w.period.ToString())));
            }
        }

        private void textBoxWorkoutPlanTitle_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void textBoxWorkResultTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string mask = "0123456789 ";
            foreach (char c in timeSeparators)
                mask += c.ToString();
            e.Handled = mask.IndexOf(e.Text) < 0;
        }
        private void textBoxWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string otherSeparators = "";
            foreach (char c in equalRecordsSeparator)
                otherSeparators += c.ToString();
            otherSeparators += weightsSeparator.ToString();
            string mask = "0123456789 " + otherSeparators; ;
            e.Handled = mask.IndexOf(e.Text) < 0;
        }
        private void textBoxRepeat_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string otherSeparators = "";
            foreach (char c in equalRecordsSeparator)
                otherSeparators += c.ToString();
            otherSeparators += repeatsSeparator.ToString();
            string mask = "0123456789 " + otherSeparators;
            e.Handled = mask.IndexOf(e.Text) < 0;
        }

        private void textBoxWeight_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_WEIGHT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_REPEAT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_COMMENT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_TIME)
                {
                    ((TextBox)sender).Text = ((TextBox)sender).Text != DEFAULT_VALUE_TEXTBOX_TIME ? "" : DEFAULT_VALUE_TEXTBOX_TIME;
                    ((TextBox)sender).Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TEXT);
                    ((TextBox)sender).FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
                }
            }
        }
        private void textBoxWeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text == "" || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_TIME)
                {
                    ((TextBox)sender).Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                    ((TextBox)sender).FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                    if (((TextBox)sender).Name.IndexOf("textBoxWeight") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_WEIGHT;
                    else if (((TextBox)sender).Name.IndexOf("textBoxRepeat") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_REPEAT;
                    else if (((TextBox)sender).Name.IndexOf("textBoxHardWorkComment") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_COMMENT;
                    else if (((TextBox)sender).Name.IndexOf("textBoxWorkResultTime") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_TIME;
                }
            }
        }

        private void textBoxWeightAndRepeat_TextChanged(object sender, TextChangedEventArgs e)
        { }
        private void textBoxRepeat_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        /// <summary>
        /// Добавляет на панель плана тренировок запись плана тренировоки  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddWorkoutPlanRecord_Click(object sender, RoutedEventArgs e)
        {
            AddControlsOnHardWorkGrid("", "", "", "");
        }
        /// <summary>
        /// Записывает план тренировок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWorkoutPlanCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!sqlite.CheckWorkoutPlanTitle(textBoxWorkoutPlanTitle.Text))
                {
                    WorkoutPlan wp = new WorkoutPlan(textBoxWorkoutPlanTitle.Text, Convert.ToByte(textBoxWorkoutPlanPeriod.Text), userId);
                    sqlite.AddWorkoutPlan(wp);
                    LoadAllWorkoutPlans();
                    Messages.Info("План тренировок записан", "Запись плана тренировок успешно записана в базу данных!");
                }
                else
                    Messages.Warning("Не удалось записать план тренировок", "Создавать планы тренировок с одинаковыми названиями запрещено!");
            }
            catch(Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        /// <summary>
        /// Удаление записи упражнения в плане тренировки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteWorkoutPlanRecord_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                if (((Button)sender).Parent is Grid)
                {
                    if (((Button)sender).Parent is Grid)
                    {
                        if (((Grid)((Grid)((Button)sender).Parent).Parent).Children.Contains(((Grid)((Button)sender).Parent)))
                        {
                            byte number = GetNumberOfGrid(((Grid)((Button)sender).Parent));
                            ((Grid)((Grid)((Button)sender).Parent).Parent).Children.Remove(((Grid)((Button)sender).Parent));
                            if (countHardWorks > 0)
                                countHardWorks--;
                            RebuildGridHardWorks(number);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет на панель всех упражнений одно упражнение
        /// </summary>
        private void AddControlsOnHardWorkGrid(string hardWork, string weight, string repeat, string comment)
        {
            try
            {
                Grid newHardWork = new Grid();
                newHardWork.Name = gridWorkoutPlanRecordTemplate.Name + "_" + countHardWorks;
                newHardWork.Margin = new Thickness(gridWorkoutPlanRecordTemplate.Margin.Left, gridWorkoutPlanRecordTemplate.Margin.Top, gridWorkoutPlanRecordTemplate.Margin.Right, gridWorkoutPlanRecordTemplate.Margin.Bottom);

                Button newButtonDelete = new Button();
                newButtonDelete.Name = buttonDeleteWorkoutPlanRecord.Name + "_" + countHardWorks;
                newButtonDelete.Margin = new Thickness(buttonDeleteWorkoutPlanRecord.Margin.Left, buttonDeleteWorkoutPlanRecord.Margin.Top, buttonDeleteWorkoutPlanRecord.Margin.Right, buttonDeleteWorkoutPlanRecord.Margin.Bottom);
                newButtonDelete.Content = buttonDeleteWorkoutPlanRecord.Content;
                newButtonDelete.HorizontalAlignment = buttonDeleteWorkoutPlanRecord.HorizontalAlignment;
                newButtonDelete.VerticalAlignment = buttonDeleteWorkoutPlanRecord.VerticalAlignment;
                newButtonDelete.Width = buttonDeleteWorkoutPlanRecord.Width;
                newButtonDelete.Height = buttonDeleteWorkoutPlanRecord.Height;
                newButtonDelete.Click += buttonDeleteWorkoutPlanRecord_Click;

                ComboBox newComboBoxHardWork = new ComboBox();
                newComboBoxHardWork.Name = comboBoxWorkoutPlanRecordHardWorks.Name + "_" + countHardWorks;
                newComboBoxHardWork.Margin = new Thickness(comboBoxWorkoutPlanRecordHardWorks.Margin.Left, comboBoxWorkoutPlanRecordHardWorks.Margin.Top, comboBoxWorkoutPlanRecordHardWorks.Margin.Right, comboBoxWorkoutPlanRecordHardWorks.Margin.Bottom);
                foreach (string item in comboBoxWorkoutPlanRecordHardWorks.Items)
                    newComboBoxHardWork.Items.Add(item);
                newComboBoxHardWork.HorizontalAlignment = comboBoxWorkoutPlanRecordHardWorks.HorizontalAlignment;
                newComboBoxHardWork.VerticalAlignment = comboBoxWorkoutPlanRecordHardWorks.VerticalAlignment;
                newComboBoxHardWork.Width = comboBoxWorkoutPlanRecordHardWorks.Width;
                newComboBoxHardWork.Height = comboBoxWorkoutPlanRecordHardWorks.Height;
                newComboBoxHardWork.Text = hardWork;

                TextBox newTextBoxWeight = new TextBox();
                newTextBoxWeight.Name = textBoxWorkoutPlanRecordWeight.Name + "_" + countHardWorks;
                newTextBoxWeight.Margin = new Thickness(textBoxWorkoutPlanRecordWeight.Margin.Left, textBoxWorkoutPlanRecordWeight.Margin.Top, textBoxWorkoutPlanRecordWeight.Margin.Right, textBoxWorkoutPlanRecordWeight.Margin.Bottom);
                newTextBoxWeight.Text = weight == "" ? DEFAULT_VALUE_TEXTBOX_WEIGHT : weight;
                newTextBoxWeight.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxWeight.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxWeight.HorizontalAlignment = textBoxWorkoutPlanRecordWeight.HorizontalAlignment;
                newTextBoxWeight.VerticalAlignment = textBoxWorkoutPlanRecordWeight.VerticalAlignment;
                newTextBoxWeight.Width = textBoxWorkoutPlanRecordWeight.Width;
                newTextBoxWeight.Height = textBoxWorkoutPlanRecordWeight.Height;
                newTextBoxWeight.TextChanged += new TextChangedEventHandler(textBoxWeightAndRepeat_TextChanged);
                newTextBoxWeight.PreviewTextInput += textBoxWeight_PreviewTextInput;
                newTextBoxWeight.GotFocus += textBoxWeight_GotFocus;
                newTextBoxWeight.LostFocus += textBoxWeight_LostFocus;

                TextBox newTextBoxRepeat = new TextBox();
                newTextBoxRepeat.Name = textBoxWorkoutPlanRecordRepeat.Name + "_" + countHardWorks;
                newTextBoxRepeat.Margin = new Thickness(textBoxWorkoutPlanRecordRepeat.Margin.Left, textBoxWorkoutPlanRecordRepeat.Margin.Top, textBoxWorkoutPlanRecordRepeat.Margin.Right, textBoxWorkoutPlanRecordRepeat.Margin.Bottom);
                newTextBoxRepeat.Text = repeat == "" ? DEFAULT_VALUE_TEXTBOX_REPEAT : repeat;
                newTextBoxRepeat.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxRepeat.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxRepeat.HorizontalAlignment = textBoxWorkoutPlanRecordRepeat.HorizontalAlignment;
                newTextBoxRepeat.VerticalAlignment = textBoxWorkoutPlanRecordRepeat.VerticalAlignment;
                newTextBoxRepeat.Width = textBoxWorkoutPlanRecordRepeat.Width;
                newTextBoxRepeat.Height = textBoxWorkoutPlanRecordRepeat.Height;
                newTextBoxRepeat.TextChanged += new TextChangedEventHandler(textBoxWeightAndRepeat_TextChanged);
                newTextBoxRepeat.PreviewTextInput += textBoxRepeat_PreviewTextInput;
                newTextBoxRepeat.GotFocus += textBoxWeight_GotFocus;
                newTextBoxRepeat.LostFocus += textBoxWeight_LostFocus;

                TextBox newTextBoxHardWorkComment = new TextBox();
                newTextBoxHardWorkComment.Name = textBoxWorkoutPlanRecordComment.Name + "_" + countHardWorks;
                newTextBoxHardWorkComment.Margin = new Thickness(textBoxWorkoutPlanRecordComment.Margin.Left, textBoxWorkoutPlanRecordComment.Margin.Top, textBoxWorkoutPlanRecordComment.Margin.Right, textBoxWorkoutPlanRecordComment.Margin.Bottom);
                newTextBoxHardWorkComment.Text = comment == "" ? DEFAULT_VALUE_TEXTBOX_COMMENT : comment;
                newTextBoxHardWorkComment.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxHardWorkComment.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxHardWorkComment.HorizontalAlignment = textBoxWorkoutPlanRecordComment.HorizontalAlignment;
                newTextBoxHardWorkComment.VerticalAlignment = textBoxWorkoutPlanRecordComment.VerticalAlignment;
                newTextBoxHardWorkComment.Width = textBoxWorkoutPlanRecordComment.Width;
                newTextBoxHardWorkComment.Height = textBoxWorkoutPlanRecordComment.Height;
                newTextBoxHardWorkComment.GotFocus += textBoxWeight_GotFocus;
                newTextBoxHardWorkComment.LostFocus += textBoxWeight_LostFocus;

                newHardWork.Children.Add(newButtonDelete);
                newHardWork.Children.Add(newComboBoxHardWork);
                newHardWork.Children.Add(newTextBoxWeight);
                newHardWork.Children.Add(newTextBoxRepeat);
                newHardWork.Children.Add(newTextBoxHardWorkComment);

                if (countHardWorks != 0)
                {
                    newHardWork.Margin = new Thickness(newHardWork.Margin.Left, lastAddedHardWork.Margin.Top + HARD_WORK_STEP, newHardWork.Margin.Right, newHardWork.Margin.Bottom);
                }

                lastAddedHardWork = newHardWork;
                gridWorkoutPlanRecords.Children.Add(newHardWork);
                countHardWorks++;
                scrollBarWorkoutPlan.Maximum = countHardWorks * HARD_WORK_STEP;
                scrollBarWorkoutPlan.SmallChange = HARD_WORK_STEP;
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }
        /// <summary>
        /// Прокрутка силовых упражнений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBarHardWork_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (object ob in gridWorkoutPlanRecords.Children)
            {
                if (ob is Grid)
                {
                    if (((Grid)ob).Name.IndexOf(gridWorkoutPlanRecordTemplate.Name + "_") != -1)
                    {
                        ((Grid)ob).Margin = new Thickness(((Grid)ob).Margin.Left, e.NewValue >= e.OldValue ? ((Grid)ob).Margin.Top - HARD_WORK_SCROLL_STEP : ((Grid)ob).Margin.Top + HARD_WORK_SCROLL_STEP, ((Grid)ob).Margin.Right, ((Grid)ob).Margin.Bottom);
                    }
                }
            }
        }
        
        /// <summary>
        /// Поднимает вверх все гриды с работами на место удаленной
        /// </summary>
        /// <param name="number">Порядковый номер удаленной работы</param>
        private void RebuildGridHardWorks(byte deleted)
        {
            byte current = 0;
            foreach (object g in gridWorkoutPlanRecords.Children)
            {
                if (g is Grid)
                {
                    current = GetNumberOfGrid(((Grid)g));
                    if (current > deleted && current != 255)
                        if (((Grid)g).Margin != gridWorkoutPlanRecordTemplate.Margin)
                            ((Grid)g).Margin = new Thickness(((Grid)g).Margin.Left, ((Grid)g).Margin.Top - HARD_WORK_STEP, ((Grid)g).Margin.Right, ((Grid)g).Margin.Bottom);
                }
            }
        }
        /// <summary>
        /// Возвращает порядковый номер грида (для работы и силовой работы)
        /// </summary>
        /// <param name="g">Грид</param>
        /// <returns></returns>
        private byte GetNumberOfGrid(Grid g)
        {
            return g.Name.Split(gridIdSeparator).Length > 1 ? byte.Parse(g.Name.Split(gridIdSeparator)[1]) : (byte)255;
        }

        private void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            //Проверить, есть ли план тренировок, соответствующий указанным критериям
            if (sqlite.IsWorkoutOfWorkoutPlanExist(sqlite.GetWorkoutPlanId(comboBoxWorkoutPlanTitle.Text), 
                (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutPlanMusclesGroup.Text), 
                (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_PLAN_TYPE, comboBoxWorkoutPlanType.Text)))
            {
                UpdateWorkoutForWorkoutPlan();
            }
            else
                SaveWorkoutForWorkoutPlan();
        }

        /// <summary>
        /// Обновляет план тренировки в БД
        /// </summary>
        private void UpdateWorkoutForWorkoutPlan()
        { }

        /// <summary>
        /// Записывает план тренировки в БД
        /// </summary>
        private void SaveWorkoutForWorkoutPlan()
        {
            long newWorkoutId = -1;
            bool notMessagesShowed = true;
            try
            {
                OtherMethods.Debug("userId: " + userId.ToString());
                sqlite.AddWorkout(new Workout(userId, 
                    (short)4,//да, захардкожено, но я не знаю, как сделать иначе
                    (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutPlanMusclesGroup.Text),
                    DateTime.Now, 
                    new TimeSpan(), 
                    new TimeSpan(), 
                    0, 
                    sqlite.GetWorkoutPlanId(comboBoxWorkoutPlanTitle.Text), 
                    (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_PLAN_TYPE, comboBoxWorkoutPlanType.Text), 
                    DateTime.Now));
                newWorkoutId = sqlite.GetLastWorkout(userId).id;
                OtherMethods.Debug("newWorkoutId: " + newWorkoutId.ToString());
                string comment = "";
                bool ok = false;
                string hardWork = "";
                string weight = "";
                string repeat = "";
                foreach (object ob in gridWorkoutPlanRecords.Children)
                {
                    ok = false;
                    if (ob is Grid)
                    {
                        foreach (object obChild in ((Grid)ob).Children)
                        {
                            if (obChild is ComboBox)
                            {
                                if (((ComboBox)obChild).Name.IndexOf("comboBoxWorkoutPlanRecordHardWorks_") != -1)
                                {
                                    ok = true;
                                    hardWork = ((ComboBox)obChild).Text;
                                    OtherMethods.Debug("hard work: " + hardWork);
                                }
                            }
                            else if (obChild is TextBox)
                            {
                                if (((TextBox)obChild).Name.IndexOf("textBoxWorkoutPlanRecordWeight_") != -1)
                                {
                                    weight = ((TextBox)obChild).Text;
                                    OtherMethods.Debug("weight: " + weight);
                                }
                                else if (((TextBox)obChild).Name.IndexOf("textBoxWorkoutPlanRecordRepeat_") != -1)
                                {
                                    repeat = ((TextBox)obChild).Text;
                                    OtherMethods.Debug("repeat: " + repeat);
                                }
                                else if (((TextBox)obChild).Name.IndexOf("textBoxWorkoutPlanRecordComment_") != -1)
                                {
                                    if (((TextBox)obChild).Text != DEFAULT_VALUE_TEXTBOX_COMMENT)
                                    {
                                        comment = ((TextBox)obChild).Text;
                                        OtherMethods.Debug("comment: " + comment);
                                    }
                                }
                            }
                        }
                        if (ok)
                        {
                            //проверенный список всех весов
                            List<string> normalizeWeights = new List<string>();
                            //проверенный список всех повторов
                            List<string> normalizeRepeats = new List<string>();

                            string[] weights = new string[0];
                            string[] repeats = new string[0];
                            //в случае, если вес НЕ заполнен
                            if (weight == "")
                            {
                                for (int i = 0; i < normalizeRepeats.Count; i++)
                                    normalizeWeights.Add("0");
                            }
                            else if (weight == DEFAULT_VALUE_TEXTBOX_WEIGHT)
                            {
                                normalizeWeights.Add("0");
                            }
                            else
                                weights = weight.Split(weightsSeparator);//результаты всегда будут писаться только через запятую 

                            if (repeat == "")
                            {
                                for (int i = 0; i < normalizeWeights.Count; i++)
                                    normalizeRepeats.Add("0");
                            }
                            else if (repeat == DEFAULT_VALUE_TEXTBOX_REPEAT)
                            {
                            }
                            else
                                repeats = repeat.Split(repeatsSeparator); //результаты всегда будут писаться только через запятую 
                            //начинаем проверку весов на корректность
                            foreach (string w in weights)
                            {
                                if (w.IndexOfAny(equalRecordsSeparator) == -1)
                                {
                                    normalizeWeights.Add(w);
                                }
                                else //если есть специальный разделитель, то
                                {
                                    //нулевой элемент - количество повторений веса, а первый - сам вес
                                    string[] wSplited = w.Split(equalRecordsSeparator);
                                    if (wSplited.Length == 2)
                                    {
                                        int count = int.Parse(wSplited[0]);
                                        for (int i = 0; i < count; i++)
                                            normalizeWeights.Add(wSplited[1]);
                                    }
                                    else
                                    {
                                        Messages.Warning("Ошибка", "Неверный формат записи весов! Возможно отделение весов запятыми (Например: \"40, 50, 60\"), сокращенная запись одинаковых весов (например: \"3х40\") и комбинированная запись весов (например: \"3х40, 50, 2х60\"). План тренировки не сохранен!");
                                        sqlite.DeleteWorkout(newWorkoutId);
                                        notMessagesShowed = false;
                                    }
                                }
                            }
                            foreach (string r in repeats)
                            {
                                if (r.IndexOfAny(equalRecordsSeparator) == -1)
                                {
                                    normalizeRepeats.Add(r);
                                }
                                else //если есть специальный разделитель, то
                                {
                                    //нулевой элемент - количество повторений веса, а первый - сам вес
                                    string[] rSplited = r.Split(equalRecordsSeparator);
                                    if (rSplited.Length == 2)
                                    {
                                        int count = int.Parse(rSplited[0]);
                                        for (int i = 0; i < count; i++)
                                            normalizeRepeats.Add(rSplited[1]);
                                    }
                                    else
                                    {
                                        Messages.Warning("Ошибка", "Неверный формат записи повторов! Возможно отделение повторов запятыми (Например: \"10, 13, 15\"), сокращенная запись одинаковых повторов (например: \"8х10\") и комбинированная запись повторов (например: \"3х20, 30, 2х40\"). План тренировки не сохранен!");
                                        sqlite.DeleteWorkout(newWorkoutId);
                                        notMessagesShowed = false;
                                    }
                                }
                            }
                            if (normalizeWeights.Count == normalizeRepeats.Count)
                            {
                                for (int i = 0; i < normalizeWeights.Count; i++)
                                {
                                    Result r = new Result(float.Parse(normalizeWeights[i], format), byte.Parse(normalizeRepeats[i]));
                                    sqlite.AddResult(r);
                                    long lastResultId = sqlite.GetLastResultId();

                                    OtherMethods.Debug("lastResultId: " + lastResultId.ToString());
                                    Work w = new Work(lastResultId, newWorkoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, hardWork), comment);
                                    sqlite.AddWork(w);
                                }
                            }
                            else
                            {
                                Messages.Warning("Ошибка", "Количество весов и соответствующих повторов в одной или нескольких силовых работах не одинаково! План тренировки не сохранен!");
                                sqlite.DeleteWorkout(newWorkoutId);
                                notMessagesShowed = false;
                            }
                        }
                    }
                }
                if (notMessagesShowed)
                    Messages.Info("Сообщение", "План тренировки сохранен успешно!");
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
                sqlite.DeleteWorkout(newWorkoutId);
                Messages.Warning("Ошибка", "План тренировки не сохранен из-за возникшей программной ошибки! Проверьте корректность ввода полей и повторите сохранение. Если ошибка повторится, обратитесь к разработчику.");
            }
        }
        /// <summary>
        /// Удаление выбранного плана трениовок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDeleteWorkoutPlan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sqlite.DeleteWorkoutPlan(sqlite.GetWorkoutPlanId(comboBoxWorkoutPlanTitleDelete.Text));
                LoadAllWorkoutPlans();
                Messages.Info("План тренировок удален!", "Удаление записи плана тренирово из базы данных произошло успешно.");
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }

        }
        /// <summary>
        /// При смене выделенного пункта меню загружает упражнения текущего плана тренировок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxWorkoutPlanMusclesGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInit)
                if (comboBoxWorkoutPlanType.Text != "" && comboBoxWorkoutPlanMusclesGroup.Text != "")
                {
                    if (sender is ComboBox)
                    {
                        ((ComboBox)sender).Text = e.AddedItems[0].ToString();
                        ClearGridHardWork();
                        LoadWorkoutByCurrentWorkoutPlan();
                    }
                    
                }
        }
        /// <summary>
        /// Загружает упражнения по текущему плану тренировок
        /// </summary>
        private void LoadWorkoutByCurrentWorkoutPlan()
        {
            WorkoutPlan wp = sqlite.GetWorkoutPlan(sqlite.GetWorkoutPlanId(comboBoxWorkoutPlanTitle.Text));
            if (wp != null)
            {
                List<Workout> workouts = sqlite.GetWorkouts(wp.id,
                    (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_PLAN_TYPE, comboBoxWorkoutPlanType.Text),
                    (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutPlanMusclesGroup.Text));
                foreach (Workout workout in workouts)
                {
                    List<Work> works = sqlite.GetWorks(workout.id);
                    string lastWork = "";
                    string weights = "";
                    string repeats = "";
                    string comment = "";
                    foreach (Work work in works)
                    {
                        Result result = sqlite.GetResult(work.resultId, format);
                        string currentWork = sqlite.GetValueById(SQLite.TABLE_WORK_TYPE, work.workTypeId);
                        comment = work.comment;

                        if (currentWork.Equals(lastWork))
                        {
                            weights += string.Format("{0} {1}", weightsSeparator, result.weight);
                            repeats += string.Format("{0} {1}", repeatsSeparator, result.repeat);
                        }
                        else
                        {
                            //записали всё, что было до
                            if (lastWork != "")
                                AddControlsOnHardWorkGrid(lastWork, weights, repeats, work.comment);

                            //записали текущее
                            lastWork = currentWork;
                            weights = result.weight.ToString();
                            repeats = result.repeat.ToString();
                        }
                    }
                    if (lastWork != "")
                        AddControlsOnHardWorkGrid(lastWork, weights, repeats, comment);
                }
            }
            else
                Messages.Warning("Ошибка", "Не выбран редактируемый план тренировок!");

        }
        /// <summary>
        /// Очищает список силовых работ
        /// </summary>
        private void ClearGridHardWork()
        {
            if (gridWorkoutPlanRecords != null)
            {
                for (int i = 0; i < gridWorkoutPlanRecords.Children.Count; i++)
                    if (gridWorkoutPlanRecords.Children[i] is Grid)
                    {
                        gridWorkoutPlanRecords.Children.Remove(gridWorkoutPlanRecords.Children[i]);
                        i--;
                    }
                countHardWorks = 0;
            }
        }
        /// <summary>
        /// Создает файл плана тренировок
        /// </summary>
        private void CreateWorkoutPlanFile()
        {
            try
            {
                WorkoutPlan wp = sqlite.GetCurrentWorkoutPlan();
                List<string> allWorks = new List<string>();
                allWorks.Add(string.Format("Наименование плана: {0}", wp.title));
                allWorks.Add(string.Format("Периодичность: {0}", OtherMethods.getCurrentFormOfWordDay(wp.period.ToString())));
                allWorks.Add("");
                if (wp != null)
                {
                    for (int i = 0; i < comboBoxWorkoutPlanType.Items.Count; i++)
                        for (int j = 0; j < comboBoxWorkoutPlanMusclesGroup.Items.Count; j++)
                        {
                            List<Workout> workouts = sqlite.GetWorkoutsFromWorkoutPlan(wp.id,
                                    (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_PLAN_TYPE, comboBoxWorkoutPlanType.Items[i].ToString()),
                                    (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutPlanMusclesGroup.Items[j].ToString()));
                            int k = 0;
                            foreach (Workout workout in workouts)
                            {
                                allWorks.Add("=================================================================");
                                allWorks.Add(string.Format("Тип плана: {0}\t\tГруппа мышц: {1}", sqlite.GetValueById(SQLite.TABLE_WORKOUT_PLAN_TYPE, workout.workoutPlanTypeId), sqlite.GetValueById(SQLite.TABLE_MUSCLES_GROUP, workout.musclesGroupId)));
                                //allWorks.Add(string.Format("Группа мышц: {0}",sqlite.GetValueById(SQLite.TABLE_MUSCLES_GROUP, workout.musclesGroupId)));
                                allWorks.Add("=================================================================");
                                List<Work> works = sqlite.GetWorks(workout.id);
                                string lastWork = "";
                                string weights = "";
                                string repeats = "";
                                string comment = "";
                                foreach (Work work in works)
                                {
                                    Result result = sqlite.GetResult(work.resultId, format);
                                    string currentWork = sqlite.GetValueById(SQLite.TABLE_WORK_TYPE, work.workTypeId);
                                    comment = work.comment;

                                    if (currentWork.Equals(lastWork))
                                    {
                                        weights += string.Format("{0} {1}", weightsSeparator, result.weight);
                                        repeats += string.Format("{0} {1}", repeatsSeparator, result.repeat);
                                    }
                                    else
                                    {
                                        //записали всё, что было до
                                        if (lastWork != "")
                                        {

                                            allWorks.Add(string.Format("{1}. {0}", lastWork, k + 1));
                                            k++;
                                            allWorks.Add(string.Format("Вес: \t{0}", OtherMethods.NormalizeStringComponents(weights, ',', 'х')));
                                            allWorks.Add(string.Format("Повторы: {0}", OtherMethods.NormalizeStringComponents(repeats, ',', 'х')));
                                            if (work.comment != "")
                                                allWorks.Add(string.Format("{0}", work.comment));
                                            allWorks.Add("");
                                        }

                                        //записали текущее
                                        lastWork = currentWork;
                                        weights = result.weight.ToString();
                                        repeats = result.repeat.ToString();
                                    }
                                }
                                if (lastWork != "")
                                {
                                    allWorks.Add(string.Format("{1}. {0}", lastWork, k + 1));
                                    k++;
                                    allWorks.Add(string.Format("Вес: \t{0}", OtherMethods.NormalizeStringComponents(weights, ',', 'х')));
                                    allWorks.Add(string.Format("Повторы: {0}", OtherMethods.NormalizeStringComponents(repeats, ',', 'х')));
                                    if (comment != "")
                                        allWorks.Add(string.Format("{0}", comment));
                                    allWorks.Add("");
                                }
                            }
                        }
                }

                if (!Directory.Exists(EXPORT_WORKOUT_PLAN_DIRECTORY_NAME))
                    Directory.CreateDirectory(EXPORT_WORKOUT_PLAN_DIRECTORY_NAME);
                else
                {
                    string[] files = Directory.GetFiles(EXPORT_WORKOUT_PLAN_DIRECTORY_NAME);
                    if (files.Length > MAX_COUNT_FILES_IN_DIR)
                    {
                        Directory.Delete(EXPORT_WORKOUT_PLAN_DIRECTORY_NAME, true);
                    }
                }
                
                DateTime now = DateTime.Now;
                string path = string.Format("{7}/{0} {1}-{2}-{3} {4}h {5}m {6}s.txt", "workout plan", now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Millisecond, EXPORT_WORKOUT_PLAN_DIRECTORY_NAME);
                
                File.WriteAllLines(path, allWorks.ToArray());

                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + path);
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        /// <summary>
        /// Открывает указанный план тренировок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonViewCurrentWorkoutPlan_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxWorkoutPlanTitle.Text != "")
            {
                CreateWorkoutPlanFile();
            }
            else
                Messages.Warning("Я не буду этого делать", "Выберите какой-нибудь план тренировок!");
        }


    }
}
