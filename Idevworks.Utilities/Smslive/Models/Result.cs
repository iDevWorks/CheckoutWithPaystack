using System.Text.Json.Serialization;

namespace iDevWorks.BulkSMS
{
    public class Result<T>(T data, Pager? paging)
    {
        public T Data { get; } = data;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Pager? Paging { get; } = paging;
    }

    public class Pager(int pageNo, int pageSize, int totalCount, int totalPages)
    {
        public int PageNo { get; } = pageNo;
        public int PageSize { get; } = pageSize;
        public int TotalCount { get; } = totalCount;
        public int TotalPages { get; } = totalPages;
    }

}
