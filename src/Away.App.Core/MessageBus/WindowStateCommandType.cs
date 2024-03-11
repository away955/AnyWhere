namespace Away.App.Core.MessageBus;

public enum WindowStateCommandType : byte
{
    Normal = 0,
    Minimized = 1,
    Maximized = 2,
    FullScreen = 3,
    Hide = 4,
    Show = 5,
    Activate = 6,
    Close = 7,
    ShowActivate = 8,
}