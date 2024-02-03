using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Away.Wind.Views.Systems.ViewModels;

public class SystemThemeSettingsVM : BindableBase, IDialogAware
{
    private Color? _primaryColor;
    private Color? _secondaryColor;
    private Color? _primaryForegroundColor;
    private Color? _secondaryForegroundColor;
    private readonly PaletteHelper _paletteHelper;

    public SystemThemeSettingsVM()
    {
        ToggleBaseCommand = new(OnToggleBaseCommand);
        ChangeCustomHueCommand = new(OnChangeCustomHueCommand);
        ChangeToPrimaryCommand = new(o => OnChangeScheme(ColorScheme.Primary));
        ChangeToSecondaryCommand = new(o => OnChangeScheme(ColorScheme.Secondary));
        ChangeToPrimaryForegroundCommand = new(o => OnChangeScheme(ColorScheme.PrimaryForeground));
        ChangeToSecondaryForegroundCommand = new(o => OnChangeScheme(ColorScheme.SecondaryForeground));

        _paletteHelper = new();
        var theme = _paletteHelper.GetTheme();
        _primaryColor = theme.PrimaryMid.Color;
        _secondaryColor = theme.SecondaryMid.Color;
        _selectedColor = _primaryColor;


        IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

        if (theme is Theme internalTheme)
        {
            _isColorAdjusted = internalTheme.ColorAdjustment is not null;

            var colorAdjustment = internalTheme.ColorAdjustment ?? new ColorAdjustment();
            _desiredContrastRatio = colorAdjustment.DesiredContrastRatio;
            _contrastValue = colorAdjustment.Contrast;
            _colorSelectionValue = colorAdjustment.Colors;
        }

        if (_paletteHelper.GetThemeManager() is { } themeManager)
        {
            themeManager.ThemeChanged += (_, e) =>
            {
                IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
            };
        }
    }

    #region 模块1：主题切换、色彩对比度调整

    private bool _isDarkTheme;
    /// <summary>
    /// 是否暗黑主题
    /// </summary>
    public bool IsDarkTheme
    {
        get => _isDarkTheme;
        set
        {
            if (SetProperty(ref _isDarkTheme, value))
            {
                ModifyTheme(theme => theme.SetBaseTheme(value ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme()));
            }
        }
    }

    private bool _isColorAdjusted;
    /// <summary>
    /// 是否调整颜色
    /// </summary>
    public bool IsColorAdjusted
    {
        get => _isColorAdjusted;
        set
        {
            if (SetProperty(ref _isColorAdjusted, value))
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme)
                    {
                        internalTheme.ColorAdjustment = value
                            ? new ColorAdjustment
                            {
                                DesiredContrastRatio = DesiredContrastRatio,
                                Contrast = ContrastValue,
                                Colors = ColorSelectionValue
                            }
                            : null;
                    }
                });
            }
        }
    }

    private float _desiredContrastRatio = 4.5f;
    /// <summary>
    /// 对比度
    /// </summary>
    public float DesiredContrastRatio
    {
        get => _desiredContrastRatio;
        set
        {
            if (SetProperty(ref _desiredContrastRatio, value))
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                        internalTheme.ColorAdjustment.DesiredContrastRatio = value;
                });
            }
        }
    }

    public static IEnumerable<Contrast> ContrastValues => Enum.GetValues(typeof(Contrast)).Cast<Contrast>();

    private Contrast _contrastValue;
    /// <summary>
    /// 对比度差异强度
    /// </summary>
    public Contrast ContrastValue
    {
        get => _contrastValue;
        set
        {
            if (SetProperty(ref _contrastValue, value))
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                        internalTheme.ColorAdjustment.Contrast = value;
                });
            }
        }
    }

    public static IEnumerable<ColorSelection> ColorSelectionValues => Enum.GetValues(typeof(ColorSelection)).Cast<ColorSelection>();

    private ColorSelection _colorSelectionValue;
    /// <summary>
    /// 颜色主题选择
    /// </summary>
    public ColorSelection ColorSelectionValue
    {
        get => _colorSelectionValue;
        set
        {
            if (SetProperty(ref _colorSelectionValue, value))
            {
                ModifyTheme(theme =>
                {
                    if (theme is Theme internalTheme && internalTheme.ColorAdjustment != null)
                    {
                        internalTheme.ColorAdjustment.Colors = value;
                    }
                });
            }
        }
    }

    #endregion

    #region 模块2：颜色选择

    private ColorScheme _activeScheme;
    /// <summary>
    /// 当前主题色类型
    /// </summary>
    public ColorScheme ActiveScheme { get => _activeScheme; set => SetProperty(ref _activeScheme, value); }


    private Color? _selectedColor;
    /// <summary>
    /// 当前选中的主题色
    /// </summary>
    public Color? SelectedColor
    {
        get => _selectedColor;
        set
        {
            if (_selectedColor == value)
            {
                return;
            }

            SetProperty(ref _selectedColor, value);
            var currentSchemeColor = ActiveScheme switch
            {
                ColorScheme.Primary => _primaryColor,
                ColorScheme.Secondary => _secondaryColor,
                ColorScheme.PrimaryForeground => _primaryForegroundColor,
                ColorScheme.SecondaryForeground => _secondaryForegroundColor,
                _ => throw new NotSupportedException($"{ActiveScheme} is not a handled ColorScheme.. Ye daft programmer!")
            };

            if (_selectedColor != currentSchemeColor && value is Color color)
            {
                OnChangeCustomHueCommand(color);
            }

        }
    }

    /// <summary>
    /// 切换主题：明亮|暗黑
    /// </summary>
    public DelegateCommand<bool?> ToggleBaseCommand { get; }
    private void OnToggleBaseCommand(bool? darkMode)
    {
        if (darkMode == null)
        {
            return;
        }
        ITheme theme = _paletteHelper.GetTheme();
        IBaseTheme baseTheme = darkMode.Value ? new MaterialDesignDarkTheme() : new MaterialDesignLightTheme();
        theme.SetBaseTheme(baseTheme);
        _paletteHelper.SetTheme(theme);
    }

    /// <summary>
    /// 自定义主题色
    /// </summary>
    public DelegateCommand<Color?> ChangeCustomHueCommand { get; }
    private void OnChangeCustomHueCommand(Color? color)
    {
        if (color == null)
        {
            return;
        }

        if (ActiveScheme == ColorScheme.Primary)
        {
            _primaryColor = color;
            ModifyTheme(theme => theme.SetPrimaryColor(color.Value));

        }
        else if (ActiveScheme == ColorScheme.Secondary)
        {
            _secondaryColor = color;
            ModifyTheme(theme => theme.SetSecondaryColor(color.Value));
        }
        else if (ActiveScheme == ColorScheme.PrimaryForeground)
        {
            _primaryForegroundColor = color;
            ModifyTheme(theme =>
            {
                theme.PrimaryLight = new ColorPair(theme.PrimaryLight.Color, color);
                theme.PrimaryMid = new ColorPair(theme.PrimaryMid.Color, color);
                theme.PrimaryDark = new ColorPair(theme.PrimaryDark.Color, color);
            });
        }
        else if (ActiveScheme == ColorScheme.SecondaryForeground)
        {
            _secondaryForegroundColor = color;
            ModifyTheme(theme =>
            {
                theme.SecondaryLight = new ColorPair(theme.SecondaryLight.Color, color);
                theme.SecondaryMid = new ColorPair(theme.SecondaryMid.Color, color);
                theme.SecondaryDark = new ColorPair(theme.SecondaryDark.Color, color);
            });
        }
    }

    /// <summary>
    /// 修改主主题色
    /// </summary>
    public DelegateCommand<Color?> ChangeToPrimaryCommand { get; }
    /// <summary>
    /// 修改次主题色
    /// </summary>
    public DelegateCommand<Color?> ChangeToSecondaryCommand { get; }
    /// <summary>
    ///  修改主主题前景色
    /// </summary>
    public DelegateCommand<Color?> ChangeToPrimaryForegroundCommand { get; }
    /// <summary>
    /// 修改次主题前景色
    /// </summary>
    public DelegateCommand<Color?> ChangeToSecondaryForegroundCommand { get; }
    private void OnChangeScheme(ColorScheme scheme)
    {
        ActiveScheme = scheme;
        if (ActiveScheme == ColorScheme.Primary)
        {
            SelectedColor = _primaryColor;
        }
        else if (ActiveScheme == ColorScheme.Secondary)
        {
            SelectedColor = _secondaryColor;
        }
        else if (ActiveScheme == ColorScheme.PrimaryForeground)
        {
            SelectedColor = _primaryForegroundColor;
        }
        else if (ActiveScheme == ColorScheme.SecondaryForeground)
        {
            SelectedColor = _secondaryForegroundColor;
        }
    }

    private void ModifyTheme(Action<ITheme> modificationAction)
    {
        var theme = _paletteHelper.GetTheme();
        modificationAction?.Invoke(theme);
        _paletteHelper.SetTheme(theme);
    }
    #endregion

    #region 弹窗配置

    public string Title => "主题色配置";

    public event Action<IDialogResult> RequestClose = null!;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {

    }

    public void OnDialogOpened(IDialogParameters parameters)
    {

    }

    #endregion
}

/// <summary>
/// 主题色类型
/// </summary>
public enum ColorScheme
{
    Primary,
    Secondary,
    PrimaryForeground,
    SecondaryForeground
}

public class MultiValueEqualityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values?.All(o => o?.Equals(values[0]) == true) == true || values?.All(o => o == null) == true;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

[ValueConversion(typeof(Color), typeof(Brush))]
public class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            SolidColorBrush rv = new(color);
            rv.Freeze();
            return rv;
        }
        return Binding.DoNothing;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
        {
            return brush.Color;
        }
        return default(Color);
    }
}

public class BrushToHexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;

        static string lowerHexString(int i) => i.ToString("X2").ToLower();
        var brush = (SolidColorBrush)value;
        var hex = lowerHexString(brush.Color.R) +
                  lowerHexString(brush.Color.G) +
                  lowerHexString(brush.Color.B);
        return "#" + hex;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}