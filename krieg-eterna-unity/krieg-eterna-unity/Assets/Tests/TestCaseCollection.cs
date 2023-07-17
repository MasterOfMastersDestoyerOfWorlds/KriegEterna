
namespace KriegTests
{
    using System.Collections.Generic;
    using TestCase = TestCards.TestCase;
    public abstract class TestCaseCollection
    {
        public abstract  List<TestCase> getCases();
    }
}