namespace Window2.ViewModel;

public class ProductVM : ViewModelBase
{
    private readonly PageModel _pageModel;
    public string? ProductAvailability
    {
        get => _pageModel.ProductStatus;
        set { _pageModel.ProductStatus = value; OnPropertyChanged(); }
    }

    public ProductVM()
    {
        _pageModel = new PageModel();
        ProductAvailability = "Out of Stock";
    }
}