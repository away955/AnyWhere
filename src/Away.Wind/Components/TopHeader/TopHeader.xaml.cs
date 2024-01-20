
using System.Windows.Input;

namespace Away.Wind.Components
{
    /// <summary>
    /// TopHeader.xaml 的交互逻辑
    /// </summary>
    public partial class TopHeader : UserControl
    {
        public readonly static DependencyProperty MenuToggleChangeCommandProperty;
        public readonly static DependencyProperty DataProperty;


        static TopHeader()
        {
            MenuToggleChangeCommandProperty = DependencyProperty.Register(nameof(MenuToggleChangeCommand), typeof(ICommand), typeof(TopHeader), new PropertyMetadata(default(ICommand)));

            DataProperty = DependencyProperty.Register(nameof(Data), typeof(ObservableCollection<TopMenuModel>), typeof(TopHeader), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

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

        /// <summary>
        /// 菜单数据
        /// </summary>
        public ObservableCollection<TopMenuModel> Data
        {
            get { return (ObservableCollection<TopMenuModel>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.MenuToggleChangeCommand?.Execute(true);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MenuToggleChangeCommand?.Execute(false);
        }


        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TopHeader p)
            {
                p.LB_Menu.ItemsSource = e.NewValue as ObservableCollection<TopMenuModel>;
            }
        }
    }
}
