using Away.Service.Xray;

namespace Away.Wind.ViewModels;

public class XrayServerViewModel : BindableBase
{

    public XrayServerViewModel()
    {
        UpdateNodeCommand = new DelegateCommand(OnUpdateNodeCommand);

    }



    public DelegateCommand UpdateNodeCommand { get; private set; }
    public void OnUpdateNodeCommand()
    {
        //_xrayNodeService.GetXrayNodeByUrl("https://bulinkbulink.com/freefq/free/master/v2");
    }

}
