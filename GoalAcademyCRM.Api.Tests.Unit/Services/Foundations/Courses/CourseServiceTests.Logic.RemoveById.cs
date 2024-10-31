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
        public async Task ShouldRemoveCourseByIdAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            Course randomCourse = CreateRandomCourse();
            Course storageCourse = randomCourse;
            Course expectedInputCourse = storageCourse;
            Course deletedCourse = expectedInputCourse;
            Course expectedCourse = deletedCourse.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ReturnsAsync(storageCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCourseAsync(expectedInputCourse))
                    .ReturnsAsync(deletedCourse);

            // when
            Course actualCourse = await this.courseService
                .RemoveCourseByIdAsync(inputCourseId);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(expectedInputCourse), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
