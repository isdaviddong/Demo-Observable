using System;

public class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding=System.Text.Encoding.UTF8;
        
        // 建立一個可被觀察的主題
        Subject subject = new Subject();

        // 建立兩個觀察者
        Observer observer1 = new Observer("觀察者(訂閱者) A");
        Observer observer2 = new Observer("觀察者(訂閱者) B");

        // 註冊觀察者
        IDisposable sub1 = subject.Subscribe(observer1);
        IDisposable sub2 = subject.Subscribe(observer2);

        // 發送一條訊息給觀察者
        subject.SendMessage("Hello!");

        // 取消註冊一個觀察者
        sub1.Dispose();

        // 發送另一條訊息給觀察者
        subject.SendMessage("嗨嗨嗨!");

        // 等待用戶輸入
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}

// 實現IObservable介面的主題類別
public class Subject : IObservable<string>
{
    private List<IObserver<string>> observers;

    public Subject()
    {
        observers = new List<IObserver<string>>();
    }

    public IDisposable Subscribe(IObserver<string> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);

        return new Unsubscriber(observers, observer);
    }

    public void SendMessage(string message)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(message);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<string>> _observers;
        private IObserver<string> _observer;

        public Unsubscriber(List<IObserver<string>> observers, IObserver<string> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}

// 實現IObserver介面的觀察者類別
public class Observer : IObserver<string>
{
    private string name;

    public Observer(string name)
    {
        this.name = name;
    }

    public void OnCompleted()
    {
        Console.WriteLine("Observable completed.");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine("An error occurred: " + error.Message);
    }

    public void OnNext(string value)
    {
        Console.WriteLine(name + " received message: " + value);
    }
}