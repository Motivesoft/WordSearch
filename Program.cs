using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace WordSearch
{
    internal class Program
    {
        static void Main(String[] args)
        {
            String prototype = "";
            String alphabet = "";
            String musts = "";
            bool showhelp = false;
            bool showdefinition = false;
            bool includebadwords = true;
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

                    case "-a":
                        alphabet = args[++loop];
                        break;

                    case "-m":
                        musts = args[++loop];
                        break;

                    case "-h":
                        showhelp = true;
                        break;

                    case "-d":
                        showdefinition = true;
                        break;

                    case "-e":
                        includebadwords = false;
                        break;

                    default:
                        prototype = args[loop];
                        break;
                };
            }

            if (showhelp || prototype.Length == 0 || minLength < 1 || maxLength < 1)
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
                Console.WriteLine("    -e           Exclude bad words from the search");
                Console.WriteLine("    -n number    Minimum word length (default 4)");
                Console.WriteLine("    -x number    Maximum word length (default 20)");
                Console.WriteLine("    -s number    Required word length - sets minimum and maximum lengths to be the same");
                Console.WriteLine("    -a letters   Look for words made only from this subset of letters");
                Console.WriteLine("    -m letters   Look for words that definitely containing these letters");

                return;
            }

            // Treat the pattern as matching the whole word by surrounding with ^ and $
            // On by default
            string pattern = prototype;
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

            // Discover which wordlist we have
            var files = Directory.GetFiles(".", "nwl*.txt");
            if( files.Length == 0 )
            {
                Console.WriteLine("No word list (nwl*.txt) found");
                return;
            }
            String fileName = files[0];

            // Discover which bad word list we have, if any
            List<String> badwords = new List<String>();

            files = Directory.GetFiles(".", "bad*.txt");
            if( files.Length > 0 )
            {
                String badwordsFileName = files[0];

                const Int32 BufferSize = 128;

                if( !includebadwords ) 
                { 
                    using (var fileStream = File.OpenRead(badwordsFileName))
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String? line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            badwords.Add(line);
                        }
                    }
                }
            }
            else if( !includebadwords )
            {
                Console.WriteLine("No bad word list (bad*.txt) found");
                return;
            }
            
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

                    // Allow only a subset of the normal alphabet to allow for excluded letters
                    if( !String.IsNullOrEmpty(alphabet) )
                    {
                        bool valid = true;
                        foreach( char letter in word )
                        {
                            if( !alphabet.Contains(letter,StringComparison.InvariantCultureIgnoreCase))
                            {
                                valid = false;
                                break;
                            }
                        }

                        if ( !valid ) 
                        {
                            continue;
                        }
                    }

                    // Make sure all of the letters in 'musts' are in the found word
                    if (!String.IsNullOrEmpty(musts))
                    {
                        String mustWorker = musts;
                        String wordWorker = word;
                        bool valid = true;
                        while (!String.IsNullOrEmpty(mustWorker))
                        {
                            char letter = mustWorker[0];
                            int index = wordWorker.IndexOf(letter, StringComparison.InvariantCultureIgnoreCase);
                            if (index == -1)
                            {
                                valid = false;
                                break;
                            }
                            else
                            {
                                wordWorker = wordWorker.Remove(index,1);
                            }

                            mustWorker = mustWorker.Substring(1);
                        }

                        if (!valid)
                        {
                            continue;
                        }
                    }

                    // Test word (case insensitively)
                    if (Regex.Match(word, pattern, RegexOptions.IgnoreCase).Success)
                    {
                        // Make sure this isn't a bad word
                        bool excluded = false;
                        if( !includebadwords )
                        {
                            // Exclude any matches in the bad words list
                            foreach( String badword in badwords )
                            {
                                if( word.ToLowerInvariant() == badword.ToLowerInvariant() ) 
                                { 
                                    excluded = true;
                                    break;
                                }
                            }
                        }

                        if( excluded ) 
                        {
                            continue;
                        }

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
                    Console.WriteLine("No match for: \"" + prototype + "\"");
                }
            }
        }
    }
}
