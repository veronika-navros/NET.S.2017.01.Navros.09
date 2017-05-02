using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    /// <summary>
    /// Stores book list
    /// </summary>
    public class BookListStorage : IStorage
    {
        private readonly string _fileName;
        private volatile Logger _logger;

        /// <summary>
        /// Initializes new book storage associated with the specified file name and logger instance
        /// </summary>
        /// <param name="fileName">Name of file where books are stored</param>
        /// <param name="logger">Logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public BookListStorage(string fileName, Logger logger)
        {
            if (fileName == null)
                throw new ArgumentNullException();

            if (fileName.Equals(string.Empty))
                throw  new ArgumentException();

            _fileName = fileName;
            if (!File.Exists(_fileName))
                File.Create(_fileName);
            _logger = logger;
            _logger.WriteLog("BookListStorage is created");
        }

        /// <summary>
        /// Uploads book collection into file
        /// </summary>
        /// <param name="books">Book cllection</param>
        public void Upload(IEnumerable<Book> books)
        {
            try
            {
                using (var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Write))
                {
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        _logger.WriteLog("File " + _fileName + " is opened for writing");
                        foreach (var book in books)
                        {
                            writer.Write(book.Name);
                            writer.Write(book.Author);
                            writer.Write(book.Edition);
                            writer.Write(book.Genre);
                            writer.Write(book.Publisher);
                        }
                        _logger.WriteLog("Writing into file completed");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.WriteLog("Error while writing into file");
                throw new Exception("Error while writing into file");
            }
        }

        /// <summary>
        /// Downloads book collection from file
        /// </summary>
        /// <returns>Book collection</returns>
        public IEnumerable<Book> Download()
        {
            var books = new List<Book>();

            try
            {
                using (var fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(fileStream))
                    {
                        _logger.WriteLog("File " + _fileName + " is opened for reading");
                        while (reader.PeekChar() > -1)
                        {
                            books.Add(new Book(reader.ReadString(), reader.ReadString(), reader.ReadInt32(),
                                reader.ReadString(), reader.ReadString()));
                        }
                        _logger.WriteLog("Reading from file completed");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.WriteLog("Error while reading from file");
                throw new Exception("Error while reading from file");
            }

            return books;
        }
    }
}
