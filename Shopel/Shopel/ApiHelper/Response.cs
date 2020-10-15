using System;
using System.Collections.Generic;
using System.Text;

namespace Shopel.ApiHelper
{
    public class Response<T> where T : class
    {
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
        public List<T> Data { get; set; }
    }
}
