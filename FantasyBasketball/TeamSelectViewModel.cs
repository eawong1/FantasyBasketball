using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Commands;

namespace FantasyBasketball;

public class TeamSelectViewModel : INotifyPropertyChanged
{
    private League m_league;
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
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }

    private Object? _currentView;
    public Object? CurrentView
    {
        get => _currentView;
        set
        {
            if(_currentView != value)
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }

    public ObservableCollection<string> Teams { get; } = new ObservableCollection<string>();

    public ICommand SubmitCommand{ get; }

    public TeamSelectViewModel(League league)
    {
        m_league = league;

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
            CurrentView = new FunctionSelect(m_league, SelectedItem);
            Console.WriteLine($"SelectedItem: {SelectedItem}");
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
