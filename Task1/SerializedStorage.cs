using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Task1
{
    /// <summary>
    /// Stores serialized book list 
    /// </summary>
    public class SerializedStorage : IStorage
    {
        private readonly string _fileName;
        private volatile Logger _logger;

        /// <summary>
        /// Initializes new serialized book storage associated with the specified file name and logger instance
        /// </summary>
        /// <param name="fileName">Name of file where books are stored</param>
        /// <param name="logger">Logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public SerializedStorage(string fileName, Logger logger)
        {
            if (fileName == null)
                throw new ArgumentNullException();

            if (fileName.Equals(string.Empty))
                throw new ArgumentException();

            _fileName = fileName;
            if (!File.Exists(_fileName))
                File.Create(_fileName);
            _logger = logger;
            _logger.WriteLog("SerializedStorage is created");
        }

        #region Public methods

        /// <summary>
        /// Uploads book collection into file
        /// </summary>
        /// <param name="books">Book cllection</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Upload(IEnumerable<Book> books)
        {
            if (ReferenceEquals(books, null))
                throw new ArgumentNullException();

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (Stream stream = File.Create(_fileName))
                {
                    formatter.Serialize(stream, books);
                    _logger.WriteLog("Book list serialized");
                }
            }
            catch
            {
                _logger.WriteLog("Error while serializing");
                throw new Exception("Serialization Error");
            }
        }

        /// <summary>
        /// Downloads book collection from file
        /// </summary>
        /// <returns>Book collection</returns>
        public IEnumerable<Book> Download()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (Stream stream = File.OpenRead(_fileName))
                {
                    _logger.WriteLog("Book list deserialized");
                    return (List<Book>)formatter.Deserialize(stream);
                }
            }
            catch
            {
                _logger.WriteLog("Error while deserializing");
                throw new Exception("Deserialization Error");
            }
        }

        #endregion
    }
}
