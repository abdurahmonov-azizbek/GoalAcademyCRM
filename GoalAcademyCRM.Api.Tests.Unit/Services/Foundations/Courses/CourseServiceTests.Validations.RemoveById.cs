// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidCourseId = Guid.Empty;

            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.Id),
                values: "Id is required");

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            // when
            ValueTask<Course> removeCourseByIdTask =
                this.courseService.RemoveCourseByIdAsync(invalidCourseId);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    removeCourseByIdTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(It.IsAny<Course>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfCourseIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            Course noCourse = null;

            var notFoundCourseException =
                new NotFoundCourseException(inputCourseId);

            var expectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId)).ReturnsAsync(noCourse);

            // when
            ValueTask<Course> removeCourseByIdTask =
                this.courseService.RemoveCourseByIdAsync(inputCourseId);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    removeCourseByIdTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
