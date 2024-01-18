using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Away.Wind.Components;

/// <summary>
/// LeftMenu.xaml 的交互逻辑
/// </summary>
public partial class LeftMenu : UserControl
{
    public readonly static DependencyProperty LogoProperty;
    public readonly static DependencyProperty LogoTitleProperty;
    public readonly static DependencyProperty MenuToggleProperty;
    public readonly static DependencyProperty DataProperty;
    public readonly static DependencyProperty SelectedCommandProperty;

    static LeftMenu()
    {
        LogoProperty = DependencyProperty.Register(nameof(Logo), typeof(string), typeof(LeftMenu), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLogoChanged)));
        LogoTitleProperty = DependencyProperty.Register(nameof(LogoTitle), typeof(string), typeof(LeftMenu), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLogoTitleChanged)));
        MenuToggleProperty = DependencyProperty.Register(nameof(MenuToggle), typeof(bool), typeof(LeftMenu), new PropertyMetadata(false, new PropertyChangedCallback(OnMenuToggleChanged)));

        DataProperty = DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<MenuModel>), typeof(LeftMenu), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));
        SelectedCommandProperty = DependencyProperty.Register(nameof(SelectedCommand), typeof(ICommand), typeof(LeftMenu), new PropertyMetadata(default(ICommand)));
    }

    private static void OnLogoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            p.ImgLogo.Source = new BitmapImage(new Uri(Convert.ToString(e.NewValue) ?? string.Empty, UriKind.Absolute));
        }
    }

    public LeftMenu()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Logo图片
    /// </summary>
    public string Logo
    {
        get { return (string)GetValue(LogoProperty); }
        set { SetValue(LogoProperty, value); }
    }

    /// <summary>
    /// Logo标题
    /// </summary>
    public string LogoTitle
    {
        get { return (string)GetValue(LogoTitleProperty); }
        set { SetValue(LogoTitleProperty, value); }
    }

    /// <summary>
    /// 菜单展开|收起
    /// </summary>
    public bool MenuToggle
    {
        get { return (bool)GetValue(MenuToggleProperty); }
        set { SetValue(MenuToggleProperty, value); }
    }

    /// <summary>
    /// 菜单数据源
    /// </summary>
    public ObservableCollection<MenuModel> Data
    {
        get { return (ObservableCollection<MenuModel>)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    /// <summary>
    /// 选中菜单
    /// </summary>
    public ICommand SelectedCommand
    {
        get { return (ICommand)GetValue(SelectedCommandProperty); }
        set { SetValue(SelectedCommandProperty, value); }
    }

    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            p.MenuListView.ItemsSource = e.NewValue as ObservableCollection<MenuModel>;
        }
    }

    private static void OnLogoTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            p.LogoTitleText.Text = Convert.ToString(e.NewValue);
        }
    }

    private static void OnMenuToggleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            var isShow = Convert.ToBoolean(e.NewValue);
            if (isShow)
            {
                ((Storyboard)p.FindResource("ShowMenu")).Begin();
            }
            else
            {
                ((Storyboard)p.FindResource("HideMenu")).Begin();
            }
        }
    }

    private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var model = e.AddedItems[0] as MenuModel;
        SelectedCommand?.Execute(model?.URL);
    }

    private void MenuScroll_Loaded(object sender, RoutedEventArgs e)
    {
        MenuListView.AddHandler(MouseWheelEvent, new RoutedEventHandler(MyMouseWheelH), true);
    }

    private void MyMouseWheelH(object sender, RoutedEventArgs e)
    {
        MouseWheelEventArgs eargs = (MouseWheelEventArgs)e;
        double x = (double)eargs.Delta;
        double y = MenuScroll.VerticalOffset;
        MenuScroll.ScrollToVerticalOffset(y - x);
    }
}


