# WordSearch
Simple command line utility to search a wordlist for a match to a provided regex, such as "ab..t"

The word match is done case-insensitively against a word list.

It may be advisable to put complex regex searches in quotes to avoid command line processing 

## Word Lists
Any can be used, but the expectation is that the list is plain text with each word as the first item on a line. Additional text on the same line is ignored.

The reason for handling additional information on the line is that some word lists have both the word and its definition on the same line.

The software should work with non-English languages that have a character set that could be processed as input to a regular expression.

Example word lists include the following:
* [NASPA word list](https://github.com/scrabblewords/scrabblewords), as used by Scrabble, e.g.
  * North-American word list, [NWL2020](https://github.com/scrabblewords/scrabblewords/blob/main/words/North-American/NSWL2020.txt)
  * North-American word list filtered for certain inappropriate words, [NSWL2020](https://github.com/scrabblewords/scrabblewords/blob/main/words/North-American/NSWL2020.txt)
* The ```words``` dictionary available as a Linux package that installs into ```/etc/share/dict``` or ```/etc/dict```
* [English Words](https://github.com/dwyl/english-words), e.g.
  * [words_alpha](https://github.com/dwyl/english-words/bob/master/words_alpha.txt) 
* [Natural Language Corpus Data](https://norvig.com/ngrams/)
  * e.g. The [ENABLE](https://norvig.com/ngrams/enable1.txt) list

__IMPORTANT NOTE:__ The list above may include files that are not public domain. Permission should be sought from their owner before use or redistribution unless its license information is readily available.

__Do not commit a word list to this project unless its provenance is known and there is permission to do so.__

## To Do
The following additional things could be done to this project:
* Add an optional (but default) behaviour to treat command line input as all-inclusive - e.g. surround with ```^``` and ```$```
* Have an option to display both the matching word and its definition (if present)
* Have an option to provide a word list on the command line
* Have an inbuilt or file for excluded words (similar to the Squaredle one, in its closure javascript?)
* Have an optional/configurable minimum, maximum or specific length to search for
* Show version, command line help
* Build for Linux
* Rebuild in C++