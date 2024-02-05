namespace Away.Wind.Components;

public partial class SwitchButton : UserControl
{
    public SwitchButton()
    {
        InitializeComponent();
    }

    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    public bool IsChecked
    {
        get { return (bool)GetValue(IsCheckedProperty); }
        set { SetValue(IsCheckedProperty, value); }
    }


    public readonly static DependencyProperty LabelProperty;
    public readonly static DependencyProperty IsCheckedProperty;
    static SwitchButton()
    {
        LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(SwitchButton), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLabelChanged)));
        IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(SwitchButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChange));

    }

    private static void OnIsCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not SwitchButton control)
        {
            return;
        }

        if (Convert.ToBoolean(e.NewValue))
        {
            control.CBSwitchBtn.IsChecked = true;
            control.TxtCheckLabel.Text = "开";
        }
        else
        {
            control.CBSwitchBtn.IsEnabled = false;
            control.TxtCheckLabel.Text = "关";
        }
    }

    private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
