using System.Collections.Generic;

namespace WR.Modelo.JsonLocalizer.Format
{
    internal class JsonLocalizationFormat
    {
        public string Key { get; set; }
        public Dictionary<string, string> Values = new Dictionary<string, string>();
    }
}
