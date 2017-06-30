#region Copyright (c) 2017 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace RxScansDemo
{
    using System;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MoreLinq;
    using RxScans;

    static class Program
    {
        static void Main(string[] args)
        {
            var sample = MoreEnumerable.Random(1000);

            var arg = args.FirstOrDefault();
            sample = arg != null
                   ? sample.Take(int.Parse(arg))
                   : sample.Pipe(_ => Thread.Sleep(TimeSpan.FromSeconds(0.1)));

            var sharedSample =
                sample.ToObservable()
                      .SubscribeOn(Scheduler.Default)
                      .Publish();

            var stats =
                sharedSample
                    .And(sharedSample.ScanSum())
                    .And(sharedSample.ScanCount())
                    .And(sharedSample.ScanAverage())
                    .And(sharedSample.ScanMin())
                    .And(sharedSample.ScanMax())
                    .Then((x, sum, count, avg, min, max) => new
                    {
                        Sample  = x,
                        Sum     = sum,
                        Average = avg,
                        Count   = count,
                        Min     = min,
                        Max     = max,
                    });

            var tcs = new TaskCompletionSource<DateTimeOffset>();

            var subscription =
                Observable
                    .When(stats)
                    .Select(e => new[]
                    {
                        $"Sample = {e.Sample,5:N0}",
                        $"Count = {e.Count,5:N0}",
                        $"Sum = {e.Sum,10:N0}",
                        $"Average = {e.Average,8:N2}",
                        $"Min = {e.Min,5:N0}",
                        $"Max = {e.Max,5:N0}"
                    })
                    .Select(cols => cols.ToDelimitedString(" | "))
                    .Subscribe(
                        Console.WriteLine,
                        e => tcs.SetException(e),
                        () => tcs.SetResult(DateTimeOffset.Now));

            using (subscription)
            {
                Console.WriteLine("Press ENTER to abort.");
                using (sharedSample.Connect())
                {
                    Task.WhenAny(tcs.Task, Task.Factory.StartNew(Console.ReadLine))
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
    }
}
