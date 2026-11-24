using Katzi.Package;

internal static class Program
{
    public static void Main(string[] args)
    {
        VerifyCommand verify = new("/home/katzi/KatziPackage");
        verify.Print();
    }
}
