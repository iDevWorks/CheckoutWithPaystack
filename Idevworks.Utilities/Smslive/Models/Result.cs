using System.Text.Json.Serialization;

namespace iDevWorks.BulkSMS.Contracts
{
    public class Result<T>
    {
        public Result(T data, Pager? paging)
        {
            Data = data;
            Paging = paging;
        }

        public T Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Pager? Paging { get; }
    }

    public class Pager
    {
        public Pager(int pageNo, int pageSize, int totalCount, int totalPages)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        public int PageNo { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
    }

}
