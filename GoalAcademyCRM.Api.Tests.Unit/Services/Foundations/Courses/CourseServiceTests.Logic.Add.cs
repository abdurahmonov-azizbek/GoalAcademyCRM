// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldAddCourseAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Course randomCourse = CreateRandomCourse(randomDate);
            Course inputCourse = randomCourse;
            Course persistedCourse = inputCourse;
            Course expectedCourse = persistedCourse.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCourseAsync(inputCourse)).ReturnsAsync(persistedCourse);

            // when
            Course actualCourse = await this.courseService
                .AddCourseAsync(inputCourse);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(inputCourse), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}