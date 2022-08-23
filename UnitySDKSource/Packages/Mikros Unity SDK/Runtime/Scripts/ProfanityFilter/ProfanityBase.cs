using System;
using System.Collections.Generic;
using System.Globalization;

namespace MikrosClient.ProfanityFilter
{
    internal partial class ProfanityBase
    {
        protected List<string> _profanities;

        /// <summary>
        /// Constructor that initializes the standard profanity list.
        /// </summary>
        public ProfanityBase()
        {
            _profanities = new List<string>(_wordList);
        }

        /// <summary>
        /// Constructor that allows you to insert a custom array or profanities.
        /// This list will replace the default list.
        /// </summary>
        /// <param name="profanityList">Array of words considered profanities.</param>
        protected ProfanityBase(string[] profanityList)
        {
            if (profanityList == null)
            {
                throw new ArgumentNullException(nameof(profanityList));
            }

            _profanities = new List<string>(profanityList);
        }

        /// <summary>
        /// Constructor that allows you to insert a custom list or profanities.
        /// This list will replace the default list.
        /// </summary>
        /// <param name="profanityList">List of words considered profanities.</param>
        protected ProfanityBase(List<string> profanityList)
        {
            if (profanityList == null)
            {
                throw new ArgumentNullException(nameof(profanityList));
            }

            _profanities = profanityList;
        }

        /// <summary>
        /// Add a custom profanity to the list.
        /// </summary>
        /// <param name="profanity">The profanity to add.</param>
        public void AddProfanity(string profanity)
        {
            if (string.IsNullOrEmpty(profanity))
            {
                throw new ArgumentNullException(nameof(profanity));
            }

            _profanities.Add(profanity);
        }

        /// <summary>
        /// Add a custom array profanities to the default list. This adds to the
        /// default list, and does not replace it.
        /// </summary>
        /// <param name="profanityList">The array of profanities to add.</param>
        public void AddProfanity(string[] profanityList)
        {
            if (profanityList == null)
            {
                throw new ArgumentNullException(nameof(profanityList));
            }

            _profanities.AddRange(profanityList);
        }

        /// <summary>
        /// Add a custom list profanities to the default list. This adds to the
        /// default list, and does not replace it.
        /// </summary>
        /// <param name="profanityList">The list of profanities to add.</param>
        public void AddProfanity(List<string> profanityList)
        {
            if (profanityList == null)
            {
                throw new ArgumentNullException(nameof(profanityList));
            }

            _profanities.AddRange(profanityList);
        }

        /// <summary>
        /// Remove a profanity from the current loaded list of profanities.
        /// </summary>
        /// <param name="profanity">The profanity to remove from the list.</param>
        /// <returns>True if the profanity was removed. False otherwise.</returns>
        public bool RemoveProfanity(string profanity)
        {
            if (string.IsNullOrEmpty(profanity))
            {
                throw new ArgumentNullException(nameof(profanity));
            }

            return _profanities.Remove(profanity.ToLower(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Remove a list of profanities from the current loaded list of profanities.
        /// </summary>
        /// <param name="profanities">The list of profanities to remove from the list.</param>
        /// <returns>True if the profanities were removed. False otherwise.</returns>
        public bool RemoveProfanity(List<string> profanities)
        {
            if (profanities == null)
            {
                throw new ArgumentNullException(nameof(profanities));
            }

            foreach (string naughtyWord in profanities)
            {
                if (!RemoveProfanity(naughtyWord))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove an array of profanities from the current loaded list of profanities.
        /// </summary>
        /// <param name="profanities">The array of profanities to remove from the list.</param>
        /// <returns>True if the profanities were removed. False otherwise.</returns>
        public bool RemoveProfanity(string[] profanities)
        {
            if (profanities == null)
            {
                throw new ArgumentNullException(nameof(profanities));
            }

            foreach (string naughtyWord in profanities)
            {
                if (!RemoveProfanity(naughtyWord))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Remove all profanities from the current loaded list.
        /// </summary>
        public void Clear()
        {
            _profanities.Clear();
        }

        /// <summary>
        /// Return the number of profanities in the system.
        /// </summary>
        public int Count
        {
            get
            {
                return _profanities.Count;
            }
        }
    }
}