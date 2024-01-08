using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Idevworks.Utilities.SMS.Dtos
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Pager? Paging { get; set; }
    }
}
