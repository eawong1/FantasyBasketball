using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FantasyBasketball;

public class TeamSelectViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string? _selectedItem;
    public string? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                HandleItemSelected();
            }
        }
    }

    public ObservableCollection<string> Teams { get; } = new ObservableCollection<string>();

    public TeamSelectViewModel(League league)
    {
        var teamNames = league.GetTeamNames();

        foreach (var team in teamNames.OrderBy(x => x))
        {
            Teams.Add(team);
        }
    }

    private void HandleItemSelected()
    {
        if (SelectedItem != null)
        {
            Console.WriteLine($"You selected: {SelectedItem}");
            // Perform your action here
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
