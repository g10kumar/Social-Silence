   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:WpfApplication2WpfApplication2  5      %    WpfApplication2.PasswordRequire   'localclr-namespace:WpfApplication2  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  PasswordRequire��5
       N� WindowHeight$	 425Q�6    N� WindowWidth$	 600Q�6-   .��5      �  � RowDefinitions 5   
   �� 5      $
��107* �6   �� 5      $	��22* �6   �� 5      $
��171* �6   .+�5   
   �� $
��Left=�6   $
��Wrap=�6/   $	��Top=�6D   $��73��6\   $	��600��6h   $��16(�6t   ���	#FF007ACC$��0,28,-300,0q�6�   �� 6�    � Language$ en-in�6�   $�	��6�   �� 6�   $.�(This application is password Protected .��6�   �� 6	  $ en-in�6  $
�				��6  �� 6<  $0�*Please provide password to Access further.��6@  6u  �� 5   
   $
��Left=�6   $��82,1,0,0q�6/   $
��Wrap=�6A   $��	Password:��6U   $	��Top=�6f    J� RenderTransformOrigin$ -0.265,0.648&�6~   $��74��6�   $��20��6�   ���	#FF007ACC$��1��6�   G� 5   
   -   5         G�Name$ Password��5      $��
190,1,10,1q�6(    G� 	MaxLength$ 8��6<   $	��100��6J   $��20��6V   $��1��6�   � 5   
   $
��Left=�6   $��45��6*   $��Bottom=�66   $	��600��6Q   #��� ��6]   $��
0,0,-300,0q�6�   $��2��6�   .+�5      �� -   5         �� IsCancel . $��Cancel��5   %   $��340,15,10,10q�66   $��Center=�6L   $��75��6g   6r   �� 5      -   5        	 ��Name$	 Proceed��5      $��Proceed��6&   $
��Left=�68   $��
10,15,0,10q�6S   $��75��6g   $��Center=�6r   6�   5      5      5      