using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Service for working with book collection
    /// </summary>
    public class BookListService
    {   
        private List<Book> _books;
        private volatile Logger _logger;

        #region Constructors

        /// <summary>
        /// Initializes new instance of the BookListService Class with the specified logger
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        public BookListService(Logger logger)
        {
            if (logger == null)
                throw new ArgumentNullException();

            _books = new List<Book>();
            _logger = logger;
            _logger.WriteLog("BookListService is created");
        }

        #endregion

        #region Public members

        /// <summary>
        /// Adds book if it doesn't exist to the book list
        /// </summary>
        /// <param name="book">Book to add</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException();

            if (_books.Contains(book))
                throw new Exception("Error: book already exists");

            _books.Add(book);
            _logger.WriteLog("Book is added to the storage");
        }

        /// <summary>
        /// Removes book if it exists from the book list
        /// </summary>
        /// <param name="book">Book to remove</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public void RemoveBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException();

            if (!_books.Contains(book))
                throw new Exception("Error: book doesn't exists");

            _books.Remove(book);
            _logger.WriteLog("Book is removed from the storage");
        }

        /// <summary>
        /// Finds book by specified criteria
        /// </summary>
        /// <param name="predicate">Criteria for book search</param>
        /// <returns>Book instance or null if book is not found</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Book FindBookByTag(Predicate<Book> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException();

            _logger.WriteLog("Searching for book");
            return new Book(_books.Find(predicate));
        }

        /// <summary>
        /// Sorts book list by specified criteria using IComparer instance
        /// </summary>
        /// <param name="comparer">Comparer instance for books sorting</param>
        public void SortBookListByTag(IComparer<Book> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException();

            _logger.WriteLog("Sorting book collection");
            _books.Sort(comparer);
        }

        /// <summary>
        /// Sorts book list by specified criteria using Comparison instance
        /// </summary>
        /// <param name="comparison">Comparison instance for books sorting</param>
        public void SortBookListByTag(Comparison<Book> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException();

            _logger.WriteLog("Sorting book collection");
            _books.Sort(Comparer<Book>.Create(comparison));
        }

        /// <summary>
        /// Uploads book collection into the storage
        /// </summary>
        public void UploadBookList(IStorage storage)
        {
            try
            {
                _logger.WriteLog("Starting uploading book collection to the storage");
                storage.Upload(_books);
                _logger.WriteLog("Book collection is uploaded to the storage");
            }
            catch (Exception e)
            {
                _logger.WriteLog("Error while uploading book collection");
                throw new Exception("Error while uploading book collection");
            }
        }

        /// <summary>
        /// Downloads book collection from the storage
        /// </summary>
        /// <returns>Book collection</returns>
        public IEnumerable<Book> DownloadBookList(IStorage storage)
        {
            try
            {
                _logger.WriteLog("Starting downloading book collection from the storage");
                _books = storage.Download().ToList();
                _logger.WriteLog("Book collection is downloaded from the storage");
            }
            catch (Exception e)
            {
                _logger.WriteLog("Error while downloading book collection");
                throw new Exception(@"Error while downloading book collection");
            }

            var books = new Book[_books.Count];
            _books.CopyTo(books);

            return books;
        }

        #endregion
    }
}
