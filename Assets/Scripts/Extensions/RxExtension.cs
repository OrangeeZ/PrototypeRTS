using System;

namespace Assets.Scripts.Extensions
{
    public static class RxExtension
    {
        public static void Cancel(this IDisposable disposable)
        {
            disposable?.Dispose();
        }
    }
}
