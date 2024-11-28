using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;

namespace FantasyBasketball;

public partial class TeamSelect : UserControl
{
    public TeamSelect(League league)
    {
        InitializeComponent();
        teams.ItemsSource = league.GetTeamNames().OrderBy(x => x).ToList();
    }
}
