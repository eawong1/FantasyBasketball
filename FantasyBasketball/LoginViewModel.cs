using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Commands;
using Utilities;

namespace FantasyBasketball;
public class LoginViewModel : INotifyPropertyChanged
{
    private readonly MainViewModel m_mainViewModel;
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
                //notify Login Command that LeagueId textbox is populated
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
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
                //notify Login Command that LeagueYear textbox is populated
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
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

    public LoginViewModel(MainViewModel mainViewModel)
    {
        m_mainViewModel = mainViewModel;
        LoginCommand = new RelayCommand(() => ExecuteLoginAsync(), CanLogin);
    }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(LeagueId) && !string.IsNullOrWhiteSpace(LeagueYear);
    }

    private async Task ExecuteLoginAsync()
    {
        var responseData = await UtilityFunctions.Login(LeagueId, LeagueYear, Swid, EspnS2);

        League league = new League(responseData);

        // Update the CurrentView to TeamSelect
        m_mainViewModel.CurrentView = new TeamSelectViewModel(m_mainViewModel, league);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

