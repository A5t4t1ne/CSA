using Explorer700Library;

namespace Explorer700Demo;

public class Exp700Singleton
{
    private static readonly Explorer700 instance = new Explorer700();

    // Private constructor to prevent external instantiation
    private Exp700Singleton() { }

    // Static property to access the single instance
    public static Explorer700 Instance
    {
        get { return instance; }
    }
}
