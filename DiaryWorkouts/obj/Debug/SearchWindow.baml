   M S B A M L   `   `   `  ����  DiaryWorkouts     DiaryWorkouts.SearchWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  SearchWindow��5   	   $	��600��6   $
��1024��6+    � 
ResizeMode$ NoResize=�68   $��
Neris Thin)�6N   .��5      � .+�5   
   � -   5         �Name$ 
gridSearch��5      $
��Left=�6#   $	��485��6>   $��	10,74,0,0q�6K   $	��Top=�6^   $	��996��6v   .+�5      � -   5        $ gridLastWorkoutTemplate��5      $��30��64   $��0,32,2,0q�6@   $	��Top=�6R   $��2��6�   .+�5      �� -   5         ��Name$ labelWorkoutDateTemplate��5      $'��!Понедельник 08.09.2014��6:   $
��Left=�6[   $	��Top=�6v   $��0,2,0,0q�6�   $��26��6�   $	��150��6�   $��0��6�   6�   �� 5	      -   5        $&  labelWorkoutWorkAndReultTemplate��5	      $����Бег 1000 метров 8 раз (3:05, 3:04, 3:03 ...), жим лёжа 60 кг 8х10, жим гантелей лежа 15 кг 6х10��6B   $
��Left=�6�   $	��Top=�6�   $��	150,2,0,0q�6�   $	��506��6�   $��26��6�   6
  5
      �� 5      $��Right=�6   $	��Top=�64   $	��485��6L   $��17��6Y   $��Right=�6d   $��0,0,2,0q�6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGridTableSearch��5      $
��Left=�60   $��	10,79,0,0q�6K   $	��Top=�6^   $	��480��6v   $	��996��6�   $��16(�6�   � 5   
   $
��Left=�6   $��24��6*   $��	10,10,0,0q�66   $	��Top=�6I   $	��996��6a   .+�5      �� -   5         ��Name$ textBoxSearch��5      $
��Left=�6-   $��23��6H    �� TextWrapping$
 Wrap=�6T   $	��Top=�6h   $	��245��6�   $��Center=�6�   $��Поиск...��6�   6�   �� 5      $��По дате��6   $
��Left=�6'   $��	250,0,0,0q�6B   $	��Top=�6U   $��52��6m   $��24��6x   �� 5      -   5         ��Name$ comboBoxWorkoutTypeSearch��5      $
��Left=�6:   $��	307,0,0,0q�6U   $	��Top=�6h   $	��112��6�   6�   �� 5      -   5         ��Name$ checkBoxKardio��5      $��Кардио��6/   $
��Left=�6@   $��	424,4,0,0q�6[   $	��Top=�6n   	 d� 	IsChecked$
	 TrueY�6�   6�   �� 5      -	   5        $ checkBoxHardWork��5      $��Силовые��61   $
��Left=�6C   $��	488,4,0,0q�6^   $	��Top=�6q   
 J� RenderTransformOrigin$
 2.18,0.8&�6�   $
	 TrueY�6�   $��
Neris Thin)�6�   �� 5      -
   5        $' !comboBoxWorkoutMusclesGroupSearch��5      $
��Left=�6B   $��	560,1,0,0q�6]   $	��Top=�6p   $	��109��6�    �� SelectedIndex$ 0��6�   $��22��6�   .@�5      �� $�� ��6   5      �� 5      -   5         ��Name$ buttonSearch��5      $��
Найти��6+   $��	674,0,0,0q�6;   $	��Top=�6N   $��24��6f   6r   5      � 5   
   -   5        $ 	gridPages��5      $
��Left=�6"   $��35��6=   $	��Top=�6I   $	��962��6a   $��	10,39,0,0q�6m   .+�5      �� -   5        $ labelPageTemplate��5      $��99��6/   $
��Left=�6<   $	��Top=�6W   $��34��6o   $��34��6{   $��16(�6�   $��Center=�6�   $��Center=�6�   $��36,0,0,0q�6�   ���	#FFB7BFF36�   �� 5      -   5        $ labelPagePrevious��5      $��<��6/   $
��Left=�6>   $	��Top=�6Y   $��34��6q   $��34��6}   $��16(�6�   $��Center=�6�   $��Center=�6�   ���	#FFB7BFF3$��Batang)�6�   6  5      5      5       