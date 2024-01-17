using System.Windows.Documents;
using System.Windows.Input;

namespace Away.Wind.Components
{
    /// <summary>
    /// Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FirstPageCommandProperty;
        public ICommand FirstPageCommand
        {
            get { return (ICommand)GetValue(FirstPageCommandProperty); }
            set { SetValue(FirstPageCommandProperty, value); }
        }

        public static readonly DependencyProperty LastPageCommandProperty;
        public ICommand LastPageCommand
        {
            get { return (ICommand)GetValue(LastPageCommandProperty); }
            set { SetValue(LastPageCommandProperty, value); }
        }

        public static readonly DependencyProperty CurrentPageProperty;
        public string CurrentPage
        {
            get { return (string)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }


        public static readonly DependencyProperty TotalPageProperty;
        public string TotalPage
        {
            get { return (string)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }


        static Pagination()
        {
            FirstPageCommandProperty = DependencyProperty.Register(nameof(FirstPageCommand), typeof(ICommand), typeof(Pagination), new PropertyMetadata(default(ICommand)));
            LastPageCommandProperty = DependencyProperty.Register(nameof(LastPageCommand), typeof(ICommand), typeof(Pagination), new PropertyMetadata(default(ICommand)));


            CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(string), typeof(Pagination), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnCurrentPageChanged)));
            TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(string), typeof(Pagination), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTotalPageChanged)));
        }




        public static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination p)
            {
                Run rTotal = (Run)p.FindName("rTotal");

                rTotal.Text = (string)e.NewValue;
            }
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Pagination p)
            {
                Run rCurrrent = (Run)p.FindName("rCurrent");

                rCurrrent.Text = (string)e.NewValue;
            }
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            LastPageCommand?.Execute(this);
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            FirstPageCommand?.Execute(this);
        }
    }
}
