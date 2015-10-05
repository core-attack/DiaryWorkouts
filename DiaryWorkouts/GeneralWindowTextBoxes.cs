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
using System.Text.RegularExpressions;

namespace DiaryWorkouts
{
    /// <summary>
    /// Здесь только методы работы с текстбоксами
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Закрывает все окна комментариев в кардио работах
        /// </summary>
        private void CloseAllTextBoxCommentKardio()
        {
            foreach (object child in gridWorks.Children)
            {
                if (child is Grid)
                    foreach (object o in ((Grid)child).Children)
                        if (o is TextBox)
                        {
                            if (((TextBox)o).Name.IndexOf("textBoxWorkComment") != -1)
                                if (((TextBox)o).Visibility != System.Windows.Visibility.Hidden)
                                {
                                    ((TextBox)o).Visibility = System.Windows.Visibility.Hidden;
                                    MoveGridWorks(GetNumberOfGrid(((Grid)child)), false);
                                }
                        }
                        else if (o is Button)
                            if (((Button)o).Name.IndexOf("buttonWorkAddComment") != -1)
                            {
                                ((Button)o).Content = BUTTON_ADD_COMMENT_OPEN_CONTENT;
                            }
            }
        }
        /// <summary>
        /// Закрывает все окна комментариев в силовых работах
        /// </summary>
        private void CloseAllTextBoxCommentHardWork()
        {
            foreach (object child in gridHardWorks.Children)
            {
                if (child is Grid)
                    foreach (object o in ((Grid)child).Children)
                        if (o is TextBox)
                        {
                            if (((TextBox)o).Name.IndexOf("textBoxHardWorkComment") != -1)
                                if (((TextBox)o).Visibility != System.Windows.Visibility.Hidden)
                                {
                                    ((TextBox)o).Visibility = System.Windows.Visibility.Hidden;
                                    MoveGridHardWorks(GetNumberOfGrid(((Grid)child)), false);
                                }
                        }
                        else if (o is Button)
                            if (((Button)o).Name.IndexOf("buttonHardWorkAddComment") != -1)
                            {
                                ((Button)o).Content = BUTTON_ADD_COMMENT_OPEN_CONTENT;
                            }
            }
        }

        private void textBoxRepeat_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Автоматически вставляет после двух цифр соответствующие разделители
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxWorkResultTime_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (sender is TextBox)
                {
                    //00:00:00.00
                    switch (((TextBox)sender).Text.Length)
                    {
                        case 2: { ((TextBox)sender).Text += ":"; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
                            break;
                        case 5: { ((TextBox)sender).Text += ":"; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
                            break;
                        case 8: { ((TextBox)sender).Text += "."; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
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
                            if (value < 99)
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
                                value++;
                                values[1] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[1] = "00";
                        }
                        else if (caretIndex <= 8)//секунды
                        {
                            value = int.Parse(values[2]);
                            if (value < 59)
                            {
                                value++;
                                values[2] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[2] = "00";
                        }
                        else
                        {
                            value = int.Parse(values[3]);
                            if (value < 99)
                            {
                                value += 1;
                                values[3] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[3] = "00";
                        }
                        ((TextBox)sender).Text = string.Format("{0}:{1}:{2}.{3}", values[0], values[1], values[2], values[3]);
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
                                values[0] = "99";
                        }
                        else if (caretIndex <= 5)//минуты
                        {
                            value = int.Parse(values[1]);
                            if (value > 0)
                            {
                                value--;
                                values[1] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[1] = "59";
                        }
                        else if (caretIndex <= 8)//секунды
                        {
                            value = int.Parse(values[2]);
                            if (value > 0)
                            {
                                value--;
                                values[2] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[2] = "59";
                        }
                        else
                        {
                            value = int.Parse(values[3]);
                            if (value >= 1)
                            {
                                value -= 1;
                                values[3] = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                values[3] = "99";
                        }
                        ((TextBox)sender).Text = string.Format("{0}:{1}:{2}.{3}", values[0], values[1], values[2], values[3]);
                        ((TextBox)sender).CaretIndex = caretIndex;
                    }
                    else if (e.Key == Key.Delete || e.Key == Key.Back)
                    {
                        isDeleteOrBackspaceKeyPressed = true;
                    }
                    else //без этого условия будет некорректно работать
                    {
                        isDeleteOrBackspaceKeyPressed = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
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
            string mask = "0123456789 " + otherSeparators;
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
        private void textBoxWorkoutTimeBegin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string mask = "0123456789";
            e.Handled = mask.IndexOf(e.Text) < 0;
        }
        private void textBoxWarmUpTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string mask = "0123456789";
            e.Handled = mask.IndexOf(e.Text) < 0;
        }
        private void textBoxWorkoutTimeBegin_KeyUp(object sender, KeyEventArgs e)
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
        private void textBoxWarmUpTime_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (sender is TextBox)
                {
                    if (((TextBox)sender).Text.Length <= 2)
                    {
                        byte value = byte.Parse(((TextBox)sender).Text);
                        if (e.Key == Key.Up)
                        {
                            if (value < 99)
                            {
                                value++;
                                ((TextBox)sender).Text = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                ((TextBox)sender).Text = "00";
                        }
                        else if (e.Key == Key.Down)
                        {
                            if (value > 0)
                            {
                                value--;
                                ((TextBox)sender).Text = OtherMethods.IntToBinaryString(value);
                            }
                            else
                                ((TextBox)sender).Text = "99";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorsHandler.ShowError(ex);
            }
        }
        private void textBoxWeight_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_WEIGHT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_REPEAT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_COMMENT
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_TIME
                 || ((TextBox)sender).Text == DEFAULT_VALUE_TEXTBOX_SEARCH)
                {
                    ((TextBox)sender).Text = ((TextBox)sender).Text != DEFAULT_VALUE_TEXTBOX_TIME ? "" : DEFAULT_VALUE_TEXTBOX_TIME;
                    ((TextBox)sender).Foreground = new SolidColorBrush(DEFAULT_TEXTBOX_COLOR_TEXT);
                    ((TextBox)sender).FontStyle = DEFAULT_TEXTBOX_FONTSTYLE_TEXT;
                }
                if (((TextBox)sender).Name.IndexOf("textBoxWorkResultTime") != -1)
                    ((TextBox)sender).SelectAll();
            }
        }
        private void textBoxWorkResultTime_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
                ((TextBox)sender).SelectAll();
        }

        private void textBoxWorkResultTime_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (sender is TextBox)
                ((TextBox)sender).SelectAll();
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
                    else if (((TextBox)sender).Name.IndexOf("textBoxHardWorkComment") != -1 || ((TextBox)sender).Name.IndexOf("textBoxWorkComment") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_COMMENT;
                    else if (((TextBox)sender).Name.IndexOf("textBoxWorkResultTime") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_TIME;
                    else if (((TextBox)sender).Name.IndexOf("textBoxComboBoxSearch") != -1)
                        ((TextBox)sender).Text = DEFAULT_VALUE_TEXTBOX_SEARCH;
                }
            }
        }
        private void textBoxWeightAndRepeat_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (sender is TextBox)
            //{
            //    if (((TextBox)sender).Name.IndexOf("textBoxWeight") != -1)
            //    {
            //        TextBox tbWeight = ((TextBox)sender);
            //        TextBox tbRepeat = null;
            //        object parent = ((TextBox)sender).Parent;
            //        if (parent is Grid)
            //        {
            //            foreach (object ob in ((Grid)parent).Children)
            //            {
            //                if (ob is TextBox)
            //                {
            //                    if (((TextBox)ob).Name.IndexOf("textBoxRepeat") != -1)
            //                    {
            //                        tbRepeat = ((TextBox)ob);
            //                        if (tbRepeat.Text == DEFAULT_VALUE_TEXTBOX_REPEAT)
            //                            tbRepeat.Text = "";
            //                    }
            //                }
            //            }
            //        }
            //        if (tbRepeat != null)
            //        {
            //            while (tbWeight.Text.Split(weightsSeparator).Length != tbRepeat.Text.Split(repeatsSeparator).Length)
            //            {
            //                int wSeparators = tbWeight.Text.Split(weightsSeparator).Length;
            //                int rSeparators = tbRepeat.Text.Split(repeatsSeparator).Length;
            //                if (wSeparators > rSeparators)
            //                    tbRepeat.Text += "0" + repeatsSeparator;
            //                else
            //                    tbRepeat.Text = tbRepeat.Text.Remove(tbRepeat.Text.LastIndexOf("0" + repeatsSeparator), 1);
            //            }
            //        }
            //    }
            //    else if (((TextBox)sender).Name.IndexOf("textBoxRepeat") != -1)
            //    {
            //        TextBox tbWeight = null;
            //        TextBox tbRepeat = ((TextBox)sender);
            //        object parent = ((TextBox)sender).Parent;
            //        if (parent is Grid)
            //        {
            //            foreach (object ob in ((Grid)parent).Children)
            //            {
            //                if (ob is TextBox)
            //                {
            //                    if (((TextBox)ob).Name.IndexOf("textBoxWeight") != -1)
            //                    {
            //                        tbWeight = ((TextBox)ob);
            //                        if (tbWeight.Text == DEFAULT_VALUE_TEXTBOX_WEIGHT)
            //                            tbWeight.Text = "";
            //                    }
            //                }
            //            }
            //        }
            //        if (tbWeight != null)
            //        {
            //            while (tbWeight.Text.Split(weightsSeparator).Length != tbRepeat.Text.Split(repeatsSeparator).Length)
            //            {
            //                int wSeparators = tbWeight.Text.Split(weightsSeparator).Length;
            //                int rSeparators = tbRepeat.Text.Split(repeatsSeparator).Length;
            //                if (rSeparators > wSeparators)
            //                    tbWeight.Text += "0" + repeatsSeparator;
            //                else
            //                    tbWeight.Text = tbWeight.Text.Remove(tbWeight.Text.LastIndexOf("0" + repeatsSeparator), 1);
            //            }
            //        }
            //    }
            //}
        }
        private void textBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ShowSearchWindow();
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
        /// <summary>
        /// Автоматическое добавление двоеточий и точек при вводе времени (ДОДЕЛАТЬ, РАБОТАЕТ КАК ГОВНО)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxWorkResultTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //00:00:00.00
                if (!isDeleteOrBackspaceKeyPressed)
                {
                    if (sender is TextBox)
                    {
                        string text = ((TextBox)sender).Text;
                        if (text.Length > 1)
                        {
                            if (text[text.Length - 1] != timeSeparators[0] && text[text.Length - 1] != timeSeparators[1])
                                if (text.Length == 2 || text.Length == 5)
                                {
                                    ((TextBox)sender).Text += timeSeparators[1];
                                }
                                else if (text.Length == 8)
                                {
                                    ((TextBox)sender).Text += timeSeparators[0];
                                }
                            ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
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
        /// Поиск по комбобоксу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxKardioSearch_KeyUp(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                string request = ((TextBox)sender).Text.ToLower();
                object comboBox = ((TextBox)sender).Parent;
                if (comboBox is ComboBox)
                {
                    List<string> lines = GetAllLinesByRequest(AllKardioWork, request);
                    for (int i = 0; i < ((ComboBox)comboBox).Items.Count; i++)
                        if (i != 0)
                        {
                            ((ComboBox)comboBox).Items.RemoveAt(i);
                            i--;
                        }
                    foreach (string line in lines)
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.Content = line;
                        ((ComboBox)comboBox).Items.Add(cbi);
                    }
                }

            }
        }
        /// <summary>
        /// Поиск по комбобоксу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxHardWorkSearch_KeyUp(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                string request = ((TextBox)sender).Text.ToLower();
                object comboBox = ((TextBox)sender).Parent;
                if (comboBox is ComboBox)
                {
                    List<string> lines = GetAllLinesByRequest(AllHardWorkWork, request);
                    for (int i = 0; i < ((ComboBox)comboBox).Items.Count; i++)
                        if (i != 0)
                        {
                            ((ComboBox)comboBox).Items.RemoveAt(i);
                            i--;
                        }
                    foreach (string line in lines)
                    {
                        ComboBoxItem cbi = new ComboBoxItem();
                        cbi.Content = line;
                        ((ComboBox)comboBox).Items.Add(cbi);
                    }
                }

            }
        }
        /// <summary>
        /// Возвращает все совпадения со строкой из указанного списка
        /// </summary>
        /// <param name="list"></param>
        /// <param name="request"></param>
        private List<string> GetAllLinesByRequest(List<string> list, string request)
        {
            List<string> result = new List<string>();
            foreach (string s in list)
                if (s.ToLower().IndexOf(request) != -1)
                    result.Add(s);
            return result;
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}
