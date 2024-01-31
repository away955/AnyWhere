namespace Away.Wind.Components;

/// <summary>
/// SwitchButton.xaml 的交互逻辑
/// </summary>
public partial class SwitchButton : UserControl
{
    public SwitchButton()
    {
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }


    public readonly static DependencyProperty TextProperty;
    public readonly static DependencyProperty IsCheckedProperty;
    static SwitchButton()
    {
        TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(SwitchButton), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTextChanged)));
        IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(SwitchButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SwitchButton control)
        {
            return;
        }
        control.TxtLabel.Text = Convert.ToString(e.NewValue);
    }

    private void CBSwitchBtn_Checked(object sender, RoutedEventArgs e)
    {
        this.IsChecked = true;
        TxtCheckLabel.Text = "开";
    }

    private void CBSwitchBtn_Unchecked(object sender, RoutedEventArgs e)
    {
        this.IsChecked = false;
        TxtCheckLabel.Text = "关";
    }
}
