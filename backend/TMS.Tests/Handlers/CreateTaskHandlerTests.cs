using FluentAssertions;

namespace TMS.Tests.Handlers
{
    public class CreateTaskHandlerTests
    {
        [Fact]
        public void SmokeTest_ProjectCompiles()
        {
            var expected = true;

            var actual = true;

            actual.Should().Be(expected);
        }
    }
}
