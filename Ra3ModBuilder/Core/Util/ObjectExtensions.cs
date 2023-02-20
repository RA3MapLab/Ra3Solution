using System;

namespace Ra3ModBuilder.Core.Util
{
    public static class ObjectExtensions 
    {
        // Kotlin: fun <T, R> T.let(block: (T) -> R): R
        public static R let<T, R>(this T self, Func<T, R> block) 
        {
            return block(self);
        }

        // Kotlin: fun <T> T.also(block: (T) -> Unit): T
        public static T also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }   
    }
}