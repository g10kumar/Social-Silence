   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication2WpfApplication2  5      #    WpfApplication2.CustomizeList   'localclr-namespace:WpfApplication2  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      %   WpfApplication2.CommonFunctions    Loaded$  RetrieveList��5
   =    N� Title$ CustomizeList��6    N� WindowHeight$	 425Q�6    N� WindowWidth$	 600Q�6+   $	��425��5      $	��600��6   .��5      � $
��Left=�6   $	��Top=�6&   $	��425��6>   $��0,0,-300,-101q�6K   $	��600��6b    � ColumnDefinitions 5   
   �� 5      $	��220 �6   �� 5      $	��120 �6   �� 5      $	��260 �6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $	��240 �6   �� 5      $��29 �6   �� 5      $��67 �6   �� 5      $��49 �6   .+�5   
   �� $
��Left=�6   ���	#FF007ACC$��16(�6F   $
��Wrap=�6T   $	��Top=�6i   $��40��6�   $	��600��6�   $��3��6�   caBy Default all sites present in BlackList are Blocked. Move Sites to White List to Unblock them .6  � 5   
   $��1��6   $��10,15,10,25q�6   .+�5      $  Xceed.Wpf.Toolkit.CheckListBox  -   5          Name$ 	BlackList��5       
���   �$��14(�6G   5      � 5   
   $��1��6   $��2��6   $��30,15,30,25q�6,   .+�5        -   5        $ 	WhiteList��5       
���   �$��14(�6G   5      � 5    
   $
��Left=�6   $��50��6*   $��3��66   $��Bottom=�6C   $	��600��6^   $��3��6j   #��� ��6~   $��0,0,0,15q�6�   .+�5!      �� -   5        $
��Next��5!      $
��Left=�6$   $��400,15,0,10q�6?   $��75��6T   6_   �� 5"      -   5        $��Cancel��5"       �� IsCancel . $
��Left=�67   $��500,15,0,10q�6R   $��75��6g   6r   5#      �� 5$   
   -   5        $��Add -->��5$      $��1��6&   $��
20,45,20,0q�66   $��1��6J   $	��Top=�6W   $��30��6o   6{   �� 5%   
   -   5        $��
<-- Remove��5%      $��1��6)   $��20,128,20,0q�69   $��1��6N   $	��Top=�6[   $��30��6s   6   �� 5&   
   $��
Select All��6   $��2��6(   $
��Left=�68   $��30,220,0,28q�6S   $��1��6h   $	��110��6u   $��2��6�   �� 5'   
   -   5        $��
Select All��5'      $��10,220,0,28q�6(   $��1��6=   $
��Left=�6J   $	��108��6e   $��2��6q   6�   5(      5.      