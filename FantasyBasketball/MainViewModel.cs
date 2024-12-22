

using System.ComponentModel;


namespace FantasyBasketball;
public class MainViewModel : INotifyPropertyChanged
{
    private object? _currentView;

    public event PropertyChangedEventHandler? PropertyChanged;

    public object? CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView != value)
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
    }

    public MainViewModel()
    {
        // Start with the TeamSelect view
        CurrentView = new LoginViewModel();
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
