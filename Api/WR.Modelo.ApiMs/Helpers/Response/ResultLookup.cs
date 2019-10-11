namespace WR.Modelo.Api.Helpers.Response
{
    public class ResultLookup<TIdentyity>
    {
        public ResultLookup() { }

        public ResultLookup(TIdentyity id, object text)
        {
            this.Id = id;
            this.Label = text;
        }

        public TIdentyity Id { get; set; }

        public string Value => this.Id.ToString();

        public object Label { get; set; }
    }
}
