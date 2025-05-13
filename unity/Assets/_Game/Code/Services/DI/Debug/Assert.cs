using System;

namespace Code.DI
{
    internal static class Assert
    {
        public static void This(bool condition, string message)
        {
            if (condition)
            {
                return;
            }

            throw new Exception(message);
        }
    }
}