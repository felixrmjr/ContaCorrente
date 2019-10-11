using System.Collections.Generic;

namespace WR.Modelo.Domain.Interfaces.Base
{
    public interface IPagedList<T>
    {
        int Total { get; }
        IEnumerable<T> Data { get; }
    }
}
