namespace Away.Service;

public interface IDemo
{
    void Go();
}
public class Demo:IDemo
{
    public void Go()
    {
        Console.WriteLine("hello world");
    }
}
