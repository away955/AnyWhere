namespace Away.Wind.Components;

public class MultiComboBoxModel : NotifyPropertyChangedBase
{
    public MultiComboBoxModel(string title)
    {
        Title = title;
    }

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            NotifyPropertyChanged(nameof(Title));
        }
    }

    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            NotifyPropertyChanged(nameof(IsChecked));
        }
    }
}
