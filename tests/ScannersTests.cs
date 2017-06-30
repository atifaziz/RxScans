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

namespace RxScans.Tests
{
    using System.Linq;
    using System.Reactive.Linq;
    using Xunit;

    public class ScannersTests
    {
        [Theory]
        [InlineData(
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 })]
        public void ScanSumInts(int[] xs, int[] expectations)
        {
            var scan = xs.ToObservable().ScanSum().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new long[] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55 })]
        public void ScanSumLongs(long[] xs, long[] expectations)
        {
            var scan = xs.ToObservable().ScanSum().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 5f },
            new[] { 0.5f, 1.5f, 3f, 5f, 7.5f, 10.5f, 14f, 18f, 22.5f, 27.5f })]
        public void ScanSumSingles(float[] xs, float[] expectations)
        {
            var scan = xs.ToObservable().ScanSum().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5 },
            new[] { 0.5, 1.5, 3, 5, 7.5, 10.5, 14, 18, 22.5, 27.5 })]
        public void ScanSumDoubles(double[] xs, double[] expectations)
        {
            var scan = xs.ToObservable().ScanSum().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 },
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        public void ScanCount(int[] xs, int[] expectations)
        {
            var scan = xs.ToObservable().ScanCount().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 91, 81, 95, 69, 41, 77 },
            new[] { 91, 81, 81, 69, 41, 41 })]
        public void ScanMin(int[] xs, int[] expectations)
        {
            var scan = xs.ToObservable().ScanMin().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 91, 81, 95, 69, 41, 77 },
            new[] { 91, 91, 95, 95, 95, 95 })]
        public void ScanMax(int[] xs, int[] expectations)
        {
            var scan = xs.ToObservable().ScanMax().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 2, 3, 2, 2, 4, 1 }, new object[]
            {
                new[] { 2 },
                new[] { 2, 3 },
                new[] { 2, 3 },
                new[] { 2, 3 },
                new[] { 2, 3, 4 },
                new[] { 2, 3, 4, 1 },
            })]
        public void ScanDistinct(int[] xs, object[] expectations)
        {
            var results =
                xs.ToObservable()
                  .ScanDistinct()
                  .ToEnumerable()
                  .Zip(expectations, (s, e) => new { Scan = s,
                                                     Expecation = (int[]) e });

            foreach (var e in results)
                Assert.Equal(e.Scan.OrderBy(x => x), e.Expecation.OrderBy(x => x));
        }

        [Theory]
        [InlineData(
            new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5 })]
        public void ScanAverageInts(int[] xs, double[] expectations)
        {
            var scan = xs.ToObservable().ScanAverage().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 1L, 2L, 3L, 4L, 5L, 6L, 7L, 8L, 9L, 10L },
            new[] { 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5 })]
        public void ScanAverageLongs(long[] xs, double[] expectations)
        {
            var scan = xs.ToObservable().ScanAverage().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f, 5f },
            new[] { 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75 })]
        public void ScanAverageSingles(float[] xs, double[] expectations)
        {
            var scan = xs.ToObservable().ScanAverage().ToEnumerable();
            Assert.Equal(expectations, scan);
        }

        [Theory]
        [InlineData(
            new[] { 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5 },
            new[] { 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2.0, 2.25, 2.5, 2.75 })]
        public void ScanAverageDoubles(double[] xs, double[] expectations)
        {
            var scan = xs.ToObservable().ScanAverage().ToEnumerable();
            Assert.Equal(expectations, scan);
        }
    }
}
