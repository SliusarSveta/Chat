   M S B A M L   `   `   `  ����A  =Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ServerServer  5   	       Server.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Server     5   	   -   5          �Name$  
mainWindow��5   	    � Title$ 
MainWindow��5   	   $	��332��6   $	��479��6)   .��5	      � $��	0,0,2,-20q�6    � RowDefinitions 5
      �� 5      $	��73* �6   �� 5      $��7* �6   .+�5      �� $��Порт��6
   $
��Left=�6   $��30��64   $��
299,11,0,0q�6@   $	��Top=�6T   $��55��6l   $��14(�6w    J� RenderTransformOrigin$ 
-2.2,0.237&�6�   �� 5      -   5         ��Name$ portBox��5      $
��Left=�6   $��23��68   $��
351,14,0,0q�6D    �� TextWrapping$
 Wrap=�6X   $
��8888��6l   $	��Top=�6x   $��84��6�   $��14(�6�   $ -1.488,0.783&�6�    �� TextAlignment$ Center=�6�   �� 5      ���	#FFE3E9EF$��1q�6#   $
��Left=�67   $	��227��6R   $��	22,47,0,0q�6_   $	��Top=�6r   $	��413��6�   ._�5      �� -   5         ��Name$ chatInfo��5      $
��Left=�6#   $	��233��6>   $��	-1,-1,0,0q�6K   $	��Top=�6^   $	��415��6v   ���	#FFEEEEEE. �5      /� .T�5      J� .*�6   .*�5      �� $� ��6   5      5      5      5      �� 5      $
��Left=�6
   $��32��6%   $��	22,10,0,0q�61   $	��Top=�6D   $��46��6\   �� 5      $��24,6,403,247q�6
   $��LocalServer.ico��6    $
��Fill=�69   $ 
12.5,0.688&�6H   �� 5      -   5         ��Name$ startButton��5      $��Start��6    $
��Left=�60   $��29��6K   $��66,9,0,0q�6W   $	��Top=�6i   $��72��6�   6�   �� 5      -   5        $ finishButton��5      $��Finish��6!   $
��Left=�62   $��29��6M   $��
154,10,0,0q�6Y   $	��Top=�6m   $��72��6�   6�   5      5      