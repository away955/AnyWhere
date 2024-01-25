using System.Windows.Media.Imaging;

namespace Away.Wind.Components;

/// <summary>
/// NofityIcon.xaml 的交互逻辑
/// </summary>
public partial class NofityIcon : UserControl
{
    public readonly static DependencyProperty TitleProperty;
    public readonly static DependencyProperty IconProperty;
    public readonly static DependencyProperty DataProperty;

    static NofityIcon()
    {
        TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(NofityIcon), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTitleChanged)));
        IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string), typeof(NofityIcon), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnIconChanged)));
        DataProperty = DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<TopMenuModel>), typeof(NofityIcon), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NofityIcon p)
        {
            if (e.NewValue == null)
            {
                return;
            }
            p.TBI.IconSource = new BitmapImage(new Uri(Convert.ToString(e.NewValue) ?? string.Empty, UriKind.Absolute));
        }
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NofityIcon p)
        {
            p.TBI.ToolTipText = Convert.ToString(e.NewValue);
        }
    }

    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is NofityIcon p)
        {
            p.TB_Menu.ItemsSource = e.NewValue as ObservableCollection<TopMenuModel>;
        }
    }

    public NofityIcon()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    /// <summary>
    /// 图标
    /// </summary>
    public string Icon
    {
        get { return (string)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    /// <summary>
    /// 菜单数据
    /// </summary>
    public ObservableCollection<TopMenuModel> Data
    {
        get { return (ObservableCollection<TopMenuModel>)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }
}
