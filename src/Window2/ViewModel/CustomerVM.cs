namespace Window2.ViewModel;

public class CustomerVM : ViewModelBase
{
    private readonly PageModel _pageModel;
    public int CustomerID
    {
        get { return _pageModel.CustomerCount; }
        set { _pageModel.CustomerCount = value; OnPropertyChanged(); }
    }

    public CustomerVM()
    {
        _pageModel = new PageModel();
        CustomerID = 100528;
    }
}