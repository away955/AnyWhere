
using System.Windows.Input;

namespace Away.Wind.Components
{
    /// <summary>
    /// TopHeader.xaml 的交互逻辑
    /// </summary>
    public partial class TopHeader : UserControl
    {
        public readonly static DependencyProperty MenuToggleChangeCommandProperty;

        static TopHeader()
        {
            MenuToggleChangeCommandProperty = DependencyProperty.Register(nameof(MenuToggleChangeCommand), typeof(ICommand), typeof(TopHeader), new PropertyMetadata(default(ICommand)));
        }

        public TopHeader()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 菜单展开|收起
        /// </summary>
        public ICommand MenuToggleChangeCommand
        {
            get { return (ICommand)GetValue(MenuToggleChangeCommandProperty); }
            set { SetValue(MenuToggleChangeCommandProperty, value); }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MenuToggleChangeCommand?.Execute(true);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MenuToggleChangeCommand?.Execute(false);
        }
    }
}
