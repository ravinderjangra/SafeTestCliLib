using System;
using System.Collections.Generic;
using System.Text;

namespace SafeCliCore
{
    public class SafeAppPtr
    {
        public IntPtr Value { get; private set; }

        public SafeAppPtr(IntPtr appPtr)
        {
            Value = appPtr;
        }

        public SafeAppPtr()
            : this(IntPtr.Zero)
        {
        }

        public static implicit operator IntPtr(SafeAppPtr obj)
        {
            return obj.Value;
        }

        public void Clear()
        {
            Value = IntPtr.Zero;
        }
    }
}
