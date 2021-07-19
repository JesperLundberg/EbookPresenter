using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EBookPresenter.Factories;
using EBookPresenter.Models;
using EBookPresenter.Wrappers;

[assembly: InternalsVisibleTo("EBookPresenter.Tests")]

namespace EBookPresenter.Repositories
{
    public class EBookRepository : IEBookRepository
    {
        private IFileSystem FileSystem { get; }
        private IFileInfoFactory FileInfoFactory { get; }

        public EBookRepository(IFileSystem fileSystem, IFileInfoFactory fileInfoFactory)
        {
            FileSystem = fileSystem;
            FileInfoFactory = fileInfoFactory;
        }

        public IEnumerable<EBook> GetAllEbooks(string folderToRead, PaginationFilter paginationFilter,
            out int totalItems)
        {
            var allFiles = GetEbooksRecursive(folderToRead);

            var ebooks = new List<EBook>();

            foreach (var file in allFiles)
            {
                // Change from back slash to front slash as Linux uses the latter and Windows is agnostic
                var fixedString = string.IsNullOrEmpty(file) ? "" : file.Replace('\\', '/');

                var fileInfo = FileInfoFactory.Create(fixedString);

                ebooks.Add(new EBook
                    {Title = Path.GetFileName(file), Path = fixedString, CreatedDate = fileInfo.CreationTime});
            }

            var orderedBooks = OrderBooks(ebooks, paginationFilter);

            totalItems = orderedBooks.Count();

            return orderedBooks.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                .Take(paginationFilter.PageSize);
        }

        internal IEnumerable<EBook> OrderBooks(IEnumerable<EBook> ebooks, PaginationFilter paginationFilter)
        {
            return paginationFilter.SortOrder switch
            {
                "creation" => ebooks.OrderByDescending(x => x.CreatedDate),
                "alphabetic" => ebooks.OrderBy(x => x.Title),
                _ => ebooks.OrderBy(x => x.Title)
            };
        }

        internal IEnumerable<string> GetEbooksRecursive(string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return new List<string>();
            }

            var directories = FileSystem.GetDirectories(folder);

            var files = FileSystem.GetFiles(folder).Where(x => x.EndsWith(".epub")).ToList();

            foreach (var directory in directories)
            {
                files.AddRange(GetEbooksRecursive(directory));
            }

            return files;
        }
    }
}