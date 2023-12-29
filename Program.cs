using System.Text;
using System.Text.RegularExpressions;

namespace WordSearch
{
    internal class Program
    {
        static void Main(String[] args)
        {
            String pattern = "";
            bool showhelp = false;
            bool showdefinition = false;
            bool limited = true;
            short minLength = 4;
            short maxLength = 20;

            for ( int loop = 0; loop < args.Length; loop++ )
            {
                switch (args[loop])
                {
                    case "-l":
                        limited = false;
                        break;

                    case "-n":
                        minLength = short.Parse(args[++loop]);
                        break;

                    case "-x":
                        maxLength = short.Parse(args[++loop]);
                        break;

                    case "-s":
                        minLength = maxLength = short.Parse(args[++loop]);
                        break;

                    case "-h":
                        showhelp = true;
                        break;

                    case "-d":
                        showdefinition = true;
                        break;

                    default:
                        pattern = args[loop];
                        break;
                };
            }

            if (showhelp || pattern.Length == 0 || minLength < 1 || maxLength < 1)
            {
                Console.WriteLine("WordSearch");

                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("    WordSearch <options> [regex]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("    -h           Show this help");
                Console.WriteLine("    -l           Do not surround the regex with ^ and $ by default");
                Console.WriteLine("    -d           Show the found words' definitions, if available");
                Console.WriteLine("    -n number    Minimum word length (default 4)");
                Console.WriteLine("    -x number    Maximum word length (default 20)");
                Console.WriteLine("    -s number    Required word length - sets minimum and maximum lengths to be the same");

                return;
            }

            // Treat the pattern as matching the whole word by surrounding with ^ and $
            // On by default
            if( limited )
            {
                if( !pattern.StartsWith('^'))
                {
                    pattern = '^' + pattern;
                }
                if (!pattern.EndsWith('$'))
                {
                    pattern = pattern + '$';
                }
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

                    if (word.Length < minLength)
                    {
                        continue;
                    }

                    if (word.Length > maxLength)
                    {
                        continue;
                    }

                    // Test word (case insensitively)
                    if (Regex.Match(word, pattern, RegexOptions.IgnoreCase).Success)
                    {
                        found = true;

                        if( showdefinition)
                        {
                            Console.WriteLine(line);
                        }
                        else
                        {
                            Console.WriteLine(word);
                        }
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
