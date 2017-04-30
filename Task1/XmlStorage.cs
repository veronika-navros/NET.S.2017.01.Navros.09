using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Task1
{
    /// <summary>
    /// Stores book list in XML format
    /// </summary>
    public class XmlStorage : IStorage
    {
        private readonly string _fileName;
        private volatile Logger _logger;

        /// <summary>
        /// Initializes new XML book storage associated with the specified file name and logger instance
        /// </summary>
        /// <param name="fileName">Name of file where books are stored</param>
        /// <param name="logger">Logger instance</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public XmlStorage(string fileName, Logger logger)
        {
            if (fileName == null)
                throw new ArgumentNullException();

            if (fileName.Equals(string.Empty))
                throw new ArgumentException();

            _fileName = fileName;
            if (!File.Exists(_fileName))
                File.Create(_fileName);
            _logger = logger;
            _logger.WriteLog("XmlStorage is created");
        }

        #region Public Methods

        /// <summary>
        /// Uploads book collection into XML file
        /// </summary>
        /// <param name="books">Book cllection</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void Upload(IEnumerable<Book> books)
        {
            if (ReferenceEquals(books, null))
                throw new ArgumentNullException();

            if (!_fileName.EndsWith(".xml"))
                throw new ArgumentException();

            XmlTextWriter docXmlW = new XmlTextWriter(_fileName, Encoding.UTF8);
            docXmlW.WriteStartDocument();
            docXmlW.WriteStartElement("Books");
            docXmlW.WriteEndElement();
            docXmlW.Close();
            XmlDocument doc = new XmlDocument();
            doc.Load(_fileName);

            foreach (var book in books)
            {
                XmlElement element = doc.CreateElement("Book");
                doc.DocumentElement.AppendChild(element);
                XmlElement name = doc.CreateElement("Name");
                XmlElement author = doc.CreateElement("Author");
                XmlElement edition = doc.CreateElement("Edition");
                XmlElement genre = doc.CreateElement("Genre");
                XmlElement publisher = doc.CreateElement("Publisher");

                name.InnerText = book.Name;
                author.InnerText = book.Author;
                edition.InnerText = book.Edition.ToString();
                genre.InnerText = book.Genre;
                publisher.InnerText = book.Publisher;

                element.AppendChild(name);
                element.AppendChild(author);
                element.AppendChild(edition);
                element.AppendChild(genre);
                element.AppendChild(publisher);
            }

            doc.Save(_fileName);
            _logger.WriteLog("Saved to XML file");
        }

        /// <summary>
        /// Downloads book collection from XML file
        /// </summary>
        /// <returns>Book collection</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<Book> Download()
        {
            if (string.IsNullOrEmpty(_fileName))
                throw new ArgumentNullException();

            if (!_fileName.EndsWith(".xml"))
                throw new ArgumentException();

            List<Book> books = new List<Book>();
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.Load(_fileName);
            }
            catch (XmlException ex)
            {
                _logger.WriteLog("Error while loading file");
                throw new XmlException(ex.Message);
            }

            if (!ReferenceEquals(doc.DocumentElement, null))
            {
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                foreach (XmlElement book in nodes)
                {
                    books.Add(new Book(
                        book.ChildNodes[0].InnerText,
                        book.ChildNodes[1].InnerText,
                        int.Parse(book.ChildNodes[2].InnerText),
                        book.ChildNodes[3].InnerText,
                        book.ChildNodes[4].InnerText));
                }
            }

            _logger.WriteLog("Loaded from XML file");
            return books;
        }

        #endregion
    }
}
