using WR.Modelo.Util;

namespace WR.Modelo.Api.Helpers.Response
{
    public class Error
    {
        public string Reference { get; private set; }

        public string Message { get; private set; }

        public Error(string message) => this.SetMessage(message);

        public Error(string reference, string message)
        {
            this.SetReference(reference);
            this.SetMessage(message);
        }

        private void SetReference(string reference)
        {
            Throw.IfIsNullOrWhiteSpace(reference);
            this.Reference = reference;
        }

        private void SetMessage(string message)
        {
            Throw.IfIsNullOrWhiteSpace(message);
            this.Message = message;
        }

        public override string ToString() => this.Message;
    }
}
