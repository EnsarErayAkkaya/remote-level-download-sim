using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace EEA.Utilities
{
    public static class Backoff
    {
        public static async UniTask DoAsync(
            Func<UniTask> action,
            Func<bool> validateResult = null,
            int maxRetries = 10,
            int maxDelayMilliseconds = 2000,
            int delayMilliseconds = 200,
            CancellationToken cancellationToken = default)
        {
            var backoff = new ExponentialBackoff(delayMilliseconds, maxDelayMilliseconds);
            var exceptions = ClassPool<List<Exception>>.Spawn() ?? new List<Exception>();
            exceptions.Clear();

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
                        ClassPool<List<Exception>>.Despawn(exceptions);

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
                    var aggregateException_1 = new AggregateException("Operation was canceled.", exceptions);

                    ClassPool<List<Exception>>.Despawn(exceptions);

                    throw aggregateException_1;
                }
            }

            var aggregateException = new AggregateException(exceptions);

            ClassPool<List<Exception>>.Despawn(exceptions);

            throw aggregateException;
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