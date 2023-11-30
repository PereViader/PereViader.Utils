using System.Collections.Generic;
using NUnit.Framework;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Test
{
    [TestFixture]
    public class ReadOnlyListExtensionsTests
    {
        [Test]
        public void Find_ReturnsMatchingElement()
        {
            IReadOnlyList<int> list = new List<int>() { 1, 2, 3, 4, 5 };
            int result = list.Find(x => x == 3);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void Find_ReturnsDefaultIfNotFound()
        {
            IReadOnlyList<int> list = new List<int> { 1, 2, 3, 4, 5 };
            int result = list.Find(x => x == 6);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Find_WithArg_ReturnsMatchingElement()
        {
            IReadOnlyList<int> list = new List<int> { 1, 2, 3, 4, 5 };
            int result = list.Find((x, arg) => x == arg, 3);
            Assert.AreEqual(3, result);
        }

        [Test]
        public void FindAll_ReturnsMatchingElements()
        {
            IReadOnlyList<int> list = new List<int> { 1, 2, 3, 4, 5, 3 };
            List<int> result = list.FindAll(x => x == 3, new List<int>());
            CollectionAssert.AreEqual(new List<int> { 3, 3 }, result);
        }
    }
}