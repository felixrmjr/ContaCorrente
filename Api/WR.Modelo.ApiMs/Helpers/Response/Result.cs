using System.Collections.Generic;
using WR.Modelo.Util;

namespace WR.Modelo.Api.Helpers.Response
{
    public class Result
    {
        public static Result Empty = new Result();

        private readonly List<Error> _errors = new List<Error>();

        public bool HasErrors => _errors.Count > 0;

        public List<Error> Errors => _errors;

        protected Result() { }

        public Result(Error error) => this.AddError(error);

        public Result(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
                this.AddError(error);
        }

        public void AddError(Error error)
        {
            Throw.IfIsNull(error);
            this.Errors.Add(error);
        }
    }

    public class Result<TData>
    {
        public TData Data { get; }
        public long Total { get; }

        public Result(TData data) => this.Data = data;

        public Result(TData data, long total)
        {
            this.Data = data;
            this.Total = total;
        }
    }
}
