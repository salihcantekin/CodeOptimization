namespace Optimization4;
public class BookManagerOptimized
{
    private List<Book> books;

    public BookManagerOptimized(List<Book> books)
    {
        this.books = books;
    }

    public bool CheckIfBookExists(string title)
    {
        return books.Any(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<Book> GetBooksPublishedAfter(int year)
    {
        return books.Where(b => b.Year > year);
    }

    public Book FindBookByTitle(string title)
    {
        return books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }


    public IEnumerable<Book> GetBooksByAuthor(string author)
    {
        return books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }

    public bool AreThereAnyBooksByAuthor(string author)
    {
        return books.Exists(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
        //return books.Any(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase));
    }

    public Book GetSingleBookByTitle(string title)
    {
        return books.SingleOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    public Book GetFirstPublishedBook()
    {
        return books.MinBy(b => b.Year);
    }
}
