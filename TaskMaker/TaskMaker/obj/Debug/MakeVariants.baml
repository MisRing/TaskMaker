   M S B A M L   `   `   `  ����D  @TaskMaker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TaskMaker	TaskMaker  5   	       TaskMaker.MakeVariants   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:TaskMaker     5   	     � Title$%  Создать варианты��5   	   $	��600��6"   $	��500��6/    � WindowStyle$ SingleBorderWindow=�5	   	    � WindowStartupLocation$ CenterOwner=�5
   	    � 
ResizeMode$ NoResize=�5   	   .��5      � ��5   
   �� 5       �� ImageSource$ 	font2.jpg��6   .+�5   
   �� -   5         ��Name$ comboBox��5      $��18(�6%   $
��Left=�64   $��30��6O   $��
408,79,0,0q�6[   $	��Top=�6o   $��66��6�    �� SelectedIndex$ 0��6�    J� RenderTransformOrigin$ 0.512,0.527&�6�   .@�5      �� $	��pdf��6   �� 5      $
��docx��6   5      �� 5   
   -   5        $ 	comboBox3��5      $��18(�6&   $
��Left=�65   $��30��6P   $��
408,34,0,0q�6\   $	��Top=�6p   $��66��6�   $ 0��6�   $ 0.512,0.527&�6�   .@�5      �� $��1)��6   �� 5      $��1.��6   �� 5      $
��№1��6   5      �� 5   
   -   5         ��Name$ label��5      $��Экспорт в:��6   
�������$��18(�6G   $
��Left=�6U   $��40��6p   $��
261,75,0,0q�6|   $	��Top=�6�   $	��100��6�   �� 5   
   -   5        $ label1��5      $,��&Количество вопросов:��6    
�������$��18(�6R   $
��Left=�6`   $��40��6{   $��7,75,0,0q�6�   $	��Top=�6�   $	��195��6�   �� 5   
   -   5        	 ��Name$	 textBox��5      $
��Left=�6#   $��Center=�6>   $��Center=�6b   
�������$��2q�6�   $��18(�6�   $��35��6�   $��
212,77,0,0q�6�   
 �� TextWrapping$

 Wrap=�6�    �� 	MaxLength$ 2��6�   $	��Top=�6�   $��35��6  �� 5   
   -   5        $ label2��5      $.��(Количество вариантов:��6    
�������$��18(�6T   $
��Left=�6b   $��	7,120,0,0q�6}   $	��Top=�6�   �� 5   
   -   5        $	 textBox_Copy��5      $
��Left=�6(   $��Center=�6C   $��Center=�6g   
�������$��2q�6�   $��18(�6�   $��35��6�   $��212,121,0,0q�6�   $

 Wrap=�6�   $ 2��6�   $	��Top=�6  $��35��6  �� 5   
   -   5        $ label3��5      $ ��Размер шрифта:��6    $��18(�69   
�������$
��Left=�6Z   $��261,120,0,0q�6u   $	��Top=�6�   �� 5    
   -	   5        $ label5��5       $"��Оформление ОКР:��6    $��18(�6:   
�������$
��Left=�6[   $��7,30,0,0q�6v   $	��Top=�6�   �� 5!   
   -
   5        $ label7��5!      $ ��Тип нумерации:��6    
�������$��18(�6L   $
��Left=�6Z   $��40��6u   $��
261,30,0,0q�6�   $	��Top=�6�   $	��142��6�   �� 5#   
   -   5        $ 	comboBox2��5#      $��18(�6&   $
��Left=�65   $��30��6P   $��408,124,0,0q�6\   $	��Top=�6q   $��66��6�   $ 3��6�   .@�5$      �� $��6��6   �� 5%      $��8��6   �� 5&      $��10��6   �� 5'      $��12��6   �� 5(      $��14��6   �� 5)      $��16��6   �� 5*      $��18��6   5+      �� 5,   
   -   5        $ label4��5,      $��Шапка работы:��6    $��22(�68   
�������$��Center=�6Y   $
��Left=�6}   $��
-6,181,0,0q�6�   $	��Top=�6�   $	��494��6�   �� 5-   
   -   5         ��Name$ richTextBox��5-      $
��Left=�6+   $	��150��6F    �� TextAlignment$ Center=�6S   $��2q�6p   
����ح�$��
97,239,0,0q�6�   $	��Top=�6�   $	��300��6�   �� 5.   
   -   5         ��Name$ create��5.   .      one#��[  6   $��20(�6>   $��Создать��6L   $
��Left=�6^   $��
50,460,0,0q�6y   $	��Top=�6�   $	��155��6�   $��55��6�   �� 5/   
   -   5        $
 exit��5/   .   #��[  6   $��20(�6<   $��Закрыть��6J   $
��Left=�6\   $	��Top=�6w   $	��155��6�   $��287,460,0,0q�6�   $��55��6�   �� 50   
   -   5         ��Name$ checkBox��50      $
��Left=�6'   $��
222,41,0,0q�6B   $	��Top=�6V   $��16��6n   $��17��6z   $ 0.5,0.5&�6�   z�51      V� 52      .~�53      ��  �� ScaleX$
 2.55Q�6%    �� ScaleY$	 2.8Q�63   �� 54      �� 55      T� 56      57      59      5;      5<      