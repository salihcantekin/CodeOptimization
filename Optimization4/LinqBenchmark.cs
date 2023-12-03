using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using Bogus;

namespace Optimization4;

[MemoryDiagnoser]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
[HideColumns(Column.Error, Column.Median, Column.StdDev, Column.StdErr, Column.Gen0, Column.Gen1, Column.Gen2)]
public class LinqBenchmark
{
    private BookManager bookManager;
    private BookManagerOptimized optimizedbookManager;
    private List<Book> books;
    private readonly int fakerSeed = 1024;
    private Book dummyBook;

    [GlobalSetup]
    public void SetUp()
    {
        books = new Faker<Book>()
            .UseSeed(fakerSeed)
            .RuleFor(i => i.Title, i => i.Lorem.Sentence(5))
            .RuleFor(i => i.Author, i => i.Person.FullName)
            .RuleFor(i => i.Year, i => i.Random.Int())
            .Generate(1_000);

        dummyBook = books.First();

        bookManager = new BookManager(books);
        optimizedbookManager = new BookManagerOptimized(books);
    }

    [Benchmark]
    public void CheckIfBookExists()
    {
        _ = bookManager.CheckIfBookExists(dummyBook.Title);
    }

    [Benchmark]
    public void CheckIfBookExists_Opt()
    {
        _ = optimizedbookManager.CheckIfBookExists(dummyBook.Title);
    }

    [Benchmark]
    public void GetBooksPublishedAfter()
    {
        _ = bookManager.GetBooksPublishedAfter(dummyBook.Year);
    }

    [Benchmark]
    public void GetBooksPublishedAfter_Opt()
    {
        _ = optimizedbookManager.GetBooksPublishedAfter(dummyBook.Year);
    }

    [Benchmark]
    public void FindBookByTitle()
    {
        _ = bookManager.FindBookByTitle(dummyBook.Title);
    }
    [Benchmark]
    public void FindBookByTitle_Opt()
    {
        _ = optimizedbookManager.FindBookByTitle(dummyBook.Title);
    }

    [Benchmark]
    public void GetBooksByAuthor()
    {
        _ = bookManager.GetBooksByAuthor(dummyBook.Author);
    }

    [Benchmark]
    public void GetBooksByAuthor_Opt()
    {
        _ = optimizedbookManager.GetBooksByAuthor(dummyBook.Author);
    }

    [Benchmark]
    public void AreThereAnyBooksByAuthor()
    {
        _ = bookManager.AreThereAnyBooksByAuthor(dummyBook.Author);
    }

    [Benchmark]
    public void AreThereAnyBooksByAuthor_Opt()
    {
        _ = optimizedbookManager.AreThereAnyBooksByAuthor(dummyBook.Author);
    }

    [Benchmark]
    public void GetSingleBookByTitle()
    {
        _ = bookManager.GetSingleBookByTitle(dummyBook.Title);
    }

    [Benchmark]
    public void GetSingleBookByTitle_Opt()
    {
        _ = optimizedbookManager.GetSingleBookByTitle(dummyBook.Title);
    }

    [Benchmark]
    public void GetFirstPublishedBook()
    {
        _ = bookManager.GetFirstPublishedBook();
    }

    [Benchmark]
    public void GetFirstPublishedBook_Opt()
    {
        _ = optimizedbookManager.GetFirstPublishedBook();
    }
}
