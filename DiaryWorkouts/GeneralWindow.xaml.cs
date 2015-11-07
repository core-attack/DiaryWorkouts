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
using System.Windows.Threading;

namespace DiaryWorkouts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLite sqlite = new SQLite();
        /// <summary>
        /// Максимальное колчество файлов в директориях
        /// </summary>
        const int MAX_COUNT_FILES_IN_DIR = 25;
        /// <summary>
        /// Шаг отступа при добавлении новой работы
        /// </summary>
        const double WORK_STEP = 28;
        /// <summary>
        /// Шаг сдвига гридов при изменения состояния текстбокса коммента
        /// </summary>
        const double WORK_GRID_STEP_COMMENT_VISIBLITY_CHANGED = 29;
        /// <summary>
        /// Шаг отступа при добавлении новой силовой работы
        /// </summary>
        const double HARD_WORK_STEP = 56;
        /// <summary>
        /// Шаг прокрутки работ
        /// </summary>
        const double WORK_SCROLL_STEP = 50;
        /// <summary>
        /// Шаг прокрутки силовых работ
        /// </summary>
        const double HARD_WORK_SCROLL_STEP = 86;
        /// <summary>
        /// Высота грида кардио без комментария
        /// </summary>
        const double GRID_WORK_HEIGHT_WITHOUT_COMMENT = 23;
        /// <summary>
        /// Высота грида кардио с комментарием
        /// </summary>
        const double GRID_WORK_HEIGHT_WITH_COMMENT = 48;
        /// <summary>
        /// Текст кнопки открытия коммента в кардио работе
        /// </summary>
        const string BUTTON_ADD_COMMENT_OPEN_CONTENT = "↓";
        /// <summary>
        /// Текст кнопки закрытия коммента в кардио работе
        /// </summary>
        const string BUTTON_ADD_COMMENT_CLOSE_CONTENT = "↑";
        /// <summary>
        /// Текст кнопки добавления кардио работы
        /// </summary>
        const string BUTTON_ADD_WORK_CONTENT = "+";
        /// <summary>
        /// Текст кнопки добавления силовой работы
        /// </summary>
        const string BUTTON_ADD_HARD_WORK_CONTENT = "+";
        /// <summary>
        /// Текст кнопки удаления кардио работы
        /// </summary>
        const string BUTTON_DELETE_WORK_CONTENT = "×";
        /// <summary>
        /// Текст кнопки удаления силовой работы
        /// </summary>
        const string BUTTON_DELETE_HARD_WORK_CONTENT = "×";
        /// <summary>
        /// Значение по умолчанию в текстбоксе веса силовой работы
        /// </summary>
        const string DEFAULT_VALUE_TEXTBOX_WEIGHT = "Вес";
        /// <summary>
        /// Значение по умолчанию в текстбоксе повторений силовой работы
        /// </summary>
        const string DEFAULT_VALUE_TEXTBOX_REPEAT = "Повторения";
        /// <summary>
        /// Значение по умолчанию в текстбоксе комментарий
        /// </summary>
        const string DEFAULT_VALUE_TEXTBOX_COMMENT = "Комментарий";
        /// <summary>
        /// Значение по умолчанию в текстбоксе время
        /// </summary>
        const string DEFAULT_VALUE_TEXTBOX_TIME = "00:00:00.00";
        /// <summary>
        /// Значение по умолчанию в текстбоксе поиска
        /// </summary>
        const string DEFAULT_VALUE_TEXTBOX_SEARCH = "Поиск...";
        /// <summary>
        /// Подпись при успешном сохранении тренировки
        /// </summary>
        const string LABEL_MASSAGE_WORKOUT_SUCCESS_SAVED = "Тренировка сохранена";
        /// <summary>
        /// Цвет подсказок в текстбоксах
        /// </summary>
        Color DEFAULT_TEXTBOX_COLOR_TOOLTIP = Color.FromRgb((byte)160, (byte)155, (byte)155);
        /// <summary>
        /// Цвет набираемого текста
        /// </summary>
        Color DEFAULT_TEXTBOX_COLOR_TEXT = Color.FromRgb((byte)0,(byte)0,(byte)0);
        /// <summary>
        /// Стиль подсказок в текстбоксах
        /// </summary>
        FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP = FontStyles.Italic;
        /// <summary>
        /// Стиль текста в текстбоксах
        /// </summary>
        FontStyle DEFAULT_TEXTBOX_FONTSTYLE_TEXT = FontStyles.Normal;
        /// <summary>
        /// Шрифт по умолчанию
        /// </summary>
        FontFamily DEFAULT_FONT = new FontFamily("Neris Thin");
        /// <summary>
        /// Размер шрифта по умолчанию
        /// </summary>
        double DEFAULT_FONT_SIZE = 12;
        /// <summary>
        /// Количество загружаемых тренировок
        /// </summary>
        int countWorkouts = 15;
        /// <summary>
        /// Последняя добавленная работа
        /// </summary>
        Grid lastAddedWork = null;
        /// <summary>
        /// Последняя добавленная силовая
        /// </summary>
        Grid lastAddedHardWork = null;
        /// <summary>
        /// Разделитель весов тяжелых работ
        /// </summary>
        char weightsSeparator = ',';
        /// <summary>
        /// Разделитель повторов тяжелых работ
        /// </summary>
        char repeatsSeparator = ',';
        /// <summary>
        /// Разделитель одинаковых работ (6х40)
        /// </summary>
        char[] equalRecordsSeparator = { 'x', 'х' };
        /// <summary>
        /// Разделители составляющих времени кардио работы
        /// </summary>
        char[] timeSeparators = new char[] { '.', ':' }; //первый разделитель между секундами и милисекундами, второй - между часами, минутами и секундами
        /// <summary>
        /// Разделитель идетификатора грида от его имени
        /// </summary>
        char gridIdSeparator = '_';
        /// <summary>
        /// Разделитель времени тренировки
        /// </summary>
        char timeBeginEndSeparator = ':';
        /// <summary>
        /// Разделитель даты
        /// </summary>
        char dateSeparator = '.';
        /// <summary>
        /// Разделитель целой и дробно частей числа
        /// </summary>
        string decimalSeparator = ".";
        /// <summary>
        /// Дни недели
        /// </summary>
        Dictionary<string, string> daysWeek = new Dictionary<string,string>();
        /// <summary>
        /// Месяцы
        /// </summary>
        Dictionary<byte, string> month = new Dictionary<byte, string>();
        /// <summary>
        /// Количество работ
        /// </summary>
        int countWorks;
        /// <summary>
        /// Количество силовых работ
        /// </summary>
        int countHardWorks;
        /// <summary>
        /// Идентификатор авторизованного пользователя
        /// </summary>
        long userId = 0;
        /// <summary>
        /// Были ли нажаты клавиши удаления текста
        /// </summary>
        bool isDeleteOrBackspaceKeyPressed = false;
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        /// <summary>
        /// Горизонтальный календарь
        /// </summary>
        HorizontalCalenderUserControl horizontalCalender;
        /// <summary>
        /// Была ли инициализация компонентов
        /// </summary>
        bool isInit = false;
        /// <summary>
        /// Скопированный грид кардио
        /// </summary>
        Grid copiedKardioGrid;
        /// <summary>
        /// Скопированный грид силовой
        /// </summary>
        Grid copiedHardWorkGrid;
        /// <summary>
        /// Массив всех кардио работ
        /// </summary>
        List<string> AllKardioWork = new List<string>();
        /// <summary>
        /// Массив всех силовых работ
        /// </summary>
        List<string> AllHardWorkWork = new List<string>();
        
        public MainWindow(long userId)
        {
            InitializeComponent();
            isInit = true;
            String strVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Title = string.Format("Версия сборки: {0}", strVersion);
            this.userId = userId;

            FontFamily = DEFAULT_FONT;
            FontSize = DEFAULT_FONT_SIZE;

            Init();
        }
        /// <summary>
        /// Инициализация пользовательских элементов управления
        /// </summary>
        private void Init()
        {
            format = new System.Globalization.NumberFormatInfo();
            format.NumberDecimalSeparator = decimalSeparator;

            horizontalCalender = new HorizontalCalenderUserControl(DEFAULT_FONT, DEFAULT_FONT_SIZE, format, gridIdSeparator);
            horizontalCalender.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            horizontalCalender.MouseRightButtonUp += horizontalCalender_MouseRightButtonUp;
            gridForCalender.Children.Add(horizontalCalender);

            sqlite.Connect();

            daysWeek.Clear();
            daysWeek.Add("Monday", "Понедельник");
            daysWeek.Add("Tuesday", "Вторник");
            daysWeek.Add("Wednesday", "Среда");
            daysWeek.Add("Thursday", "Четверг");
            daysWeek.Add("Friday", "Пятница");
            daysWeek.Add("Saturday", "Суббота");
            daysWeek.Add("Sunday", "Воскресенье");

            month.Add(1, "января");
            month.Add(2, "февраля");
            month.Add(3, "марта");
            month.Add(4, "апреля");
            month.Add(5, "мая");
            month.Add(6, "июня");
            month.Add(7, "июля");
            month.Add(8, "августа");
            month.Add(9, "сентября");
            month.Add(10, "октября");
            month.Add(11, "ноября");
            month.Add(12, "декабря");

            InitContextMenu();

            countWorks = 0;
            countHardWorks = 0;

            List<WorkType> workTypes = sqlite.GetAllWorkTypesKardio();
            foreach (WorkType wt in workTypes)
                AllKardioWork.Add(wt.value);

            List<WorkType> workTypes2 = sqlite.GetAllWorkTypesHardWork();
            foreach (WorkType wt in workTypes2)
                AllHardWorkWork.Add(wt.value);

            comboBoxWorkDistance.Text = "";
            gridWorkTemplate.Visibility = System.Windows.Visibility.Hidden;
            gridHardWorkTemplate.Visibility = System.Windows.Visibility.Hidden;
            AddControlsOnWorkGrid("", "", "");
            AddControlsOnHardWorkGrid("", "", "", "");

            List<WorkoutType> workoutTypes = sqlite.GetAllWorkoutTypes();
            comboBoxWorkoutType.Items.Clear();
            foreach (WorkoutType wt in workoutTypes)
            {
                if (wt.id != 4)
                    comboBoxWorkoutType.Items.Add(wt.value);
            }
            comboBoxWorkoutType.SelectedIndex = 0;

            List<WorkoutPlanType> workoutPlanTypes = sqlite.GetAllWorkoutPlanTypes();
            comboBoxWorkoutPlanType.Items.Clear();
            foreach (WorkoutPlanType wt in workoutPlanTypes)
            {
                if (wt.id != 4)
                    comboBoxWorkoutPlanType.Items.Add(wt.value);
            }
            comboBoxWorkoutPlanType.SelectedIndex = 0;

            List<MusclesGroup> musclesGroups = sqlite.GetAllMusclesGroups();
            comboBoxWorkoutMusclesGroup.Items.Clear();

            foreach (MusclesGroup mg in musclesGroups)
            {
                comboBoxWorkoutMusclesGroup.Items.Add(mg.value);
            }
            comboBoxWorkoutMusclesGroup.SelectedIndex = 3;

            buttonWorkAdd.Content = BUTTON_ADD_WORK_CONTENT;
            buttonWorkDelete.Content = BUTTON_DELETE_WORK_CONTENT;
            buttonHardWorkAdd.Content = BUTTON_ADD_HARD_WORK_CONTENT;
            buttonHardWorkDelete.Content = BUTTON_DELETE_HARD_WORK_CONTENT;
            buttonWorkAddComment.Content = BUTTON_ADD_COMMENT_OPEN_CONTENT;

            textBoxWarmUpTime.Focus();

            textBoxWorkoutTimeBegin.Text = sqlite.GetDefaultWorkoutsStartDate();
            textBoxWorkoutTimeEnd.Text = sqlite.GetDefaultWorkoutsEndDate();

        }
        
        /// <summary>
        /// Добавляет группу контролов для описания работы на грид всех работ
        /// </summary>
        private void AddControlsOnWorkGrid(string distance, string time, string comment)
        {
            try
            {
                Grid newWork = new Grid();
                newWork.Name = gridWorkTemplate.Name + "_" + countWorks;
                newWork.Margin = new Thickness(gridWorkTemplate.Margin.Left, gridWorkTemplate.Margin.Top, gridWorkTemplate.Margin.Right, gridWorkTemplate.Margin.Bottom);
                newWork.ClipToBounds = true;
                newWork.ContextMenu = gridWorkTemplate.ContextMenu;

                Button newButtonDelete = new Button();
                newButtonDelete.Name = buttonWorkDelete.Name + "_" + countWorks;
                newButtonDelete.Margin = new Thickness(buttonWorkDelete.Margin.Left, buttonWorkDelete.Margin.Top, buttonWorkDelete.Margin.Right, buttonWorkDelete.Margin.Bottom);
                newButtonDelete.Content = buttonWorkDelete.Content;
                newButtonDelete.HorizontalAlignment = buttonWorkDelete.HorizontalAlignment;
                newButtonDelete.VerticalAlignment = buttonWorkDelete.VerticalAlignment;
                newButtonDelete.Width = buttonWorkDelete.Width;
                newButtonDelete.Height = buttonWorkDelete.Height;
                newButtonDelete.Click += buttonWorkDelete_Click;

                TextBox newTextBoxSearch = new TextBox();
                newTextBoxSearch.Name = textBoxComboBoxSearch.Name + "_" + countWorks;
                newTextBoxSearch.Text = DEFAULT_VALUE_TEXTBOX_SEARCH;
                newTextBoxSearch.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxSearch.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxSearch.HorizontalAlignment = textBoxComboBoxSearch.HorizontalAlignment;
                newTextBoxSearch.VerticalAlignment = textBoxComboBoxSearch.VerticalAlignment;
                newTextBoxSearch.Width = textBoxComboBoxSearch.Width;
                newTextBoxSearch.Height = textBoxComboBoxSearch.Height;
                newTextBoxSearch.GotFocus += textBoxWeight_GotFocus;
                newTextBoxSearch.LostFocus += textBoxWeight_LostFocus;
                newTextBoxSearch.KeyUp += ComboBoxKardioSearch_KeyUp;

                ComboBox newComboBoxDistance = new ComboBox();
                newComboBoxDistance.Name = comboBoxWorkDistance.Name + "_" + countWorks;
                newComboBoxDistance.Margin = new Thickness(comboBoxWorkDistance.Margin.Left, comboBoxWorkDistance.Margin.Top, comboBoxWorkDistance.Margin.Right, comboBoxWorkDistance.Margin.Bottom);
                newComboBoxDistance.Items.Add(newTextBoxSearch);
                foreach (string item in AllKardioWork)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = item;
                    newComboBoxDistance.Items.Add(cbi);
                }
                newComboBoxDistance.HorizontalAlignment = comboBoxWorkDistance.HorizontalAlignment;
                newComboBoxDistance.VerticalAlignment = comboBoxWorkDistance.VerticalAlignment;
                newComboBoxDistance.Width = comboBoxWorkDistance.Width;
                newComboBoxDistance.Height = comboBoxWorkDistance.Height;
                newComboBoxDistance.Text = distance;
                newComboBoxDistance.FontSize = DEFAULT_FONT_SIZE + 1;
                newComboBoxDistance.FontFamily = DEFAULT_FONT;


                TextBox newTextBoxResultTime = new TextBox();
                newTextBoxResultTime.Name = textBoxWorkResultTime.Name + "_" + countWorks;
                newTextBoxResultTime.Margin = new Thickness(textBoxWorkResultTime.Margin.Left, textBoxWorkResultTime.Margin.Top, textBoxWorkResultTime.Margin.Right, textBoxWorkResultTime.Margin.Bottom);
                newTextBoxResultTime.Text = time == "" ? DEFAULT_VALUE_TEXTBOX_TIME : time;
                newTextBoxResultTime.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxResultTime.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxResultTime.HorizontalAlignment = textBoxWorkResultTime.HorizontalAlignment;
                newTextBoxResultTime.VerticalAlignment = textBoxWorkResultTime.VerticalAlignment;
                newTextBoxResultTime.Width = textBoxWorkResultTime.Width;
                newTextBoxResultTime.Height = textBoxWorkResultTime.Height;
                newTextBoxResultTime.KeyUp += textBoxWorkResultTime_KeyUp;
                newTextBoxResultTime.PreviewTextInput += textBoxWorkResultTime_PreviewTextInput;
                newTextBoxResultTime.GotKeyboardFocus += textBoxWorkResultTime_GotKeyboardFocus;
                newTextBoxResultTime.GotMouseCapture += textBoxWorkResultTime_GotMouseCapture;
                newTextBoxResultTime.TextChanged += textBoxWorkResultTime_TextChanged;
                newTextBoxResultTime.FontFamily = DEFAULT_FONT;

                TextBox newTextBoxComment = new TextBox();
                newTextBoxComment.Name = textBoxWorkComment.Name + "_" + countWorks;
                newTextBoxComment.Margin = new Thickness(textBoxWorkComment.Margin.Left, textBoxWorkComment.Margin.Top, textBoxWorkComment.Margin.Right, textBoxWorkComment.Margin.Bottom);
                newTextBoxComment.Text = comment == "" ? DEFAULT_VALUE_TEXTBOX_COMMENT : comment;
                newTextBoxComment.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxComment.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxComment.HorizontalAlignment = textBoxWorkComment.HorizontalAlignment;
                newTextBoxComment.VerticalAlignment = textBoxWorkComment.VerticalAlignment;
                newTextBoxComment.Width = textBoxWorkComment.Width;
                newTextBoxComment.Height = textBoxWorkComment.Height;
                newTextBoxComment.Visibility = System.Windows.Visibility.Hidden;
                newTextBoxComment.GotFocus += textBoxWeight_GotFocus;
                newTextBoxComment.LostFocus += textBoxWeight_LostFocus;
                newTextBoxComment.FontFamily = DEFAULT_FONT;

                Button newButtonAddComment = new Button();
                newButtonAddComment.Name = buttonWorkAddComment.Name + "_" + countWorks;
                newButtonAddComment.Margin = new Thickness(buttonWorkAddComment.Margin.Left, buttonWorkAddComment.Margin.Top, buttonWorkAddComment.Margin.Right, buttonWorkAddComment.Margin.Bottom);
                newButtonAddComment.Content = buttonWorkAddComment.Content;
                newButtonAddComment.HorizontalAlignment = buttonWorkAddComment.HorizontalAlignment;
                newButtonAddComment.VerticalAlignment = buttonWorkAddComment.VerticalAlignment;
                newButtonAddComment.Width = buttonWorkAddComment.Width;
                newButtonAddComment.Height = buttonWorkAddComment.Height;
                newButtonAddComment.Click += buttonWorkAddComment_Click;

                newWork.Children.Add(newButtonDelete);
                newWork.Children.Add(newComboBoxDistance);
                newWork.Children.Add(newTextBoxResultTime);
                newWork.Children.Add(newTextBoxComment);
                newWork.Children.Add(newButtonAddComment);

                lastAddedWork = newWork;
                itemsControlKardioWorks.Items.Add(newWork);
                countWorks++;
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
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

        /// <summary>
        /// Возвращает порядковый номер грида (для работы и силовой работы)
        /// </summary>
        /// <param name="g">Грид</param>
        /// <returns></returns>
        private long GetWorkoutIdOfGrid(Grid g)
        {
            return g.Name.Split(gridIdSeparator).Length > 1 ? long.Parse(g.Name.Split(gridIdSeparator)[1]) : long.MaxValue;
        }
        
        /// <summary>
        /// Добавляет на панель всех силовых работ одну силовую работу
        /// </summary>
        private void AddControlsOnHardWorkGrid(string work, string weight, string repeat, string comment)
        {
            try
            {
                if (weight != "")
                    weight = OtherMethods.NormalizeStringComponents(weight, weightsSeparator, equalRecordsSeparator[0]);
                if (repeat != "")
                    repeat = OtherMethods.NormalizeStringComponents(repeat, repeatsSeparator, equalRecordsSeparator[0]);

                Grid newHardWork = new Grid();
                newHardWork.Name = gridHardWorkTemplate.Name + "_" + countHardWorks;
                newHardWork.Margin = new Thickness(gridHardWorkTemplate.Margin.Left, gridHardWorkTemplate.Margin.Top, gridHardWorkTemplate.Margin.Right, gridHardWorkTemplate.Margin.Bottom);

                Button newButtonDelete = new Button();
                newButtonDelete.Name = buttonHardWorkDelete.Name + "_" + countHardWorks;
                newButtonDelete.Margin = new Thickness(buttonHardWorkDelete.Margin.Left, buttonHardWorkDelete.Margin.Top, buttonHardWorkDelete.Margin.Right, buttonHardWorkDelete.Margin.Bottom);
                newButtonDelete.Content = buttonHardWorkDelete.Content;
                newButtonDelete.HorizontalAlignment = buttonHardWorkDelete.HorizontalAlignment;
                newButtonDelete.VerticalAlignment = buttonHardWorkDelete.VerticalAlignment;
                newButtonDelete.Width = buttonHardWorkDelete.Width;
                newButtonDelete.Height = buttonHardWorkDelete.Height;
                newButtonDelete.Click += buttonHardWorkDelete_Click;

                TextBox newTextBoxSearch = new TextBox();
                newTextBoxSearch.Name = textBoxComboBoxSearch.Name + "_" + countWorks;
                newTextBoxSearch.Text = DEFAULT_VALUE_TEXTBOX_SEARCH;
                newTextBoxSearch.Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP);
                newTextBoxSearch.FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP;
                newTextBoxSearch.HorizontalAlignment = textBoxComboBoxSearch.HorizontalAlignment;
                newTextBoxSearch.VerticalAlignment = textBoxComboBoxSearch.VerticalAlignment;
                newTextBoxSearch.Width = textBoxComboBoxSearch.Width;
                newTextBoxSearch.Height = textBoxComboBoxSearch.Height;
                newTextBoxSearch.GotFocus += textBoxWeight_GotFocus;
                newTextBoxSearch.LostFocus += textBoxWeight_LostFocus;
                newTextBoxSearch.KeyUp += ComboBoxHardWorkSearch_KeyUp;

                ComboBox newComboBoxHardWork = new ComboBox();
                newComboBoxHardWork.Name = comboBoxHardWorks.Name + "_" + countHardWorks;
                newComboBoxHardWork.Margin = new Thickness(comboBoxHardWorks.Margin.Left, comboBoxHardWorks.Margin.Top, comboBoxHardWorks.Margin.Right, comboBoxHardWorks.Margin.Bottom);
                newComboBoxHardWork.Items.Add(newTextBoxSearch);
                foreach (string item in AllHardWorkWork)
                {
                    ComboBoxItem cbi = new ComboBoxItem();
                    cbi.Content = item;
                    newComboBoxHardWork.Items.Add(cbi);
                }
                newComboBoxHardWork.HorizontalAlignment = comboBoxHardWorks.HorizontalAlignment;
                newComboBoxHardWork.VerticalAlignment = comboBoxHardWorks.VerticalAlignment;
                newComboBoxHardWork.Width = comboBoxHardWorks.Width;
                newComboBoxHardWork.Height = comboBoxHardWorks.Height;
                newComboBoxHardWork.Text = work;
                newComboBoxHardWork.FontSize = DEFAULT_FONT_SIZE + 1;


                Label newLabelHardWork = new Label();
                newLabelHardWork.Name = labelHardWork.Name + "_" + countHardWorks;
                newLabelHardWork.Margin = new Thickness(labelHardWork.Margin.Left, labelHardWork.Margin.Top, labelHardWork.Margin.Right, labelHardWork.Margin.Bottom);
                newLabelHardWork.Content = labelHardWork.Content;
                newLabelHardWork.HorizontalAlignment = labelHardWork.HorizontalAlignment;
                newLabelHardWork.VerticalAlignment = labelHardWork.VerticalAlignment;
                newLabelHardWork.Width = labelHardWork.Width;
                newLabelHardWork.Height = labelHardWork.Height;
                newLabelHardWork.Visibility = System.Windows.Visibility.Hidden;

                TextBox newTextBoxWeight = new TextBox();
                newTextBoxWeight.Name = textBoxWeight.Name + "_" + countHardWorks;
                newTextBoxWeight.Margin = new Thickness(textBoxWeight.Margin.Left, textBoxWeight.Margin.Top, textBoxWeight.Margin.Right, textBoxWeight.Margin.Bottom);
                newTextBoxWeight.Text = weight == "" ? DEFAULT_VALUE_TEXTBOX_WEIGHT : weight;
                newTextBoxWeight.Foreground = weight == "" ? new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP) : new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TEXT);
                newTextBoxWeight.FontStyle = weight == "" ? DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP : DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
                newTextBoxWeight.HorizontalAlignment = textBoxWeight.HorizontalAlignment;
                newTextBoxWeight.VerticalAlignment = textBoxWeight.VerticalAlignment;
                newTextBoxWeight.Width = textBoxWeight.Width;
                newTextBoxWeight.Height = textBoxWeight.Height;
                newTextBoxWeight.TextChanged += new TextChangedEventHandler(textBoxWeightAndRepeat_TextChanged);
                newTextBoxWeight.PreviewTextInput += textBoxWeight_PreviewTextInput;
                newTextBoxWeight.GotFocus += textBoxWeight_GotFocus;
                newTextBoxWeight.LostFocus += textBoxWeight_LostFocus;


                Label newLabelSeparator = new Label();
                newLabelSeparator.Name = labelSeparator.Name + "_" + countHardWorks;
                newLabelSeparator.Margin = new Thickness(labelSeparator.Margin.Left, labelSeparator.Margin.Top, labelSeparator.Margin.Right, labelSeparator.Margin.Bottom);
                newLabelSeparator.Content = labelSeparator.Content;
                newLabelSeparator.HorizontalAlignment = labelSeparator.HorizontalAlignment;
                newLabelSeparator.VerticalAlignment = labelSeparator.VerticalAlignment;
                newLabelSeparator.Width = labelSeparator.Width;
                newLabelSeparator.Height = labelSeparator.Height;

                TextBox newTextBoxRepeat = new TextBox();
                newTextBoxRepeat.Name = textBoxRepeat.Name + "_" + countHardWorks;
                newTextBoxRepeat.Margin = new Thickness(textBoxRepeat.Margin.Left, textBoxRepeat.Margin.Top, textBoxRepeat.Margin.Right, textBoxRepeat.Margin.Bottom);
                newTextBoxRepeat.Text = repeat == "" ? DEFAULT_VALUE_TEXTBOX_REPEAT : repeat;
                newTextBoxRepeat.Foreground = repeat == "" ? new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP) : new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TEXT);
                newTextBoxRepeat.FontStyle = repeat == "" ? DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP : DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
                newTextBoxRepeat.HorizontalAlignment = textBoxRepeat.HorizontalAlignment;
                newTextBoxRepeat.VerticalAlignment = textBoxRepeat.VerticalAlignment;
                newTextBoxRepeat.Width = textBoxRepeat.Width;
                newTextBoxRepeat.Height = textBoxRepeat.Height;
                newTextBoxRepeat.TextChanged += new TextChangedEventHandler(textBoxWeightAndRepeat_TextChanged);
                newTextBoxRepeat.PreviewTextInput += textBoxRepeat_PreviewTextInput;
                newTextBoxRepeat.GotFocus += textBoxWeight_GotFocus;
                newTextBoxRepeat.LostFocus += textBoxWeight_LostFocus;


                TextBox newTextBoxHardWorkComment = new TextBox();
                newTextBoxHardWorkComment.Name = textBoxHardWorkComment.Name + "_" + countHardWorks;
                newTextBoxHardWorkComment.Margin = new Thickness(textBoxHardWorkComment.Margin.Left, textBoxHardWorkComment.Margin.Top, textBoxHardWorkComment.Margin.Right, textBoxHardWorkComment.Margin.Bottom);
                newTextBoxHardWorkComment.Text = comment == "" ? DEFAULT_VALUE_TEXTBOX_COMMENT : comment;
                newTextBoxHardWorkComment.Foreground = comment == "" ? new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TOOLTIP) : new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TEXT);
                newTextBoxHardWorkComment.FontStyle = comment == "" ? DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP : DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
                newTextBoxHardWorkComment.HorizontalAlignment = textBoxHardWorkComment.HorizontalAlignment;
                newTextBoxHardWorkComment.VerticalAlignment = textBoxHardWorkComment.VerticalAlignment;
                newTextBoxHardWorkComment.Width = textBoxHardWorkComment.Width;
                newTextBoxHardWorkComment.Height = textBoxHardWorkComment.Height;
                newTextBoxHardWorkComment.GotFocus += textBoxWeight_GotFocus;
                newTextBoxHardWorkComment.LostFocus += textBoxWeight_LostFocus;
                newTextBoxHardWorkComment.Visibility = System.Windows.Visibility.Hidden;


                Button newButtonAddComment = new Button();
                newButtonAddComment.Name = buttonHardWorkAddComment.Name + "_" + countWorks;
                newButtonAddComment.Margin = new Thickness(buttonHardWorkAddComment.Margin.Left, buttonHardWorkAddComment.Margin.Top, buttonHardWorkAddComment.Margin.Right, buttonHardWorkAddComment.Margin.Bottom);
                newButtonAddComment.Content = buttonHardWorkAddComment.Content;
                newButtonAddComment.HorizontalAlignment = buttonHardWorkAddComment.HorizontalAlignment;
                newButtonAddComment.VerticalAlignment = buttonHardWorkAddComment.VerticalAlignment;
                newButtonAddComment.Width = buttonHardWorkAddComment.Width;
                newButtonAddComment.Height = buttonHardWorkAddComment.Height;
                newButtonAddComment.Click += buttonHardWorkAddComment_Click;

                newHardWork.Children.Add(newButtonDelete);
                newHardWork.Children.Add(newLabelHardWork);
                newHardWork.Children.Add(newComboBoxHardWork);
                newHardWork.Children.Add(newTextBoxWeight);
                newHardWork.Children.Add(newLabelSeparator);
                newHardWork.Children.Add(newTextBoxRepeat);
                newHardWork.Children.Add(newTextBoxHardWorkComment);
                newHardWork.Children.Add(newButtonAddComment);

                //if (countHardWorks != 0)
                //{
                //    newHardWork.Margin = new Thickness(newHardWork.Margin.Left, lastAddedHardWork.Margin.Top + HARD_WORK_STEP, newHardWork.Margin.Right, newHardWork.Margin.Bottom);
                //}

                lastAddedHardWork = newHardWork;
                //gridHardWorks.Children.Add(newHardWork);
                itemsControlHardWorks.Items.Add(newHardWork);
                countHardWorks++;
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }
        /// <summary>
        /// Записывает тренировку в БД
        /// </summary>
        private void SaveWorkout()
        {
            //показывались ли какие-либо сообщения во время прохождения этого метода
            bool notMessagesShowed = true;
            long newWorkoutId = -1;
            try
            {
                if (textBoxWorkoutTimeBegin.Text.Split(timeBeginEndSeparator).Length > 1 && textBoxWorkoutTimeEnd.Text.Split(timeBeginEndSeparator).Length > 1)
                {
                    OtherMethods.Debug("userId: " + userId.ToString());
                    TimeSpan begin = new TimeSpan(int.Parse(textBoxWorkoutTimeBegin.Text.Split(timeBeginEndSeparator)[0]), int.Parse(textBoxWorkoutTimeBegin.Text.Split(timeBeginEndSeparator)[1]), 0);
                    TimeSpan end = new TimeSpan(int.Parse(textBoxWorkoutTimeEnd.Text.Split(timeBeginEndSeparator)[0]), int.Parse(textBoxWorkoutTimeEnd.Text.Split(timeBeginEndSeparator)[1]), 0);
                    OtherMethods.Debug("begin: " + begin.ToString());
                    OtherMethods.Debug("end: " + end.ToString());
                    DateTime date = horizontalCalender.selectedDate;
                    sqlite.AddWorkout(new Workout(userId, (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_TYPE, comboBoxWorkoutType.Text), 
                        (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutMusclesGroup.Text), 
                        date, begin, end, byte.Parse(textBoxWarmUpTime.Text), -1, DateTime.Now));
                    newWorkoutId = sqlite.GetLastWorkout(userId).id;
                    OtherMethods.Debug("newWorkoutId: " + newWorkoutId.ToString());
                    string work = "";
                    string result = "";
                    string comment = "";
                    bool ok = false;
                    foreach (object ob in itemsControlKardioWorks.Items)
                    {
                        ok = false;
                        if (ob is Grid)
                        {
                            foreach (object obChild in ((Grid)ob).Children)
                            {
                                if (obChild is ComboBox)
                                {
                                    if (((ComboBox)obChild).Name.IndexOf("comboBoxWorkDistance_") != -1)
                                    {
                                        ok = true;
                                        work = ((ComboBox)obChild).Text;
                                        OtherMethods.Debug("work: " + work);
                                    }
                                }
                                else if (obChild is TextBox)
                                {
                                    if (((TextBox)obChild).Name.IndexOf("textBoxWorkResultTime_") != -1)
                                    {
                                        result = ((TextBox)obChild).Text;
                                        OtherMethods.Debug("result: " + result);
                                    }
                                    else if (((TextBox)obChild).Name.IndexOf("textBoxWorkComment_") != -1)
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
                                Result r = new Result(result);
                                sqlite.AddResult(r);
                                long lastResultId = sqlite.GetLastResultId();

                                OtherMethods.Debug("lastResultId: " + lastResultId.ToString());
                                Work w = new Work(lastResultId, newWorkoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, work), comment);
                                sqlite.AddWork(w);
                            }
                        }
                    }
                    string hardWork = "";
                    string weight = "";
                    string repeat = "";
                    string commentHardWork = "";
                    ok = false;
                    foreach (object ob in itemsControlHardWorks.Items)
                    {
                        ok = false;
                        if (ob is Grid)
                        {
                            foreach (object obChild in ((Grid)ob).Children)
                            {
                                if (obChild is Label)
                                {
                                    if (((Label)obChild).Name.IndexOf("labelHardWork_") != -1)
                                    {
                                        //ok = true;
                                        //hardWork = ((Label)obChild).Content.ToString();
                                        //OtherMethods.Debug("hard work: " + hardWork);
                                    }
                                }
                                else if (obChild is ComboBox)
                                {
                                    if (((ComboBox)obChild).Name.IndexOf("comboBoxHardWorks_") != -1)
                                    {
                                        ok = true;
                                        hardWork = ((ComboBox)obChild).Text;
                                        OtherMethods.Debug("hard work: " + hardWork);
                                    }
                                }
                                else if (obChild is TextBox)
                                {
                                    if (((TextBox)obChild).Name.IndexOf("textBoxWeight_") != -1)
                                    {
                                        weight = ((TextBox)obChild).Text;
                                        OtherMethods.Debug("weight: " + weight);
                                    }
                                    else if (((TextBox)obChild).Name.IndexOf("textBoxRepeat_") != -1)
                                    {
                                        repeat = ((TextBox)obChild).Text;
                                        OtherMethods.Debug("repeat: " + repeat);
                                    }
                                    else if (((TextBox)obChild).Name.IndexOf("textBoxHardWorkComment_") != -1)
                                    {
                                        if (((TextBox)obChild).Text != DEFAULT_VALUE_TEXTBOX_COMMENT)
                                        {
                                            commentHardWork = ((TextBox)obChild).Text;
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
                                            for (int i = 0; i < count; i++ )
                                                normalizeWeights.Add(wSplited[1]);
                                        }
                                        else
                                        {
                                            Messages.Warning("Ошибка", "Неверный формат записи весов! Возможно отделение весов запятыми (Например: \"40, 50, 60\"), сокращенная запись одинаковых весов (например: \"3х40\") и комбинированная запись весов (например: \"3х40, 50, 2х60\"). Тренировка не записана!");
                                            labelMessage.Content = "Неверный формат записи весов! Возможно отделение весов запятыми (Например: \"40, 50, 60\"), сокращенная запись одинаковых весов (например: \"3х40\") и комбинированная запись весов (например: \"3х40, 50, 2х60\"). Тренировка не записана!";
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
                                            Messages.Warning("Ошибка", "Неверный формат записи повторов! Возможно отделение повторов запятыми (Например: \"10, 13, 15\"), сокращенная запись одинаковых повторов (например: \"8х10\") и комбинированная запись повторов (например: \"3х20, 30, 2х40\"). Тренировка не записана!");
                                            labelMessage.Content = "Неверный формат записи повторов! Возможно отделение повторов запятыми (Например: \"10, 13, 15\"), сокращенная запись одинаковых повторов (например: \"8х10\") и комбинированная запись повторов (например: \"3х20, 30, 2х40\"). Тренировка не записана!";
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
                                        Work w = new Work(lastResultId, newWorkoutId, sqlite.GetIdByValue(SQLite.TABLE_WORK_TYPE, hardWork), commentHardWork);
                                        sqlite.AddWork(w);

                                        labelMessage.Content = "";
                                    }
                                }
                                else
                                {
                                    Messages.Warning("Ошибка", "Количество весов и соответствующих повторов в одной или нескольких силовых работах не одинаково! Тренировка не записана!");
                                    labelMessage.Content = "Количество весов и соответствующих повторов в одной или нескольких силовых работах не одинаково! Тренировка не записана!";
                                    sqlite.DeleteWorkout(newWorkoutId);
                                    notMessagesShowed = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректное время тренировки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    notMessagesShowed = false;
                }
                if (notMessagesShowed)
                    Messages.Info("Сообщение", "Тренировка сохранена успешно!");
            }
            catch(Exception e)
            {
                ErrorsHandler.ShowError(e);
                sqlite.DeleteWorkout(newWorkoutId);
                Messages.Warning("Ошибка", "Тренировка не сохранена из-за возникшей программной ошибки! Проверьте корректность ввода полей и повторите сохранение. Если ошибка повторится, обратитесь к разработчику.");
            }
        }

        private void ScrollWorks(double newValue, double oldValue, double step, System.Windows.Controls.Primitives.ScrollBar scrollBar, Grid gridWorks, Grid gridTemplate)
        {
            int i = 0;
            foreach (object ob in gridWorks.Children)
            {
                if (ob is Grid)
                {
                    if (((Grid)ob).Name.IndexOf(gridTemplate.Name + "_") != -1)
                    {
                        if (newValue > oldValue && newValue <= scrollBar.Maximum)
                            ((Grid)ob).Margin = new Thickness(
                                ((Grid)ob).Margin.Left,
                                newValue == countWorks - 1 ? gridTemplate.Margin.Top - i * step : ((Grid)ob).Margin.Top - step,
                                ((Grid)ob).Margin.Right,
                                ((Grid)ob).Margin.Bottom);
                        else if (newValue < oldValue && newValue >= scrollBar.Minimum)
                            ((Grid)ob).Margin = new Thickness(
                                ((Grid)ob).Margin.Left,
                                newValue == 1 ? gridTemplate.Margin.Top + i * step : ((Grid)ob).Margin.Top + step,
                                ((Grid)ob).Margin.Right,
                                ((Grid)ob).Margin.Bottom);
                    }
                }
                i++;
            }
        }
        
        /// <summary>
        /// Смещает кардио работы вниз или вврех в зависимости от параметра
        /// </summary>
        /// <param name="id">Идентификатор кардио работы с которой (не включительно) начинается смещение</param>
        /// <param name="down">Смещать вверх?</param>
        private void MoveGridWorks(byte id, bool down)
        {
            foreach (object child in gridWorks.Children)
                if (child is Grid)
                    if (id < GetNumberOfGrid(((Grid)child)))
                        ((Grid)child).Margin = new Thickness(
                            ((Grid)child).Margin.Left, 
                            down ? ((Grid)child).Margin.Top + WORK_GRID_STEP_COMMENT_VISIBLITY_CHANGED : ((Grid)child).Margin.Top - WORK_GRID_STEP_COMMENT_VISIBLITY_CHANGED, 
                            ((Grid)child).Margin.Right, 
                            ((Grid)child).Margin.Bottom);
        }
        /// <summary>
        /// Смещает кардио работы вниз или вврех в зависимости от параметра
        /// </summary>
        /// <param name="id">Идентификатор кардио работы с которой (не включительно) начинается смещение</param>
        /// <param name="down">Смещать вверх?</param>
        private void MoveGridHardWorks(byte id, bool down)
        {
            foreach (object child in gridHardWorks.Children)
                if (child is Grid)
                    if (id < GetNumberOfGrid(((Grid)child)))
                        ((Grid)child).Margin = new Thickness(
                            ((Grid)child).Margin.Left,
                            down ? ((Grid)child).Margin.Top + WORK_GRID_STEP_COMMENT_VISIBLITY_CHANGED : ((Grid)child).Margin.Top - WORK_GRID_STEP_COMMENT_VISIBLITY_CHANGED,
                            ((Grid)child).Margin.Right,
                            ((Grid)child).Margin.Bottom);
        }
        /// <summary>
        /// Обновляет все списки с кардио и силовыми работами
        /// </summary>
        private void UpdateAllWorksComboBoxes()
        {
            foreach (object children in gridWorks.Children)
            {
                if (children is Grid)
                {
                    foreach (object child in ((Grid)children).Children)
                    {
                        if (child is ComboBox)
                        {
                            ((ComboBox)child).Items.Clear();
                            List<WorkType> workTypes = sqlite.GetAllWorkTypesKardio();
                            foreach (WorkType wt in workTypes)
                                ((ComboBox)child).Items.Add(wt.value);
                        }
                    }
                }
            }
            foreach (object children in gridHardWorks.Children)
            {
                if (children is Grid)
                {
                    foreach (object child in ((Grid)children).Children)
                    {
                        if (child is ComboBox)
                        {
                            ((ComboBox)child).Items.Clear();
                            List<WorkType> workTypes2 = sqlite.GetAllWorkTypesHardWork();
                            foreach (WorkType wt in workTypes2)
                                ((ComboBox)child).Items.Add(wt.value);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Очищает список кардио работ
        /// </summary>
        private void ClearGridKardio()
        {
            if (gridWorks != null)
            {
                itemsControlKardioWorks.Items.Clear();
                countWorks = 0;
            }
        }
        /// <summary>
        /// Очищает список силовых работ
        /// </summary>
        private void ClearGridHardWork()
        {
            if (gridHardWorks != null)
            {
                itemsControlHardWorks.Items.Clear();
                countHardWorks = 0;
            }
        }
        /// <summary>
        /// Отображение выделенной тренировки на панелях кардио и силовой работ
        /// </summary>
        private void ViewWorkoutOnGrid(long workoutId)
        {
            ClearGridHardWork();
            ClearGridKardio();
            Workout workout = sqlite.GetWorkout(workoutId);
            textBoxWorkoutTimeBegin.Text = workout.timeBegin;
            textBoxWorkoutTimeEnd.Text = workout.timeEnd;
            textBoxWarmUpTime.Text = workout.warmUp.ToString();
            comboBoxWorkoutType.Text = sqlite.GetValueById(SQLite.TABLE_WORKOUT_TYPE, Convert.ToInt64(workout.workoutTypeId));
            List<Work> works = sqlite.GetWorks(workoutId);
            foreach (Work w in works)
            {
                WorkType wt = sqlite.GetWorkType(w.workTypeId);
                if (wt.sportTypeId == 1)
                    AddControlsOnWorkGrid(wt.value, sqlite.GetResult(w.resultId, format).time, w.comment);
                else
                {
                    List<Result> results = sqlite.GetResults(w.resultId, wt.id, format);
                    string weights = "";
                    string repeats = "";
                    foreach (Result r in results)
                    {
                        weights += weights.Length == 0 ? r.weight.ToString() : ", " + r.weight.ToString();
                        repeats += repeats.Length == 0 ? r.repeat.ToString() : ", " + r.repeat.ToString();
                    }
                    AddControlsOnHardWorkGrid(wt.value, weights, repeats, w.comment);
                }
            }
        }
        /// <summary>
        /// Открывает окно поиска
        /// </summary>
        private void ShowSearchWindow()
        {
            SearchWindow searchWindow = new SearchWindow(textBoxSearch.Text, dateSeparator, daysWeek, gridIdSeparator, countWorkouts, format, userId);
            searchWindow.ShowDialog();
        }

        private void buttonReference_Click(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// Загружает упражнения по текущему плану тренировок
        /// </summary>
        private void LoadWorkoutByCurrentWorkoutPlan()
        {
            WorkoutPlan wp = sqlite.GetCurrentWorkoutPlan();
            if (wp != null)
            {
                List<Workout> workouts = sqlite.GetWorkoutsFromWorkoutPlan(wp.id, 
                    (short)sqlite.GetIdByValue(SQLite.TABLE_WORKOUT_PLAN_TYPE, comboBoxWorkoutPlanType.Text), 
                    (short)sqlite.GetIdByValue(SQLite.TABLE_MUSCLES_GROUP, comboBoxWorkoutMusclesGroup.Text));
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
                Messages.Warning("Ошибка", "Не выбран загружаемый план тренировок!");
            
        }
        /// <summary>
        /// Загружает упражнения по текущему плану тренировок
        /// </summary>
        private void LoadWorkoutBySelectedDate(DateTime date)
        {
            ClearGridKardio();
            ClearGridHardWork();
            List<Workout> workouts = sqlite.GetWorkouts(date);
            foreach (Workout workout in workouts)
            {
                List<Work> works = sqlite.GetWorks(workout.id);
                string lastWork = "";
                string weights = "";
                string repeats = "";
                string comment = "";
                string time = "";
                foreach (Work work in works)
                {
                    if (work.workTypeId != -1)
                    { 
                        WorkType wt = sqlite.GetWorkType(work.workTypeId);
                        if (wt.sportTypeId == 1)
                        {
                            Result result = sqlite.GetResult(work.resultId, format);
                            string currentWork = sqlite.GetValueById(SQLite.TABLE_WORK_TYPE, work.workTypeId);
                            time = result.time;
                            comment = work.comment;
                            AddControlsOnWorkGrid(currentWork, time, comment);
                        }
                        else if (wt.sportTypeId == 2)
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
                    }
                }
                if (lastWork != "")
                    AddControlsOnHardWorkGrid(lastWork, weights, repeats, comment);
            }

        }

        /// <summary>
        /// При смене выделенного пункта меню загружает упражнения текущего плана тренировок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxWorkoutMusclesGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInit && sender is ComboBox)
                if (comboBoxWorkoutPlanType.Text != "" && comboBoxWorkoutMusclesGroup.Text != "")
                {
                    ((ComboBox)sender).Text = e.AddedItems[0].ToString();
                    ClearGridHardWork();
                    LoadWorkoutByCurrentWorkoutPlan();
                }
            
        }

        private void horizontalCalender_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (horizontalCalender.clickedDate != null)
                LoadWorkoutBySelectedDate(horizontalCalender.clickedDate);
        }

        #region[про контекстное меню]
        /// <summary>
        /// Инициализация контекстного меню кардио
        /// </summary>
        private void InitContextMenu()
        {
            ContextMenu cmKardio = new ContextMenu();
            cmKardio.FontFamily = new FontFamily("Neris Thin");

            MenuItem copy = new MenuItem();
            copy.Header = "Копировать кардио работу";
            copy.Click += contextMenu_CopyClick;
            cmKardio.Items.Add(copy);

            MenuItem paste = new MenuItem();
            paste.Header = "Вставить кардио работу";
            paste.Click += contextMenu_PasteClick;
            cmKardio.Items.Add(paste);

            gridWorkTemplate.ContextMenu = cmKardio;
            gridWorks.ContextMenu = cmKardio;

        }
        /// <summary>
        /// Копирует грид кардио работы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu_CopyClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                object contextMenu = ((MenuItem)sender).Parent;
                if (contextMenu is ContextMenu)
                {
                    object grid = ((ContextMenu)contextMenu).PlacementTarget;
                    if (grid is Grid)
                    {
                        copiedKardioGrid = ((Grid)grid);
                    }
                }
            }
        }
        /// <summary>
        /// Вставляет грид кардио работы в конец всех гридов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu_PasteClick(object sender, RoutedEventArgs e)
        {
            if (copiedKardioGrid != null)
            {
                string distance = "";
                string time = "";
                string comment = "";
                foreach(object ch in copiedKardioGrid.Children)
                {
                    if (ch is ComboBox)
                        distance = ((ComboBox)ch).Text;
                    else if (ch is TextBox)
                    {
                        if (((TextBox)ch).Name.IndexOf("textBoxWorkResultTime") != -1)
                            time = ((TextBox)ch).Text;
                        else
                            comment = ((TextBox)ch).Text;
                    }
                }
                AddControlsOnWorkGrid(distance, time, comment);
            }
        }
        #endregion

    }
}
