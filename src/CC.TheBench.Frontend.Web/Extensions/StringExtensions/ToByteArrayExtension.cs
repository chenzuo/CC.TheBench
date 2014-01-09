namespace CC.TheBench.Frontend.Web.Extensions.StringExtensions
{
    using System;
    using System.Globalization;

    public static class ToByteArrayExtension
    {
        public static byte[] ToByteArray(this string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if ((value.Length & 1) != 0)
                throw new ArgumentOutOfRangeException("value",
                                                      value,
                                                      "value must contain an even number of characters.");

            var result = new byte[value.Length / 2];

            for (var i = 0; i < value.Length; i += 2)
                result[i / 2] = byte.Parse(value.Substring(i, 2), NumberStyles.HexNumber);

            return result;
        }
    }
}