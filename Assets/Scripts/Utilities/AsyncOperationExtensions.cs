using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Utilities
{
    public static class AsyncOperationExtensions
    {
        public static Task AsTask(this AsyncOperation operation)
        {
            var tcs = new TaskCompletionSource<bool>();

            operation.completed += _ => tcs.SetResult(true);

            return tcs.Task;
        }
    }
}