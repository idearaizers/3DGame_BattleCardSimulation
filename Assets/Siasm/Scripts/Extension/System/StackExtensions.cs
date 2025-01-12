using System.Collections.Generic;

namespace Siasm
{
    public static class StackExtensions
    {
        public static bool IsEmpty<T>(this Stack<T> stack) where T : class
        {
            return stack.Count == 0;
        }
    }
}
