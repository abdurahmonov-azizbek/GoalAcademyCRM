// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System.Linq;
using GoalAcademyCRM.Api.Models.Courses;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllCourses()
        {
            //given
            IQueryable<Course> randomCourses = CreateRandomCourses();
            IQueryable<Course> storageCourses = randomCourses;
            IQueryable<Course> expectedCourses = storageCourses.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourses()).Returns(storageCourses);

            //when
            IQueryable<Course> actualCourses =
                this.courseService.RetrieveAllCourses();

            //then
            actualCourses.Should().BeEquivalentTo(expectedCourses);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourses(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
