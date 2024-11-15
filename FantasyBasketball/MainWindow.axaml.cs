using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FantasyBasketball;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object? sender, RoutedEventArgs e)
    {
        // Your login logic here
        string leagueId = LeagueIdTextBox.Text;
        string leagueYear = LeagueYearTextBox.Text;
        string swid = SwidTextBox.Text;
        string espnS2 = espnTextBox.Text;

        Console.WriteLine("League ID: " + leagueId);
        Console.WriteLine("League Year: " + leagueYear);
        Console.WriteLine("SWID: " + swid);
        Console.WriteLine("ESPN S2: " + espnS2);

        
    }

}