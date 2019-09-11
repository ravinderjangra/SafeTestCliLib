using System.Runtime.InteropServices;

namespace SafeCliCore
{
    public struct XorNameArray
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] Name;
    }
}
