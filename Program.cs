using System.Text;
using System.Text.RegularExpressions;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WordSearch");

            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("    WordSearch [regex]");

                return;
            }

            String fileName = @"words_alpha.txt";

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                bool found = false;
                String? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    // Process line
                    if (Regex.Match(line, args[0]).Success)
                    {
                        found = true;
                        Console.WriteLine(line);
                    }
                }

                if (!found)
                {
                    Console.WriteLine();
                    Console.WriteLine("No match for: \"" + args[0] + "\"");
                }
            }
        }
    }
}
