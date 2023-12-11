using NUnit.Framework;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.Test
{
    [GenerateId]
    public partial struct TestId
    {
    }

    [TestFixture]
    public class IdSourceGeneratorTest
    {
        [Test]
        public void GeneratedId_Compiles()
        {
            _ = GeneratedId.Empty;
            GeneratedId.New().ToGuid();
            GeneratedId.FromGuid(Guid.NewGuid());
        }
    }
}
