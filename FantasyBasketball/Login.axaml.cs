using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Utilities;


namespace FantasyBasketball;

public partial class Login : UserControl
{
    public Login()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(object? sender, RoutedEventArgs e)
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

        var responseData = await UtilityFunctions.Login(leagueId, leagueYear, swid, espnS2);

        League league = new League(responseData);
        
        if (this.Parent is ContentControl contentControl)
        {
            contentControl.Content = new TeamSelect(league);
        }
    }

}