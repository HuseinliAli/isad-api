namespace Persistence.Base.Models;

public class OffSetPagedList<T>
{
    public List<T> Items { get; set; }
    public MetaData MetaData { get; set; }

    public OffSetPagedList(List<T> data, int count, int pageSize, int pageNumber)
    {
        MetaData = new()
        {
            TotalRecords = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        Items = data;
    }
}