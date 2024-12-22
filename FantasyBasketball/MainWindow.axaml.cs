using System;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace FantasyBasketball;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Content = new Login();
        DataContext = new MainViewModel();
    }
}