using MikrosClient.ProfanityFilter.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MikrosClient.ProfanityFilter
{
    /// <summary>
    ///
    /// This class will detect profanity and racial slurs contained within some text and return an indication flag.
    /// All words are treated as case insensitive.
    ///
    /// </summary>
    internal sealed class ProfanityHandler : ProfanityBase, IProfanityFilter
    {
        /// <summary>
        /// Default constructor that loads up the default profanity list.
        /// </summary>
        public ProfanityHandler()
        {
            AllowList = new AllowList();
        }

        /// <summary>
        /// Constructor overload that allows you to construct the filter with a custom
        /// profanity list.
        /// </summary>
        /// <param name="profanityList">Array of words to add into the filter.</param>
        public ProfanityHandler(string[] profanityList) : base(profanityList)
        {
            AllowList = new AllowList();
        }

        /// <summary>
        /// Constructor overload that allows you to construct the filter with a custom
        /// profanity list.
        /// </summary>
        /// <param name="profanityList">List of words to add into the filter.</param>
        public ProfanityHandler(List<string> profanityList) : base(profanityList)
        {
            AllowList = new AllowList();
        }

        /// <summary>
        /// Return the allow list;
        /// </summary>
        public IAllowList AllowList { get; }

        /// <summary>
        /// Check whether a specific word is in the profanity list. IsProfanity will first
        /// check if the word exists on the allow list. If it is on the allow list, then false
        /// will be returned.
        /// </summary>
        /// <param name="word">The word to check in the profanity list.</param>
        /// <returns>True if the word is considered a profanity, False otherwise.</returns>
        public bool IsProfanity(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }

            // Check if the word is in the allow list.
            if (AllowList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
            {
                return false;
            }

            return _profanities.Contains(word.ToLower(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// For a given sentence, return a list of all the detected profanities,
        /// without removing duplicate partial matches.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public ReadOnlyCollection<string> DetectAllProfanities(string sentence)
        {
            return DetectAllProfanities(sentence, false);
        }

        /// <summary>
        /// For a given sentence, return a list of all the detected profanities.
        /// </summary>
        /// <param name="sentence">The sentence to check for profanities.</param>
        /// <param name="removePartialMatches">Remove duplicate partial matches.</param>
        /// <returns>A read only list of detected profanities.</returns>
        public ReadOnlyCollection<string> DetectAllProfanities(string sentence, bool removePartialMatches)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return new ReadOnlyCollection<string>(new List<string>());
            }

            sentence = sentence.ToLower();
            sentence = sentence.Replace(".", "");
            sentence = sentence.Replace(",", "");

            var words = sentence.Split(' ');
            var postAllowList = FilterWordListByAllowList(words);
            List<string> swearList = new List<string>();

            // Catch whether multi-word profanities are in the allow list filtered sentence.
            AddMultiWordProfanities(swearList, ConvertWordListToSentence(postAllowList));

            // Deduplicate any partial matches, ie, if the word "twatting" is in a sentence, don't include "twat" if part of the same word.
            if (removePartialMatches)
            {
                swearList.RemoveAll(x => swearList.Any(y => x != y && y.Contains(x)));
            }

            return new ReadOnlyCollection<string>(FilterSwearListForCompleteWordsOnly(sentence, swearList).Distinct().ToList());
        }

        /// <summary>
        /// For any given string, censor any profanities from the list using the default
        /// censoring character of an asterix.
        /// </summary>
        /// <param name="sentence">The string to censor.</param>
        /// <returns></returns>
        public string CensorString(string sentence)
        {
            return CensorString(sentence, '*');
        }

        /// <summary>
        /// For any given string, censor any profanities from the list using the specified
        /// censoring character.
        /// </summary>
        /// <param name="sentence">The string to censor.</param>
        /// <param name="censorCharacter">The character to use for censoring.</param>
        /// <returns></returns>
        public string CensorString(string sentence, char censorCharacter)
        {
            return CensorString(sentence, censorCharacter, false);
        }

        /// <summary>
        /// For any given string, censor any profanities from the list using the specified
        /// censoring character.
        /// </summary>
        /// <param name="sentence">The string to censor.</param>
        /// <param name="censorCharacter">The character to use for censoring.</param>
        /// <param name="ignoreNumbers">Ignore any numbers that appear in a word.</param>
        /// <returns></returns>
        public string CensorString(string sentence, char censorCharacter, bool ignoreNumbers)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return string.Empty;
            }

            string noPunctuation = sentence.Trim();
            noPunctuation = noPunctuation.ToLower();

            noPunctuation = Regex.Replace(noPunctuation, @"[^\w\s]", "");

            var words = noPunctuation.Split(' ');

            var postAllowList = FilterWordListByAllowList(words);
            var swearList = new List<string>();

            // Catch whether multi-word profanities are in the allow list filtered sentence.
            AddMultiWordProfanities(swearList, ConvertWordListToSentence(postAllowList));

            StringBuilder censored = new StringBuilder(sentence);
            StringBuilder tracker = new StringBuilder(sentence);

            return CensorStringByProfanityList(censorCharacter, swearList, censored, tracker, ignoreNumbers).ToString();
        }

        /// <summary>
        /// For a given sentence, look for the specified profanity. If it is found, look to see
        /// if it is part of a containing word. If it is, then return the containing word and the start
        /// and end positions of that word in the string.
        ///
        /// For example, if the string contains "scunthorpe" and the passed in profanity is "cunt",
        /// then this method will find "cunt" and work out that it is part of an enclosed word.
        /// </summary>
        /// <param name="toCheck">Sentence to check.</param>
        /// <param name="profanity">Profanity to look for.</param>
        /// <returns>Tuple of the following format (start character, end character, found enclosed word).
        /// If no enclosed word is found then return null.</returns>
        public (int, int, string)? GetCompleteWord(string toCheck, string profanity)
        {
            if (string.IsNullOrEmpty(toCheck))
            {
                return null;
            }

            string profanityLowerCase = profanity.ToLower(CultureInfo.InvariantCulture);
            string toCheckLowerCase = toCheck.ToLower(CultureInfo.InvariantCulture);

            if (toCheckLowerCase.Contains(profanityLowerCase))
            {
                var startIndex = toCheckLowerCase.IndexOf(profanityLowerCase, StringComparison.Ordinal);
                var endIndex = startIndex;

                // Work backwards in string to get to the start of the word.
                while (startIndex > 0)
                {
                    if (toCheck[startIndex - 1] == ' ' || char.IsPunctuation(toCheck[startIndex - 1]))
                    {
                        break;
                    }

                    startIndex -= 1;
                }

                // Work forwards to get to the end of the word.
                while (endIndex < toCheck.Length)
                {
                    if (toCheck[endIndex] == ' ' || char.IsPunctuation(toCheck[endIndex]))
                    {
                        break;
                    }

                    endIndex += 1;
                }

                return (startIndex, endIndex, toCheckLowerCase.Substring(startIndex, endIndex - startIndex).ToLower(CultureInfo.InvariantCulture));
            }

            return null;
        }

        /// <summary>
        /// Check whether a given term matches an entry in the profanity list. ContainsProfanity will first
        /// check if the word exists on the allow list. If it is on the allow list, then false
        /// will be returned.
        /// </summary>
        /// <param name="term">Term to check.</param>
        /// <returns>True if the term contains a profanity, False otherwise.</returns>
        public bool ContainsProfanity(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return false;
            }

            List<string> potentialProfanities = _profanities.Where(word => word.Length <= term.Length).ToList();

            // We might have a very short phrase coming in, resulting in no potential matches even before the regex
            if (potentialProfanities.Count == 0)
            {
                return false;
            }

            Regex regex = new Regex(string.Format(@"(?:{0})", string.Join("|", potentialProfanities).Replace("$", "\\$"), RegexOptions.IgnoreCase));

            foreach (Match profanity in regex.Matches(term))
            {
                // if any matches are found and aren't in the allowed list, we can return true here without checking further
                if (!AllowList.Contains(profanity.Value.ToLower(CultureInfo.InvariantCulture)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// For any given string, censor any profanities from the list using the specified
        /// censoring character.
        /// </summary>
        /// <param name="censorCharacter">The character to use for censoring.</param>
        /// <param name="swearList">A list of profanities</param>
        /// <param name="censored">tracker with profanities censored out using the censorCharacter</param>
        /// <param name="tracker">The string to censor.</param>
        /// <param name="ignoreNumeric">Ignore any numbers that appear in a word.</param>
        /// <returns></returns>
        private StringBuilder CensorStringByProfanityList(char censorCharacter, List<string> swearList, StringBuilder censored, StringBuilder tracker, bool ignoreNumeric)
        {
            foreach (string word in swearList.OrderByDescending(x => x.Length))
            {
                (int, int, string)? result = (0, 0, "");
                var multiWord = word.Split(' ');

                if (multiWord.Length == 1)
                {
                    do
                    {
                        result = GetCompleteWord(tracker.ToString(), word);

                        if (result != null)
                        {
                            string filtered = result.Value.Item3;

                            if (ignoreNumeric)
                            {
                                filtered = Regex.Replace(result.Value.Item3, @"[\d-]", string.Empty);
                            }

                            if (filtered == word)
                            {
                                for (int i = result.Value.Item1; i < result.Value.Item2; i++)
                                {
                                    censored[i] = censorCharacter;
                                    tracker[i] = censorCharacter;
                                }
                            }
                            else
                            {
                                for (int i = result.Value.Item1; i < result.Value.Item2; i++)
                                {
                                    tracker[i] = censorCharacter;
                                }
                            }
                        }
                    }
                    while (result != null);
                }
                else
                {
                    censored = censored.Replace(word, CreateCensoredString(word, censorCharacter));
                }
            }

            return censored;
        }

        /// <summary>
        /// Filter a swear list so that it only contains complete words
        /// </summary>
        /// <param name="sentence">The sentence to check for profanities.</param>
        /// <param name="swearList">List of profanities</param>
        /// <returns>A list of complete words that enclose profanities</returns>
        private List<string> FilterSwearListForCompleteWordsOnly(string sentence, List<string> swearList)
        {
            List<string> filteredSwearList = new List<string>();
            StringBuilder tracker = new StringBuilder(sentence);

            foreach (string word in swearList.OrderByDescending(x => x.Length))
            {
                (int, int, string)? result = (0, 0, "");
                var multiWord = word.Split(' ');

                if (multiWord.Length == 1)
                {
                    do
                    {
                        result = GetCompleteWord(tracker.ToString(), word);

                        if (result != null)
                        {
                            if (result.Value.Item3 == word)
                            {
                                filteredSwearList.Add(word);

                                for (int i = result.Value.Item1; i < result.Value.Item2; i++)
                                {
                                    tracker[i] = '*';
                                }
                                break;
                            }

                            for (int i = result.Value.Item1; i < result.Value.Item2; i++)
                            {
                                tracker[i] = '*';
                            }
                        }
                    }
                    while (result != null);
                }
                else
                {
                    filteredSwearList.Add(word);
                    tracker.Replace(word, " ");
                }
            }

            return filteredSwearList;
        }

        /// <summary>
        /// Filter profanity words out of a string array
        /// </summary>
        /// <param name="words">A string array of words</param>
        /// <returns>A list of words that are not in the Allow List</returns>
        private List<string> FilterWordListByAllowList(string[] words)
        {
            List<string> postAllowList = new List<string>();
            foreach (string word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    if (!AllowList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
                    {
                        postAllowList.Add(word);
                    }
                }
            }

            return postAllowList;
        }

        /// <summary>
        /// Reconstruct sentence excluding allow listed words.
        /// </summary>
        /// <param name="postAllowList">List of words that are not in allow list</param>
        /// <returns>A string excluding allow listed words</returns>
        private static string ConvertWordListToSentence(List<string> postAllowList)
        {
            string postAllowListSentence = string.Empty;

            foreach (string w in postAllowList)
            {
                postAllowListSentence = postAllowListSentence + w + " ";
            }

            return postAllowListSentence;
        }

        /// <summary>
        /// Catch whether multi-word profanities are in the allow list filtered sentence.
        /// </summary>
        /// <param name="swearList">List of profanities</param>
        /// <param name="postAllowListSentence">Sentence without profanities</param>
        private void AddMultiWordProfanities(List<string> swearList, string postAllowListSentence)
        {
            swearList.AddRange(
                from string profanity in _profanities
                where postAllowListSentence.ToLower(CultureInfo.InvariantCulture).Contains(profanity)
                select profanity);
        }

        /// <summary>
        /// Create a string of word length using character for censoring
        /// </summary>
        /// <param name="word">The word to censor over</param>
        /// <param name="censorCharacter">A char used for censoring</param>
        /// <returns>A string of word length using censor character</returns>
        private static string CreateCensoredString(string word, char censorCharacter)
        {
            string censoredWord = string.Empty;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != ' ')
                {
                    censoredWord += censorCharacter;
                }
                else
                {
                    censoredWord += ' ';
                }
            }

            return censoredWord;
        }
    }
}