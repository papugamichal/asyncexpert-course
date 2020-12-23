﻿using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `Join` to wait until created Thread finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            var thread = new Thread(DoJob);
            thread.Start();
            thread.Join();
            void DoJob()
            {
                int loop = 0;

                try
                {
                    do
                    {
                        loop += 1;
                        if (token.IsCancellationRequested)
                        {
                            throw new OperationCanceledException();
                        }
                        action?.Invoke();
                    }
                    while (loop < repeats);
                }
                catch (Exception e)
                {
                    errorAction?.Invoke(e);
                }
            }
        }

        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)

            using var finished = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(DoJob);
            finished.WaitOne();

            void DoJob(object? state)
            {
                int loop = 0;

                try
                {
                    do
                    {
                        loop += 1;

                        token.ThrowIfCancellationRequested();
                        action?.Invoke();
                    }
                    while (loop < repeats);
                }
                catch (Exception e)
                {
                    errorAction?.Invoke(e);
                }

                finished.Set();
            }
        }
    }
}
