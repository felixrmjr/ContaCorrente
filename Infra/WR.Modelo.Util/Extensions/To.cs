using System;

namespace WR.Modelo.Util.Extensions
{
    public static class To
    {
        public static TEnum ToEnum<TEnum>(this object enumObj) where TEnum : struct
        {
            TEnum value;
            Enum.TryParse(enumObj?.ToString(), true, out value);
            return value;
        }
    }
}
