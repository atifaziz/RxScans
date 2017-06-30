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

namespace RxScans
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Reactive.Linq;

    public static class Scanners
    {
        public static IObservable<int> ScanSum(this IObservable<int> source) =>
            source.ScanSum(x => x);

        public static IObservable<int> ScanSum<T>(
            this IObservable<T> source, Func<T, int> selector) =>
            source.Select(selector).Scan(0, (s, x) => s + x);

        public static IObservable<long> ScanSum(this IObservable<long> source) =>
            source.ScanSum(x => x);

        public static IObservable<long> ScanSum<T>(
            this IObservable<T> source, Func<T, long> selector) =>
            source.Select(selector).Scan(0L, (s, x) => s + x);

        public static IObservable<float> ScanSum(this IObservable<float> source) =>
            source.ScanSum(x => x);

        public static IObservable<float> ScanSum<T>(
            this IObservable<T> source, Func<T, float> selector) =>
            source.Select(selector).Scan(0f, (s, x) => s + x);

        public static IObservable<double> ScanSum(this IObservable<double> source) =>
            source.ScanSum(x => x);

        public static IObservable<double> ScanSum<T>(
            this IObservable<T> source, Func<T, double> selector) =>
            source.Select(selector).Scan(0.0, (s, x) => s + x);

        public static IObservable<int> ScanCount<T>(this IObservable<T> source) =>
            source.ScanCount(_ => true);

        public static IObservable<int> ScanCount<T>(
            this IObservable<T> source, Func<T, bool> predicate) =>
            source.Scan(0, (s, e) => s + (predicate(e) ? 1 : 0));

        public static IObservable<double> ScanAverage(this IObservable<int> source) =>
            source.ScanAverage(x => x);

        public static IObservable<double> ScanAverage<T>(
            this IObservable<T> source, Func<T, int> selector) =>
            source.ScanAverage(selector, (sum, x) => sum + x);

        public static IObservable<double> ScanAverage(this IObservable<long> source) =>
            source.ScanAverage(x => x);

        public static IObservable<double> ScanAverage<T>(
            this IObservable<T> source, Func<T, long> selector) =>
            source.ScanAverage(selector, (sum, x) => sum + x);

        public static IObservable<double> ScanAverage(this IObservable<float> source) =>
            source.ScanAverage(x => x);

        public static IObservable<double> ScanAverage<T>(
            this IObservable<T> source, Func<T, float> selector) =>
            source.ScanAverage(selector, (sum, x) => sum + x);

        public static IObservable<double> ScanAverage(this IObservable<double> source) =>
            source.ScanAverage(x => x);

        public static IObservable<double> ScanAverage<T>(
            this IObservable<T> source, Func<T, double> selector) =>
            source.ScanAverage(selector, (sum, x) => sum + x);

        static IObservable<double> ScanAverage<TSource, TElement>(
            this IObservable<TSource> source,
            Func<TSource, TElement> selector,
            Func<double, TElement, double> adder) =>
            source.Select(selector)
                  .Scan((Count: 0, Sum: 0.0), (s, x) => (s.Count + 1, adder(s.Sum, x)))
                  .Select(e => e.Sum / e.Count);

        public static IObservable<T> ScanMin<T>(this IObservable<T> source)
            where T : IComparable<T> =>
            source.ScanMin(x => x);

        public static IObservable<TResult> ScanMin<T, TResult>(this IObservable<T> source, Func<T, TResult> selector)
            where TResult : IComparable<TResult> =>
            source.Select(selector).ScanComparison(Comparison.Lesser);

        public static IObservable<T> ScanMax<T>(this IObservable<T> source)
            where T : IComparable<T> =>
            source.ScanMax(x => x);

        public static IObservable<TResult> ScanMax<T, TResult>(this IObservable<T> source, Func<T, TResult> selector)
            where TResult : IComparable<TResult> =>
            source.Select(selector).ScanComparison(Comparison.Greater);

        enum Comparison { Lesser = -1, Greater = 1 }

        static IObservable<T> ScanComparison<T>(
            this IObservable<T> source, Comparison comparison)
            where T : IComparable<T> =>
            source.Scan((s, e) => Math.Sign(e.CompareTo(s)) == (int) comparison ? e : s);

        public static IObservable<ISet<T>> ScanDistinct<T>(this IObservable<T> source) =>
            source.ScanDistinct(null);

        public static IObservable<ISet<T>> ScanDistinct<T>(this IObservable<T> source, IEqualityComparer<T> comparer) =>
            source.Scan(ImmutableHashSet<T>.Empty, (s, e) => s.Add(e));
    }
}
