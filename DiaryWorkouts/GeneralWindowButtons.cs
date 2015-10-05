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
    /// Здесь только методы работы с кнопками
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Открытие окна профиля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow(userId);
            accountWindow.Show();
        }
        /// <summary>
        /// Открытие окна настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTools_Click(object sender, RoutedEventArgs e)
        {
            ToolsWindow toolsWindow = new ToolsWindow(userId, 
                format, 
                DEFAULT_VALUE_TEXTBOX_WEIGHT, 
                DEFAULT_VALUE_TEXTBOX_REPEAT, 
                DEFAULT_VALUE_TEXTBOX_COMMENT, 
                DEFAULT_VALUE_TEXTBOX_TIME, 
                weightsSeparator, 
                repeatsSeparator, 
                timeSeparators, 
                DEFAULT_TEXTBOX_COLOR_TOOLTIP, 
                DEFAULT_TEXTBOX_COLOR_TEXT, 
                DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP, 
                DEFAULT_TEXTBOX_FONTSTYLE_TEXT,
                MAX_COUNT_FILES_IN_DIR);
            toolsWindow.ShowDialog();
            UpdateAllWorksComboBoxes();
        }
        private void buttonWorkAdd_Click(object sender, RoutedEventArgs e)
        {
            CloseAllTextBoxCommentKardio();
            AddControlsOnWorkGrid("", "", "");
        }
        private void buttonHardWorkAdd_Click(object sender, RoutedEventArgs e)
        {
            CloseAllTextBoxCommentHardWork();
            AddControlsOnHardWorkGrid("", "", "", "");
        }
        private void buttonHardWorkDelete_Click(object sender, RoutedEventArgs e)
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
        private void buttonSaveWorkout_Click(object sender, RoutedEventArgs e)
        {
            labelMessage.Content = "";
            SaveWorkout();
            //if (labelMessage.Content.ToString() == "")
            //    labelMessage.Content = LABEL_MASSAGE_WORKOUT_SUCCESS_SAVED;
            
        }
        /// <summary>
        /// Открытие текстового поля для ввода комментария к кардио работе и сдвиг нижних гридов на состояние текстбокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWorkAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                bool needOpen = ((Button)sender).Content.Equals(BUTTON_ADD_COMMENT_OPEN_CONTENT);
                ((Button)sender).Content = needOpen ? BUTTON_ADD_COMMENT_CLOSE_CONTENT : BUTTON_ADD_COMMENT_OPEN_CONTENT;
                if (((Button)sender).Parent is Grid)
                {
                    Grid parent = ((Grid)((Button)sender).Parent);
                    byte gridId = GetNumberOfGrid(parent);
                    foreach (object child in parent.Children)
                        if (child is TextBox)
                            if (((TextBox)child).Name.IndexOf("textBoxWorkComment") != -1)
                                ((TextBox)child).Visibility = needOpen ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    if (parent.Parent is Grid)
                        if (((Grid)parent.Parent).Name.Equals("gridWorks"))
                            MoveGridWorks(gridId, needOpen);
                }
            }
        }
        /// <summary>
        /// Открытие текстового поля для ввода комментария к кардио работе и сдвиг нижних гридов на состояние текстбокса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonHardWorkAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                bool needOpen = ((Button)sender).Content.Equals(BUTTON_ADD_COMMENT_OPEN_CONTENT);
                ((Button)sender).Content = needOpen ? BUTTON_ADD_COMMENT_CLOSE_CONTENT : BUTTON_ADD_COMMENT_OPEN_CONTENT;
                if (((Button)sender).Parent is Grid)
                {
                    Grid parent = ((Grid)((Button)sender).Parent);
                    byte gridId = GetNumberOfGrid(parent);
                    foreach (object child in parent.Children)
                        if (child is TextBox)
                            if (((TextBox)child).Name.IndexOf("textBoxHardWorkComment") != -1)
                                ((TextBox)child).Visibility = needOpen ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    if (parent.Parent is Grid)
                        if (((Grid)parent.Parent).Name.Equals("gridHardWorks"))
                            MoveGridHardWorks(gridId, needOpen);
                }
            }
        }
        /// <summary>
        /// Удаляет грид с работой из родительского контрола
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWorkDelete_Click(object sender, RoutedEventArgs e)
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
                            if (countWorks > 0)
                                countWorks--;
                            RebuildGridWorks(number);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Очищает гриды кардио и силовой от работ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClearWorkoutGrids_Click(object sender, RoutedEventArgs e)
        {
            ClearGridKardio();
            ClearGridHardWork();
        }
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            ShowSearchWindow();
        }
        /// <summary>
        /// Открывает окно записи плана тренировок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWorkoutPlan_Click(object sender, RoutedEventArgs e)
        {
            WorkoutPlanWindow wpw = new WorkoutPlanWindow(userId, 
                                                          format, 
                                                          DEFAULT_VALUE_TEXTBOX_WEIGHT, 
                                                          DEFAULT_VALUE_TEXTBOX_REPEAT, 
                                                          DEFAULT_VALUE_TEXTBOX_COMMENT, 
                                                          DEFAULT_VALUE_TEXTBOX_TIME, 
                                                          weightsSeparator, 
                                                          repeatsSeparator, 
                                                          equalRecordsSeparator,
                                                          timeSeparators,
                                                          gridIdSeparator,
                                                          DEFAULT_TEXTBOX_COLOR_TOOLTIP, 
                                                          DEFAULT_TEXTBOX_COLOR_TEXT, 
                                                          DEFAULT_TEXTBOX_FONTSTYLE_TOOLTIP, 
                                                          DEFAULT_TEXTBOX_FONTSTYLE_TEXT,
                                                          BUTTON_ADD_HARD_WORK_CONTENT,
                                                          BUTTON_DELETE_HARD_WORK_CONTENT,
                                                          MAX_COUNT_FILES_IN_DIR);
            wpw.ShowDialog();
        }
    }
}
