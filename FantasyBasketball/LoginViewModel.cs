using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Commands;

namespace FantasyBasketball;
public class LoginViewModel : INotifyPropertyChanged
{
    private string m_leagueId;
    private string m_leagueYear;
    private string m_swid;
    private string m_espnS2;
    private object? m_currentView;
    public event PropertyChangedEventHandler? PropertyChanged;
    public string LeagueId
    {
        get => m_leagueId;
        set
        {
            if (m_leagueId != value)
            {
                m_leagueId = value;
                OnPropertyChanged(nameof(LeagueId));
            }
        }
    }
    public string LeagueYear
    {
        get => m_leagueYear;
        set
        {
            if (m_leagueYear != value)
            {
                m_leagueYear = value;
                OnPropertyChanged(nameof(LeagueYear));
            }
        }
    }
    public string Swid
    {
        get => m_swid;
        set
        {
            if (m_swid != value)
            {
                m_swid = value;
                OnPropertyChanged(nameof(Swid));
            }
        }
    }

    public string EspnS2
    {
        get => m_espnS2;
        set
        {
            if (m_espnS2 != value)
            {
                m_espnS2 = value;
                OnPropertyChanged(nameof(EspnS2));
            }
        }
    }

    public object? CurrentView
    {
        get => m_currentView;
        set
        {
            if (m_currentView != value)
            {
                m_currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new RelayCommand(async _ => await ExecuteLoginAsync(), CanLogin);
    }

    private bool CanLogin(object? parameter)
    {
        return !string.IsNullOrWhiteSpace(LeagueId) && !string.IsNullOrWhiteSpace(LeagueYear);
    }

    private async Task ExecuteLoginAsync()
    {
        var responseData = await UtilityFunctions.Login(LeagueId, LeagueYear, Swid, EspnS2);

        League league = new League(responseData);

        // Update the CurrentView to TeamSelect
        CurrentView = new TeamSelectViewModel(league);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

