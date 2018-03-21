namespace MatrixClient
{
    using System.Security;

    public static class StringExtensions
    {
        public static SecureString ToSecureString(this string unsecuredString)
        {
            var securedString = new SecureString();
            foreach (var c in unsecuredString)
            {
                securedString.AppendChar(c);
            }

            securedString.MakeReadOnly();

            return securedString;
        }

        public static bool IsNumeric(this string s)
        {
            int output;
            return int.TryParse(s, out output);
        }
    }
}
