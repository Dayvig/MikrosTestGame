using System.Collections.ObjectModel;

namespace MikrosClient.ProfanityFilter.Interfaces
{
    internal interface IAllowList
    {
        void Add(string wordToAllowlist);

        bool Contains(string wordToCheck);

        bool Remove(string wordToRemove);

        void Clear();

        int Count { get; }
        ReadOnlyCollection<string> ToList { get; }
    }
}