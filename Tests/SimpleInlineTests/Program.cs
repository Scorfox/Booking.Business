using Base.Objects.Helpers;

namespace SimpleInlineTests
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Check debugger display
            ;
            var str = Cryptography.Encrypt("r4e3w2q1AZ");
            Console.WriteLine(str);
            Console.WriteLine(Cryptography.Decrypt(str));
        }
    }
}