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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            //given
            var invalidCourseId = Guid.Empty;
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.Id),
                values: "Id is required");

            var excpectedCourseValidationException = new
                CourseValidationException(invalidCourseException);

            //when
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(invalidCourseId);

            CourseValidationException actuallCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    retrieveCourseByIdTask.AsTask);

            //then
            actuallCourseValidationException.Should()
                .BeEquivalentTo(excpectedCourseValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    excpectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfCourseIsNotFoundAndLogItAsync()
        {
            Guid someCourseId = Guid.NewGuid();
            Course noCourse = null;

            var notFoundCourseException =
                new NotFoundCourseException(someCourseId);

            var excpectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noCourse);

            //when 
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(someCourseId);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    retrieveCourseByIdTask.AsTask);

            //then
            actualCourseValidationException.Should()
                .BeEquivalentTo(excpectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    excpectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
