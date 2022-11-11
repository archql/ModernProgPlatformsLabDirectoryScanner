using lab3DirectoryScanner.DirectoryScanner;

namespace lab3DirectoryScanner.ConsoleExample
{
    public class Program
    {
        public static void Main()
        {
            DirScanner scanner = new DirScanner(Path.GetFullPath("../../../../../../")); // test/

            scanner.Start(5);
            scanner.WaitForCompletion();

            var result = scanner.Result();
            result.CalcSize();
            result.PrintToConsole();
        }
    }
}
