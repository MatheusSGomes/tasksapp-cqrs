namespace Application.Utils;

// Classe Genérica
public record PaginatedList<T>
{
    public int TotalItems { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int StartIndex { get; private set; }
    public int EndIndex { get; private set; }
    public List<T>? Items { get; private set; }

    public PaginatedList(IEnumerable<T> items, int page, int pageSize)
    {
        TotalItems = items.Count();
        CurrentPage = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(TotalItems / (double) PageSize);
        CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;
        CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
        StartIndex = (CurrentPage - 1) * PageSize;
        EndIndex = Math.Min(StartIndex + PageSize - 1, TotalItems - 1);
        Items = items.Skip(StartIndex).Take(PageSize).ToList();
    }
}
