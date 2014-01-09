namespace CC.TheBench.Frontend.Web.Extensions.ByteExtensions
{
    public static class ToHexExtension
    {
        public static string ToHex(this byte[] value)
        {
            var c = new char[value.Length * 2];
            
            // ReSharper disable once TooWideLocalVariableScope
            byte b;

            for (var i = 0; i < value.Length; i++)
            {
                b = ((byte)(value[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(value[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }

            return new string(c, 0, c.Length);
        }
    }
}