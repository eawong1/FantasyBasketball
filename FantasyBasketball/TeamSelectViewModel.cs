using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Commands;

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
                ((RelayCommand)SubmitCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public ObservableCollection<string> Teams { get; } = new ObservableCollection<string>();

    public ICommand SubmitCommand{ get; }

    public TeamSelectViewModel(League league)
    {
        var teamNames = league.GetTeamNames();

        foreach (var team in teamNames.OrderBy(x => x))
        {
            Teams.Add(team);
        }

        SubmitCommand = new RelayCommand(OnSubmit, CanSubmit);
    }

    private void OnSubmit()
    {
        if(SelectedItem != null)
        {
            Console.WriteLine($"You selected ahh: {SelectedItem}");
        }
    }

    private bool CanSubmit()
    {
        //only allow submit button to be hit if item is selected
        return !string.IsNullOrEmpty(SelectedItem);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
