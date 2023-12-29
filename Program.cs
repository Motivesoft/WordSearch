using System.Text;
using System.Text.RegularExpressions;

namespace WordSearch
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("WordSearch");

                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("    WordSearch [regex]");

                return;
            }

            String fileName = @"NWL2020.txt";

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                bool found = false;
                String? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    // Some lists, like the NWL Scrabble word lists, have the word and its definition on the same line
                    // Split the word out
                    String word = line.Split(' ')[0];

                    // Test word (case insensitively)
                    if (Regex.Match(word, args[0], RegexOptions.IgnoreCase).Success)
                    {
                        found = true;
                        Console.WriteLine(word);
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
