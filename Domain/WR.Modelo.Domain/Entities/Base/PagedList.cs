﻿using System.Collections.Generic;
using System.Linq;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Entities.Base
{
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList(IEnumerable<T> items) : this(items, items.Count()) { }

        public PagedList(IEnumerable<T> items, int total)
        {
            this.Total = total;
            this.Data = items;
        }

        public int Total { get; }

        public IEnumerable<T> Data { get; }
    }
}
