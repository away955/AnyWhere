﻿namespace Away.App.ViewModels;

[ViewModel]
public sealed class XrayNodeSubViewModel : ViewModelBase
{
    private readonly IXrayNodeSubRepository _repository;
    private readonly IMapper _mapper;

    [Reactive]
    public ObservableCollection<XrayNodeSubModel> Items { get; set; } = [];

    public ICommand AddCommand { get; }
    public ICommand DelCommand { get; }
    public ICommand SaveCommand { get; }

    public XrayNodeSubViewModel(
        IXrayNodeSubRepository xrayNodeSubRepository,
        IMapper mapper)
    {
        _repository = xrayNodeSubRepository;
        _mapper = mapper;

        AddCommand = ReactiveCommand.Create(OnAddCommand);
        DelCommand = ReactiveCommand.Create<XrayNodeSubModel?>(OnDelCommand);
        SaveCommand = ReactiveCommand.Create(OnSaveCommand);

        Init();
    }

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
            _repository.DeleteByUrl(model.Url);
            Show("删除成功");
        }
    }

    private void OnSaveCommand()
    {
        var entitys = Items.Where(o => !string.IsNullOrWhiteSpace(o.Url)).Select(_mapper.Map<XrayNodeSubEntity>).ToList();
        _repository.Save(entitys);
        Show("保存成功");
        Init();
    }

}