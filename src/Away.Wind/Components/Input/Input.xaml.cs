using MaterialDesignThemes.Wpf;


namespace Away.Wind.Components;

/// <summary>
/// Input.xaml 的交互逻辑
/// </summary>
public partial class Input : UserControl
{
    public Input()
    {
        InitializeComponent();
    }

    public readonly static DependencyProperty TextProperty;
    public readonly static DependencyProperty LabelProperty;
    static Input()
    {
        TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(Input), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));
        LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(Input), new PropertyMetadata(string.Empty, OnLableChanged));
    }

    private static void OnLableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Input control)
        {
            return;
        }
        HintAssist.SetHint(control.Txt_Input, e.NewValue);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Input control)
        {
            return;
        }
        control.Txt_Input.Text = Convert.ToString(e.NewValue) ?? string.Empty;
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(TextProperty, value); }
    }

    private void Txt_Input_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = Txt_Input.Text;
    }
}
