using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FantasyBasketball;

public partial class TeamSelect : UserControl
{
    public TeamSelect(League league)
    {
        InitializeComponent();
        
        DataContext = new TeamSelectViewModel(league);
    }

    private void SubmitButton_Clicked(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine("submit");
        if (this.Parent is ContentControl contentControl)
        {
            contentControl.Content = new FunctionSelect(league, );
        }
    }
}
