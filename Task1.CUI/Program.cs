namespace Task1.CUI
{
    class Program
    {
        static void Main()
        {
            var book = new Book("Name", "Author", 2, "Genre", "Publisher");
            var logger = new Logger("logger.txt");
            var storage = new BookListStorage("storage", logger);
            var service = new BookListService(logger);

            service.AddBook(book);
            service.UploadBookList(storage);
            service.RemoveBook(book);
            var books = service.DownloadBookList(storage);
        }
    }
}
