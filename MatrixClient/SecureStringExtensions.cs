namespace MatrixClient
{
    using System;
    using System.Runtime.InteropServices;

    public static class SecureStringExtensions
    {
        public static string ToUnsecureString(this System.Security.SecureString secureString)
        {
            if (secureString == null)
                return null;// throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
