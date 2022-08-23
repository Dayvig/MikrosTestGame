using MikrosClient.ProfanityFilter.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MikrosClient.ProfanityFilter
{
    internal sealed class AllowList : IAllowList
    {
        private List<string> _allowList;

        internal AllowList()
        {
            _allowList = new List<string>();
        }

        /// <summary>
        /// Return an instance of a read only collection containing allow list
        /// </summary>
        public ReadOnlyCollection<string> ToList
        {
            get
            {
                return new ReadOnlyCollection<string>(_allowList);
            }
        }

        /// <summary>
        /// Add a word to the profanity allow list. This means a word that is in the allow list
        /// can be ignored. All words are treated as case insensitive.
        /// </summary>
        /// <param name="wordToAllowlist">The word that you want to allow list.</param>
        public void Add(string wordToAllowlist)
        {
            if (string.IsNullOrEmpty(wordToAllowlist))
            {
                throw new ArgumentNullException(nameof(wordToAllowlist));
            }

            if (!_allowList.Contains(wordToAllowlist.ToLower(CultureInfo.InvariantCulture)))
            {
                _allowList.Add(wordToAllowlist.ToLower(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// True if provided word is present in allow list, else false.
        /// </summary>
        /// <param name="wordToCheck">Word to check.</param>
        /// <returns></returns>
        public bool Contains(string wordToCheck)
        {
            if (string.IsNullOrEmpty(wordToCheck))
            {
                throw new ArgumentNullException(nameof(wordToCheck));
            }

            return _allowList.Contains(wordToCheck.ToLower(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Return the number of items in the allow list.
        /// </summary>
        /// <returns>The number of items in the allow list.</returns>
        public int Count
        {
            get
            {
                return _allowList.Count;
            }
        }

        /// <summary>
        /// Remove all words from the allow list.
        /// </summary>
        public void Clear()
        {
            _allowList.Clear();
        }

        /// <summary>
        /// Remove a word from the profanity allow list. All words are treated as case insensitive.
        /// </summary>
        /// <param name="wordToRemove">The word that you want to use</param>
        /// <returns>True if the word is successfuly removes, False otherwise.</returns>
        public bool Remove(string wordToRemove)
        {
            if (string.IsNullOrEmpty(wordToRemove))
            {
                throw new ArgumentNullException(nameof(wordToRemove));
            }

            return _allowList.Remove(wordToRemove.ToLower(CultureInfo.InvariantCulture));
        }
    }
}