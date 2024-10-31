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
        public async Task ShouldModifyCourseAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomModifyCourse(randomDate);
            Course inputCourse = randomCourse;
            Course storageCourse = inputCourse.DeepClone();
            storageCourse.UpdatedDate = randomCourse.CreatedDate;
            Course updatedCourse = inputCourse;
            Course expectedCourse = updatedCourse.DeepClone();
            Guid courseId = inputCourse.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId))
                    .ReturnsAsync(storageCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateCourseAsync(inputCourse))
                    .ReturnsAsync(updatedCourse);

            // when
            Course actualCourse =
               await this.courseService.ModifyCourseAsync(inputCourse);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(courseId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCourseAsync(inputCourse), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
