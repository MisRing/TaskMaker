   M S B A M L   `   `   `  ����A  =Editor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:EditorEditor  5   	       Editor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   &localclr-namespace:Editor     5   	   -   5          � Title$  
MainWindow��5   	   $	��550��5   	   $	��720��6    � WindowStartupLocation$ CenterScreen=�5   	   
������� � 	Resources 5       �� 
TargetType   DisabledImageStyle �� Triggers P� Property P� Value �� Property �� Value��%�   &	        �� 5   
   " ��6    5      P� 5       � {� .  . �5      ��  �@J�Opacity$	 0.4Q�60   5      5      5      	 J� CommandBindings	 5      )  #System.Windows.Input.CommandBinding  5   
   -   5        
   Command$
 Close��5      61     5   
   -   5        $
 DecreaseZoom��5      61     5   
   -   5        $
 IncreaseZoom��5      61   .��5      ]� -   5         ]�Name$ mainDock��5      ��. .+�5   
   � $	��Top=�6   � 5   
   $	��Top=�6   $��Stretch=�6$   $��0,2,0,4q�6B   .+�5      `�  `� IsLocked . .�5       c� -   5         c� Band$ 1��5        c� 	BandIndex$ 1��6#   $��30��62   .@�5!      �� -   5         ��Name$ zoomoutButton��5!      $��DecreaseZoom��64    � ToolTip$ Zoom out��6K    ^� ShowOnDisabled . .��5"         Editor.AutoGreyableImage  $��images\gui\zoomout.png��62   5#      �� 5$      -   5        $ zoominButton��5$      $��IncreaseZoom��63   $ Zoom in��6J    . .��5%        $��images\gui\zoomin.png��62   5&      �� 5'      d� 5(      -	   5         d�Name$ underbarToggle��5(   #   $ Show/Hide Underbar��6;   .��5)      �� $��images\gui\underbar.png��6    5*      �� 5+      5,      5-      �� 5.      -
   5        $ button��5.      $%��Вставить формулу��6%   $
��Left=�6@   $��30��6[   $��	187,0,0,0q�6g   $	��Top=�6z   $	��357��6�   
�������
�������$��16(�6�   5/         Editor.MathToolBar  50   
   -   5          Name$ mathToolBar��50      $	��Top=�61    K� ZIndex$ 99��6F   6^   � 51   
   ���	#FFEEDDCC$��Bottom=�6&   $y�	Collapsed=�6>    � ColumnDefinitions 52      �� 53      $��* �6#   6/   �� 54      $
��Auto �6#   62   .+�56      �� -   5        $��statusBarLeftLabel��56      $��25��6.   6<   �� 57      -   5        $��statusBarRightLabel��57   $   $��1��6   $��25��6?   $��Right=�6K   6i   58      �� 59   
   -   5         ��Name$ scrollViwer��59      |�. $
��Auto=�6�    � FocusVisualStyle 6,   Z� .��5:         Editor.EditorControl  -   5          Name$ editor��5:   ;   
������ |�.  6]   Z� 6{   5;      5<      5=      