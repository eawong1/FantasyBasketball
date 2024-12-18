using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Utilities;


namespace FantasyBasketball;

public partial class FunctionSelect : UserControl
{
    public FunctionSelect()
    {
        InitializeComponent();
        TeamNameText.Text += "ahhhh";
    }

    private void GetPosButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Posbuttoncluicked");
    }

    private void FutureButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("Posbuttoncluicked");
    }

}