using System;

namespace Task1
{
    /// <summary>
    /// REpresents class which contains information about a book
    /// </summary>
    [Serializable]
    public class Book : IEquatable<Book>, IComparable, IComparable<Book>
    {
        #region Properties

        public string Name { get; set; }
        public string Author { get; set; }
        public int Edition { get; set; }
        public string Genre { get; set; }
        public string Publisher { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new Book instance with fields' default values
        /// </summary>
        public Book() : this(string.Empty, string.Empty, 0, string.Empty, string.Empty) { }

        /// <summary>
        /// Initializes new Book instance with specified values
        /// </summary>
        /// <param name="name">Book name</param>
        /// <param name="author">Book author</param>
        /// <param name="edition">Book edition</param>
        /// <param name="genre">Book genre</param>
        /// <param name="publisher">Book publisher</param>
        public Book(string name, string author, int edition,
            string genre, string publisher)
        {
            if(ReferenceEquals(name, null) || ReferenceEquals(author, null) || 
                ReferenceEquals(genre, null) || ReferenceEquals(publisher, null))
                throw new ArgumentNullException();

            Name = name;
            Author = author;
            Edition = edition;
            Genre = genre;
            Publisher = publisher;
        }

        /// <summary>
        /// Initializes new Book instance with values from existing Book instance
        /// </summary>
        /// <param name="book">Book instance with source fields' values</param>
        internal Book(Book book)
        {
            Name = book.Name;
            Author = book.Author;
            Edition = book.Edition;
            Genre = book.Genre;
            Publisher = book.Publisher;
        }

        #endregion

        #region Public members

        /// <summary>
        /// Determines whether the specified book instance is equal to the current book instance.
        /// </summary>
        /// <param name="book">The book to compare with the current book. </param>
        /// <returns>true if the specified book is equal to the current book; otherwise, false.</returns>
        public bool Equals(Book book)
        {
            if (ReferenceEquals(book, null))
                return false;

            if (ReferenceEquals(book, this))
                return true;

            return Name.Equals(book.Name) && Author.Equals(book.Author) && Edition == book.Edition
                   && Genre.Equals(book.Genre) && Publisher.Equals(book.Publisher);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current book instance
        /// </summary>
        /// <param name="o">The object to compare with the current book. </param>
        /// <returns>true if the specified object is equal to the current book; otherwise, false.</returns>
        public override bool Equals(object o)
        {
            if (ReferenceEquals(o, null))
                return false;

            if (o.GetType() != GetType())
                return false;

            return Equals((Book)o);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns> A string that represents the current object</returns>
        public override string ToString() => Name + " " + Author + " Edition: " + Edition
            + " " + Genre + " Publisher: " + Publisher;

        public int CompareTo(object o)
        {
            if (ReferenceEquals(o, null))
                return 1;

            if (!ReferenceEquals(o as Book, null))
                return CompareTo(o as Book);

            return 0;
        }

        public int CompareTo(Book book)
        {
            if (ReferenceEquals(book, null))
                return 1;

            return string.Compare(Name, book.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}
