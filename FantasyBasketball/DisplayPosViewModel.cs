using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia.Controls;
using Commands;

namespace FantasyBasketball;

public class DisplayPosViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private readonly MainViewModel m_mainViewModel;

    public ObservableCollection<ExpanderItem> Expanders { get; set; } = new ObservableCollection<ExpanderItem>();

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public DisplayPosViewModel(MainViewModel mainViewModel, Dictionary<string, List<Player>> positions)
    {
        m_mainViewModel = mainViewModel;
        foreach(var pos in positions)
        {
            ObservableCollection<string> playerNames = new ObservableCollection<string>();
            foreach(var player in pos.Value)
            {
                playerNames.Add(player.GetName());
            }

            Expanders.Add(new ExpanderItem { Header = pos.Key, Content = playerNames});
        }
    }
}

public class ExpanderItem : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private string? m_header;
    
    public string? Header
    {
        get => m_header;
        set
        {
            if (m_header != value)
            {
                m_header = value;
                OnPropertyChanged(nameof(Header));  // Use the OnPropertyChanged method
            }
        }
    }
    
    public ObservableCollection<string> Content { get; set; } = new ObservableCollection<string>();
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}