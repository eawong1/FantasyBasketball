using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia.Controls;
using Commands;

namespace FantasyBasketball;

public class FunctionSelectViewModel : INotifyPropertyChanged
{
    private string m_teamName;
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly MainViewModel m_mainViewModel;
    private League m_league;
    private string m_prompt;
    public string? Prompt
    {
        get => m_prompt;
        set
        {
            if(m_prompt != value)
            {
                m_prompt = value;
                OnPropertyChanged(nameof(Prompt));
            }
        }
    }
    public ICommand GetPosCommand { get; }

    public FunctionSelectViewModel(MainViewModel mainViewModel, League league, string teamName)
    {
        m_mainViewModel = mainViewModel;
        m_league = league;
        m_teamName = teamName;
        Prompt = $"What woud you like to do with Team\n {m_teamName}";
        GetPosCommand = new RelayCommand(() => ExecuteGetPos(), () => { return true; });
    }

    public void ExecuteGetPos()
    {
        var positions = m_league.GetPositions(m_teamName);

        m_mainViewModel.CurrentView = new DisplayPosViewModel(m_mainViewModel, positions);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}