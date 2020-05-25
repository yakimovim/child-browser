using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace ChildBrowser
{
    class Bookmark
    {
        public string Title { get; set; }
        public string Address { get; set; }
    }

    class BookmarksStorage
    {
        private const string Path = "bookmarks.txt";

        public BookmarksStorage()
        {
            if (!File.Exists(Path))
                File.CreateText(Path);

            var content = File.ReadAllText(Path);

            var bookmarks = JsonConvert.DeserializeObject<Bookmark[]>(content);

            foreach (var bookmark in bookmarks)
            {
                Bookmarks.Add(bookmark);
            }
        }

        public ObservableCollection<Bookmark> Bookmarks { get; } = new ObservableCollection<Bookmark>();

        public void Store(Bookmark bookmark)
        {
            if (bookmark is null)
            {
                throw new ArgumentNullException(nameof(bookmark));
            }

            Bookmarks.Add(bookmark);

            var content = JsonConvert.SerializeObject(Bookmarks.ToArray());

            File.WriteAllText(Path, content);
        }
    }
}
