using System;
using Avalonia.Controls;
using Avalonia.Interactivity;


namespace FantasyBasketball;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Set the DataContext to an instance of MainViewModel
        DataContext = new MainViewModel();
    }
}
