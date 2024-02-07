using System.Windows.Input;

namespace Away.Wind.Views;

[Navigation]
[ViewModel(typeof(MainWindowViewModel))]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Loaded += MainWindow_Loaded;
    }

    private void TopHeader_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm)
        {
            return;
        }
        vm.TopHeaderVM.PropertyChanged += TopHeaderVM_PropertyChanged;
        vm.TaskBarIconVM.PropertyChanged += TaskBarIconVM_PropertyChanged;
    }

    private void TaskBarIconVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TaskBarIconVM vm)
        {
            return;
        }
        if (e.PropertyName == nameof(TaskBarIconVM.IsShow))
        {
            this.Show();
            vm.IsShow = false;
            return;
        }
        if (e.PropertyName == nameof(TaskBarIconVM.IsClose))
        {
            this.Close();
            return;
        }
    }

    private void TopHeaderVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not TopHeaderVM vm)
        {
            return;
        }
        if (e.PropertyName == nameof(TopHeaderVM.WindowState))
        {
            WindowState = vm.WindowState;
            return;
        }
        if (e.PropertyName == nameof(TopHeaderVM.IsHide))
        {
            if (vm.IsHide)
            {
                this.Hide();
                vm.IsHide = false;
            }
            return;
        }
    }
}