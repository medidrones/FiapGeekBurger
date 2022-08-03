using System;

namespace GeekBurger.StoreCatalog.Contract
{
    public class OperationResult<T>
    {
        public DateTime Date { get { return DateTime.Now; } }
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
