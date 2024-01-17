using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Away.Wind.Components;

/// <summary>
/// LeftMenu.xaml 的交互逻辑
/// </summary>
public partial class LeftMenu : UserControl
{
    public LeftMenu()
    {
        InitializeComponent();
    }

    private readonly static DependencyProperty LogoTitleProperty;
    private readonly static DependencyProperty DataProperty;
    public static readonly DependencyProperty SelectedCommandProperty;

    /// <summary>
    /// Logo标题
    /// </summary>
    public string LogoTitle
    {
        get { return (string)GetValue(LogoTitleProperty); }
        set { SetValue(LogoTitleProperty, value); }
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

    static LeftMenu()
    {
        LogoTitleProperty = DependencyProperty.Register(nameof(LogoTitle), typeof(string), typeof(LeftMenu), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnLogoTitleChanged)));
        DataProperty = DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<MenuModel>), typeof(LeftMenu), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged))); ;
        SelectedCommandProperty = DependencyProperty.Register(nameof(SelectedCommand), typeof(ICommand), typeof(LeftMenu), new PropertyMetadata(default(ICommand)));
    }

    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            p.LV.ItemsSource = e.NewValue as ObservableCollection<MenuModel>;
        }
    }

    private static void OnLogoTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LeftMenu p)
        {
            p.logoTitle.Text = Convert.ToString(e.NewValue);
        }
    }

    private void LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var model = e.AddedItems[0] as MenuModel;
        SelectedCommand?.Execute(model?.URL);
    }
}


