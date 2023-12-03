namespace Optimization4;
public class BookManager
{
    private List<Book> books;

    public BookManager(List<Book> books)
    {
        this.books = books;
    }

    public bool CheckIfBookExists(string title)
    {
        return books.Any(b => b.Title == title);
    }

    public IEnumerable<Book> GetBooksPublishedAfter(int year)
    {
        return books.Where(b => b.Year > year).ToList();
    }

    public Book FindBookByTitle(string title)
    {
        return books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }


    public IEnumerable<Book> GetBooksByAuthor(string author)
    {
        return books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
    }

    public bool AreThereAnyBooksByAuthor(string author)
    {
        return books.Count(b => b.Author == author) > 0;
    }

    public Book GetSingleBookByTitle(string title)
    {
        return books.SingleOrDefault(b => b.Title.Contains(title));
    }

    public Book GetFirstPublishedBook()
    {
        return books.OrderBy(b => b.Year).FirstOrDefault();
    }
}
