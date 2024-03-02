namespace Away.Wind.ViewModels;

public class XrayNodeSubSettingsVM : BindableBase
{
    private readonly IXrayNodeSubRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;

    public XrayNodeSubSettingsVM(
        IXrayNodeSubRepository xrayNodeSubRepository,
        IMapper mapper,
        IMessageService messageService
        )
    {
        _repository = xrayNodeSubRepository;
        _mapper = mapper;
        _messageService = messageService;

        AddCommand = new(OnAddCommand);
        DelCommand = new(OnDelCommand);
        SaveCommand = new(OnSaveCommand);

        Init();
    }


    private ObservableCollection<XrayNodeSubModel> _items = [];
    public ObservableCollection<XrayNodeSubModel> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public DelegateCommand AddCommand { get; private set; }
    public DelegateCommand<XrayNodeSubModel?> DelCommand { get; private set; }
    public DelegateCommand SaveCommand { get; private set; }

    private void Init()
    {
        var nodeSubItems = _repository.GetList().Select(_mapper.Map<XrayNodeSubModel>);
        Items = new ObservableCollection<XrayNodeSubModel>(nodeSubItems);
    }

    public void OnAddCommand()
    {
        Items.Add(new XrayNodeSubModel());
    }

    private void OnDelCommand(XrayNodeSubModel? model)
    {
        if (model == null)
        {
            return;
        }
        Items.Remove(model);
        if (model.Id > 0)
        {
            _repository.DeleteById(model.Id);
            _messageService.Show("删除成功");
        }
    }

    private void OnSaveCommand()
    {
        var entitys = Items.Where(o => !string.IsNullOrWhiteSpace(o.Url)).Select(_mapper.Map<XrayNodeSubEntity>).ToList();
        var flag = _repository.InsertOrUpdate(entitys);
        if (flag)
        {
            _messageService.Show("保存成功");
            Init();
        }
        else
        {
            _messageService.Show("保存失败");
        }
    }

}
