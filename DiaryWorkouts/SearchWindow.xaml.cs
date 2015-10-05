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
using System.Data;
using System.Data.Common;

namespace DiaryWorkouts
{
    /// <summary>
    /// Окно поиска по тренировкам
    /// </summary>
    public partial class SearchWindow : Window
    {
        /// <summary>
        /// Текст поля поиска по умолчанию
        /// </summary>
        static public string DEFAULT_SEARCH_TEXT = "Поиск...";
        /// <summary>
        /// Текст по умолчанию в поиске по списку тренировок
        /// </summary>
        static public string DEFAULT_SEARCH_COMBOBOX_WORKOUT_TYPE = "Все тренировки";
        /// <summary>
        /// Текст по умолчанию в поиске по списку тренировок
        /// </summary>
        static public string DEFAULT_SEARCH_COMBOBOX_MUSCLES_GROUPS_TEXT = "Все группы мыщц";
        /// <summary>
        /// Шаг отступа при добавлении результатов поиска
        /// </summary>
        const double SEARCH_RESULT_STEP = 30;
        /// <summary>
        /// Шаг прокрутки результатов поиска
        /// </summary>
        const double SEARCH_RESULTS_SCROLL = 25;
        /// <summary>
        /// Шаг отступа страниц
        /// </summary>
        const double PAGES_STEP = 36;
        /// <summary>
        /// Количество видимых страниц по краям
        /// </summary>
        const int LIMIT_VIEW_PAGES = 10;
        /// <summary>
        /// Разделитель целой и дробно частей числа
        /// </summary>
        char dateSeparator;
        /// <summary>
        /// Разделитель идетификатора грида от его имени
        /// </summary>
        char gridIdSeparator;
        /// <summary>
        /// Дни недели
        /// </summary>
        Dictionary<string, string> daysWeek;
        /// <summary>
        /// Количество загружаемых тренировок
        /// </summary>
        int countWorkoutsPerPage;
        /// <summary>
        /// Количество страниц отображения результатов запроса
        /// </summary>
        int pagesCount;
        /// <summary>
        /// Текущая страница отображения результатов запроса
        /// </summary>
        int currentPage = 1;
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        long userId;
        /// <summary>
        /// Доступ к БД
        /// </summary>
        SQLite sqlite = new SQLite();
        /// <summary>
        /// Разделитель дробной части числа
        /// </summary>
        System.Globalization.NumberFormatInfo format;
        /// <summary>
        /// Последняя добавленная страница
        /// </summary>
        Label lastPage;
        public SearchWindow(string request, char dateSeparator, Dictionary<string, string> daysWeek, char gridIdSeparator, int countWorkoutsPerPage, System.Globalization.NumberFormatInfo format, long userId)
        {
            InitializeComponent();
            this.dateSeparator = dateSeparator;
            this.daysWeek = daysWeek;
            this.gridIdSeparator = gridIdSeparator;
            this.countWorkoutsPerPage = countWorkoutsPerPage;
            this.userId = userId;
            this.format = format;

            sqlite.Connect();

            textBoxSearch.Text = request;
            gridLastWorkoutTemplate.Visibility = System.Windows.Visibility.Hidden;

            List<WorkoutType> workoutTypes = sqlite.GetAllWorkoutTypes();
            comboBoxWorkoutTypeSearch.Items.Clear();
            comboBoxWorkoutTypeSearch.Items.Add(DEFAULT_SEARCH_COMBOBOX_WORKOUT_TYPE);
            foreach (WorkoutType wt in workoutTypes)
                comboBoxWorkoutTypeSearch.Items.Add(wt.value);
            comboBoxWorkoutTypeSearch.SelectedIndex = 0;

            List<MusclesGroup> musclesGroups = sqlite.GetAllMusclesGroups();
            comboBoxWorkoutMusclesGroupSearch.Items.Clear();
            comboBoxWorkoutMusclesGroupSearch.Items.Add(DEFAULT_SEARCH_COMBOBOX_MUSCLES_GROUPS_TEXT);
            foreach (MusclesGroup mg in musclesGroups)
                comboBoxWorkoutMusclesGroupSearch.Items.Add(mg.value);
            comboBoxWorkoutMusclesGroupSearch.SelectedIndex = 0;

            labelPageTemplate.Visibility = System.Windows.Visibility.Hidden;

            FindWorkouts(request, 1);
        }
        /// <summary>
        /// Загружает тренировки в панель поиска
        /// </summary>
        private void FindWorkouts(string request, int currentPage)
        {
            
            try
            {
                if (sqlite.CheckDB())
                {
                    gridSearch.Children.Clear();
                    /*if (request == DEFAULT_SEARCH_TEXT && comboBoxWorkoutTypeSearch.Text == DEFAULT_SEARCH_COMBOBOX_WORKOUT_TYPE && comboBoxWorkoutMusclesGroupSearch.Text == DEFAULT_SEARCH_COMBOBOX_MUSCLES_GROUPS_TEXT && (bool)checkBoxKardio.IsChecked && (bool)checkBoxHardWork.IsChecked)
                    {
                        pagesCount = (sqlite.GetCountLastWorkouts(userId) - 1) / countWorkoutsPerPage + 1;
                        SetAllPagesLinks(pagesCount);
                        SetDBTable(sqlite.Search2(request, countWorkoutsPerPage, (currentPage - 1) * countWorkoutsPerPage, comboBoxWorkoutTypeSearch.Text, (bool)checkBoxKardio.IsChecked, (bool)checkBoxHardWork.IsChecked, comboBoxWorkoutMusclesGroupSearch.Text, userId, format));
                        /*List<Dictionary<string, string>> workouts = sqlite.GetLastWorkouts(userId, countWorkoutsPerPage, (currentPage - 1) * countWorkoutsPerPage, format);
                        int index = 0;
                        foreach (Dictionary<string, string> workout in workouts)
                        {
                            AddWorkoutOnSearchPanel(workout, index);
                            index++;
                        }
                    }
                    else*/
                    {
                        int i = sqlite.GetCountSearchedWorkouts(request, comboBoxWorkoutTypeSearch.Text, (bool)checkBoxKardio.IsChecked, (bool)checkBoxHardWork.IsChecked, comboBoxWorkoutMusclesGroupSearch.Text, userId);
                        pagesCount = sqlite.GetCountSearchedWorkouts(request, comboBoxWorkoutTypeSearch.Text, (bool)checkBoxKardio.IsChecked, (bool)checkBoxHardWork.IsChecked, comboBoxWorkoutMusclesGroupSearch.Text, userId) / countWorkoutsPerPage + 1;
                        SetAllPagesLinks(pagesCount);
                        int j = (currentPage - 1) * countWorkoutsPerPage;
                        SetDBTable(sqlite.Search2(request, countWorkoutsPerPage, (currentPage - 1) * countWorkoutsPerPage, comboBoxWorkoutTypeSearch.Text, (bool)checkBoxKardio.IsChecked, (bool)checkBoxHardWork.IsChecked, comboBoxWorkoutMusclesGroupSearch.Text, userId, format));
                        /*List<Dictionary<string, string>> workouts = sqlite.Search(request, countWorkoutsPerPage, (currentPage - 1) * countWorkoutsPerPage, comboBoxWorkoutTypeSearch.Text, (bool)checkBoxKardio.IsChecked, (bool)checkBoxHardWork.IsChecked, comboBoxWorkoutMusclesGroupSearch.Text, userId, format);
                        int index = 0;
                        foreach (Dictionary<string, string> workout in workouts)
                        {
                            AddWorkoutOnSearchPanel(workout, index);
                            index++;
                        }*/
                    }
                }
            }
            catch (Exception e)
            {
                ErrorsHandler.ShowError(e);
            }
        }
        /// <summary>
        /// Добавляет в панель поиска тренировку
        /// </summary>
        /// <param name="args"></param>
        private void AddWorkoutOnSearchPanel(Dictionary<string, string> args, int index)
        {
            Grid newGridWorkout = new Grid();
            newGridWorkout.Name = "gridWorkout_" + args["id"];
            newGridWorkout.Margin = new Thickness(gridLastWorkoutTemplate.Margin.Left, gridLastWorkoutTemplate.Margin.Top + index * SEARCH_RESULT_STEP, gridLastWorkoutTemplate.Margin.Right, gridLastWorkoutTemplate.Margin.Bottom);
            newGridWorkout.HorizontalAlignment = gridLastWorkoutTemplate.HorizontalAlignment;
            newGridWorkout.VerticalAlignment = gridLastWorkoutTemplate.VerticalAlignment;
            newGridWorkout.MouseLeftButtonDown += searchResult_MouseLeftButtonDown;

            Label newLabelWorkoutDate = new Label();
            newLabelWorkoutDate.Margin = new Thickness(labelWorkoutDateTemplate.Margin.Left,
                                                       labelWorkoutDateTemplate.Margin.Top,
                                                       labelWorkoutDateTemplate.Margin.Right,
                                                       labelWorkoutDateTemplate.Margin.Bottom);
            newLabelWorkoutDate.HorizontalAlignment = labelWorkoutDateTemplate.HorizontalAlignment;
            newLabelWorkoutDate.Name = "labelWorkoutDate";
            string[] parts = args["date"].Split(dateSeparator);
            DateTime weekDay = new DateTime(int.Parse(parts[2]), int.Parse(parts[1]), int.Parse(parts[0]));
            newLabelWorkoutDate.Content = string.Format("{0} {1}", daysWeek[weekDay.DayOfWeek.ToString()], args["date"]);
            newLabelWorkoutDate.MouseLeftButtonDown += searchResult_MouseLeftButtonDown;
            newGridWorkout.Children.Add(newLabelWorkoutDate);

            Label newLabelWorkoutWorkAndResult = new Label();
            newLabelWorkoutWorkAndResult.Margin = new Thickness(labelWorkoutWorkAndReultTemplate.Margin.Left,
                                                                labelWorkoutWorkAndReultTemplate.Margin.Top,
                                                                labelWorkoutWorkAndReultTemplate.Margin.Right,
                                                                labelWorkoutWorkAndReultTemplate.Margin.Bottom);
            newLabelWorkoutWorkAndResult.HorizontalAlignment = labelWorkoutWorkAndReultTemplate.HorizontalAlignment;
            newLabelWorkoutWorkAndResult.Name = "labelWorkAndResult";
            newLabelWorkoutWorkAndResult.Content = args["worksAndResults"];
            newLabelWorkoutWorkAndResult.MouseLeftButtonDown += searchResult_MouseLeftButtonDown;
            newGridWorkout.ToolTip = args["worksAndResults"];
            newGridWorkout.Children.Add(newLabelWorkoutWorkAndResult);

            gridSearch.Children.Add(newGridWorkout);
        }
        /// <summary>
        /// Строит список страниц с результатами запроса
        /// </summary>
        /// <param name="pagesCount"></param>
        private void SetAllPagesLinks(int pagesCount)
        {
            lastPage = null;
            for (int i = 0; i < gridPages.Children.Count; i++ )
            {
                if (gridPages.Children[i] is Label)
                {
                    if (((Label)gridPages.Children[i]).Name.IndexOf("labelPagePrevious") == -1 && ((Label)gridPages.Children[i]).Name.IndexOf("labelPageTemplate") == -1)
                    {
                        gridPages.Children.RemoveAt(i);
                        i--;
                    }
                }
            }

            for (int i = currentPage; i <= pagesCount; i++)
            {
                if (i <= currentPage + LIMIT_VIEW_PAGES || i > pagesCount - LIMIT_VIEW_PAGES)
                {
                    Label newLabelPage = new Label();
                    newLabelPage.Margin = new Thickness(lastPage == null ? labelPageTemplate.Margin.Left : lastPage.Margin.Left + PAGES_STEP,
                                                               labelPageTemplate.Margin.Top,
                                                               labelPageTemplate.Margin.Right,
                                                               labelPageTemplate.Margin.Bottom);
                    newLabelPage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    newLabelPage.HorizontalContentAlignment = labelPageTemplate.HorizontalContentAlignment;
                    newLabelPage.VerticalContentAlignment = labelPageTemplate.VerticalContentAlignment;
                    newLabelPage.VerticalAlignment = labelPageTemplate.VerticalAlignment;
                    newLabelPage.FontSize = labelPageTemplate.FontSize;
                    newLabelPage.Background = i == currentPage ? new SolidColorBrush(Color.FromRgb(254, 187, 8)) : labelPageTemplate.Background;
                    newLabelPage.Name = "labelPage_" + i;
                    newLabelPage.Width = labelPageTemplate.Width;
                    newLabelPage.Height = labelPageTemplate.Height;
                    newLabelPage.Content = OtherMethods.IntToBinaryString(i);
                    newLabelPage.MouseLeftButtonDown += viewPage_MouseLeftButtonDown;
                    lastPage = newLabelPage;
                    gridPages.Children.Add(newLabelPage);

                    if (i == pagesCount)
                    {
                        Label newPageNext = new Label();
                        newPageNext.Margin = new Thickness(lastPage == null ? labelPageTemplate.Margin.Left : newLabelPage.Margin.Left + PAGES_STEP,
                                                           labelPagePrevious.Margin.Top,
                                                           labelPagePrevious.Margin.Right,
                                                           labelPagePrevious.Margin.Bottom);
                        newPageNext.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        newPageNext.HorizontalContentAlignment = labelPagePrevious.HorizontalContentAlignment;
                        newPageNext.VerticalContentAlignment = labelPagePrevious.VerticalContentAlignment;
                        newPageNext.VerticalAlignment = labelPagePrevious.VerticalAlignment;
                        newPageNext.FontSize = labelPagePrevious.FontSize;
                        newPageNext.Background = labelPagePrevious.Background;
                        newPageNext.FontFamily = labelPagePrevious.FontFamily;
                        newPageNext.Width = labelPagePrevious.Width;
                        newPageNext.Height = labelPagePrevious.Height;
                        newPageNext.Name = "labelPageNext";
                        newPageNext.Content = ">";
                        newPageNext.MouseLeftButtonDown += viewNextPage_MouseLeftButtonDown;
                        gridPages.Children.Add(newPageNext);
                    }
                }
                if (i == LIMIT_VIEW_PAGES + currentPage && i < pagesCount - LIMIT_VIEW_PAGES)
                {
                    Label newLabelPage = new Label();
                    newLabelPage.Margin = new Thickness(lastPage == null ? labelPageTemplate.Margin.Left : lastPage.Margin.Left + PAGES_STEP,
                                                               labelPageTemplate.Margin.Top,
                                                               labelPageTemplate.Margin.Right,
                                                               labelPageTemplate.Margin.Bottom);
                    newLabelPage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    newLabelPage.HorizontalContentAlignment = labelPageTemplate.HorizontalContentAlignment;
                    newLabelPage.VerticalContentAlignment = labelPageTemplate.VerticalContentAlignment;
                    newLabelPage.VerticalAlignment = labelPageTemplate.VerticalAlignment;
                    newLabelPage.FontSize = labelPageTemplate.FontSize;
                    newLabelPage.Content = "...";
                    newLabelPage.MouseLeftButtonDown += viewHiddenPage_MouseLeftButtonDown;
                    
                    lastPage = newLabelPage;
                    gridPages.Children.Add(newLabelPage);
                }
                
            }
            

        }
        /// <summary>
        /// Показать скрытые страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewHiddenPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                currentPage = currentPage + 1 < pagesCount ? currentPage + 1 : currentPage;
            }
        }
        /// <summary>
        /// Показать следующую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewNextPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                currentPage = currentPage + 1 < pagesCount ? currentPage + 1 : currentPage;
                FindWorkouts(textBoxSearch.Text, currentPage);
            }
        }
        /// <summary>
        /// Показать предыдущую страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewPreviousPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                currentPage = currentPage - 1 >= 1 ? currentPage - 1 : currentPage;
                FindWorkouts(textBoxSearch.Text, currentPage);
            }
        }
        /// <summary>
        /// Показать страницу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label)
            {
                currentPage = ((Label)sender).Name.Split(gridIdSeparator).Length > 1 ? int.Parse(((Label)sender).Name.Split(gridIdSeparator)[1]) : int.MaxValue;
                FindWorkouts(textBoxSearch.Text, currentPage);
            }
        }
        /// <summary>
        /// Отображение выделенной тренировки на панелях кардио и силовой работ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchResult_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Grid)
            {
                long gridId = GetWorkoutIdOfGrid(((Grid)sender));
                //ViewWorkoutOnGrid(gridId);//своё представление сделать
            }
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
        private void comboBoxWorkoutTypeSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != 0)
            {
                FindWorkouts(textBoxSearch.Text, 1);
            }
        }
        /// <summary>
        /// При изменении состояния галочки на чекбоксах поиска
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchCheckedChanged(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch != null)
                FindWorkouts(textBoxSearch.Text, 1);
        }

        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (sender is TextBox)
                    FindWorkouts(((TextBox)sender).Text, 1);
        }
        /// <summary>
        /// Выделяет всё содержимое текстбокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
                ((TextBox)sender).SelectAll();
        }
        private void textBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxSearch.Text = "";
        }

        private void textBoxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch.Text == "")
                textBoxSearch.Text = SearchWindow.DEFAULT_SEARCH_TEXT;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            FindWorkouts(textBoxSearch.Text, 1);
        }

        private void SetDBTable(DbDataReader reader)
        {
            dataGridTableSearch.ItemsSource = null;
            if (reader != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("№");
                dt.Columns.Add("Дата тренировки");
                dt.Columns.Add("Тип тренировки");
                dt.Columns.Add("Тип работы");
                dt.Columns.Add("Время");
                dt.Columns.Add("Вес");
                dt.Columns.Add("Повторы");
                while (reader.Read())
                {
                    dt.Rows.Add(reader.GetInt64(0), 
                        OtherMethods.GetDate((long)reader.GetInt64(1)), 
                        reader.GetString(2), 
                        reader.GetString(3), 
                        OtherMethods.GetTime((long)reader.GetInt64(4)), 
                        reader.GetValue(5).ToString() == "" ? (float)0 : float.Parse(reader.GetValue(5).ToString(), format),
                        reader.GetValue(6).ToString() == "" ? (Int16)0 : reader.GetInt16(6));
                }
                dataGridTableSearch.ItemsSource = dt.DefaultView;
            }
        }
    }
}
