using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChildBrowser.Bookmarks
{
    class BookmarksStorage
    {
        private const string Path = "bookmarks.txt";

        public BookmarksStorage()
        {
            if (!File.Exists(Path)) return;

            var content = File.ReadAllText(Path);

            var bookmarks = JsonConvert.DeserializeObject<Bookmark[]>(content)
                ?? new Bookmark[0];

            foreach (var bookmark in bookmarks)
            {
                Bookmarks.Add(bookmark);
            }
        }

        public ICollection<Bookmark> Bookmarks { get; } = new List<Bookmark>();

        public void Add(Bookmark bookmark)
        {
            if (bookmark is null)
            {
                throw new ArgumentNullException(nameof(bookmark));
            }

            Bookmarks.Add(bookmark);

            Save();
        }

        public void Remove(Bookmark bookmark)
        {
            if (bookmark is null)
            {
                throw new ArgumentNullException(nameof(bookmark));
            }

            Bookmarks.Remove(bookmark);

            Save();
        }

        public void Save()
        {
            var content = JsonConvert.SerializeObject(Bookmarks.ToArray());

            File.WriteAllText(Path, content);
        }
    }
}
