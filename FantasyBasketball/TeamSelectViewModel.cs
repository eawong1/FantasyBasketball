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
    private readonly MainViewModel m_mainViewModel;
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


    public ObservableCollection<string> Teams { get; } = new ObservableCollection<string>();

    public ICommand SubmitCommand{ get; }

    public TeamSelectViewModel(MainViewModel mainViewModel, League league)
    {
        m_mainViewModel = mainViewModel;
        m_league = league;

        var teamNames = m_league.GetTeamNames();

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
            m_mainViewModel.CurrentView = new FunctionSelectViewModel(m_mainViewModel, m_league, SelectedItem);
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
