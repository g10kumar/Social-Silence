   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication2WpfApplication2  5      '    !WpfApplication2.AdittionalOptions   'localclr-namespace:WpfApplication2  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      X TXceed.Wpf.Toolkit, Version=1.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5        N� Title$  AdittionalOptions��5      $	��593��6   .��5      �  � RowDefinitions 5   
   �� 5      $
��199* �6   �� 5      $	��40* �6   �� 5      $	��64* �6    � ColumnDefinitions 5   
   �� 5      $
��154* �6   �� 5      $
��144* �6   �� 5      $
��295* �6   .+�5   
   �� -   5         ��Name$ 
SetOpenDns��5      $
��Left=�6'   $��	25,28,0,0q�6B   $	��Top=�6U   $��25��6m   $	��558��6y   $��3��6�   .��5      �� ���	#FF154AAF$��16(�6/   $	��457��6=   $��42��6I   �� 6V   $�
Configure ��6Z   �� 6n   -   5         ��Name$ h1��5   x   $��http://www.opendns.com/?�6�   .�6�   �� $�OpenDNS��6�   6�   �� 6�   $� as DNS of this Computer��6�   �� 6  �� 6$  6+  5      �� 5   
   -   5        $ 
BlockPopUp��5      $
��Left=�6'   $��	25,80,0,0q�6B   $	��270��6U   ���	#FF154AAF$��16(�6x   $	��Top=�6�   $��20��6�   $��2��6�   .��6�   Block Unwanted PopUp and Ads5      �� 5   
   -   5        $ SettingForever��5      $
��Left=�6+   $��
25,129,0,0q�6F   $	��270��6Z   ���	#FF154AAF$��16(�6}   $	��Top=�6�   $��20��6�   $��2��6�   .��6�   Keep Setting Forever6  �� 5    
   -   5        $ SettingTime��5       $
��Left=�6(   $��
25,179,0,0q�6C   ���	#FF154AAF$	��Top=�6n   $��3��6�   $	��378��6�   $��16(�6�   $��20��6�   .��6�   0.Make Setting for the following period of Time:6+  %  Xceed.Wpf.Toolkit.ButtonSpinner  5!   
   -   5          Name$ Hours��5!      $��0,5,80,4q�6J   $��1��6\   $��1��6l      Content. 5"      �� $��0��6   $��Right=�6   $��33��6B   $��0,0,0,0q�6N   5#      �� 5$   
   ���	#FF154AAF$	��Hrs��6,   $��16(�67   $��1��6E   $��	73,8,40,0q�6U   $��1��6h     5%   
   -   5        $	 Min��5%      $��	0,5,231,4q�6G   $��2��6Z   $��1��6j   . 5&      �� $��0��6   $��Right=�6   $��33                      ��6B   5'      �� 5(   
   ���	#FF154AAF$��
73,8,190,0q�6+   $
��Wrap=�6?   $	��Min��6S   $��16(�6^   $��2��6l   $��1��6|   � 5*   
   $
��Left=�6   $��45��6*   $��Bottom=�66   $	��594��6Q   #��� ��6]   $��0,0,-1,0q�6�   $��3��6�   $��2��6�   .+�5+      �� -   5        $��Finish��5+      $
��Left=�6&   $��396,15,0,10q�6A   $��75��6V   6a   �� 5,      -	   5        $��Cancel��5,      $
��Left=�6&   $��498,15,0,10q�6A   $��75��6V    �� IsCancel . 6q   5-      51      52      