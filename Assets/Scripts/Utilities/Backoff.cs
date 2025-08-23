using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EEA.Utilities
{
    public class Backoff
    {
        public async UniTask DoAsync(
            Func<UniTask> action,
            Func<bool> validateResult = null,
            int maxRetries = 10,
            int maxDelayMilliseconds = 2000,
            int delayMilliseconds = 200,
            CancellationToken cancellationToken = default)
        {

            UnityEngine.Debug.Log("Backoff starting");

            var backoff = new ExponentialBackoff(delayMilliseconds, maxDelayMilliseconds);
            var exceptions = new List<Exception>();

            for (var retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Run action on Unity main thread and await result
                    await action();

                    bool isValid = validateResult?.Invoke() ?? true;
                    if (isValid)
                    {
                        return;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }

                // Wait with exponential backoff before retrying
                try
                {
                    await backoff.Delay(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    throw new AggregateException("Operation was canceled.", exceptions);
                }
            }

            throw new AggregateException(exceptions);
        }

        private struct ExponentialBackoff
        {
            private readonly int _delayMilliseconds;
            private readonly int _maxDelayMilliseconds;
            private int _retries;

            public ExponentialBackoff(int delayMilliseconds, int maxDelayMilliseconds)
            {
                _delayMilliseconds = delayMilliseconds;
                _maxDelayMilliseconds = maxDelayMilliseconds;
                _retries = 0;
            }

            public UniTask Delay(CancellationToken cancellationToken = default)
            {
                _retries++;
                // Exponential growth
                int delay = Math.Min(_delayMilliseconds * (1 << (_retries - 1)), _maxDelayMilliseconds);
                return UniTask.Delay(delay, cancellationToken: cancellationToken);
            }
        }
    }
}