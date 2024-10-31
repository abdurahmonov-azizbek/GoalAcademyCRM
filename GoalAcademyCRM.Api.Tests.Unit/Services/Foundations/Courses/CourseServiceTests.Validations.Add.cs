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
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Course nullCourse = null;
            var nullCourseException = new NullCourseException();

            var expectedCourseValidationException =
                new CourseValidationException(nullCourseException);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(nullCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(addCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedCourseValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(It.IsAny<Course>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfJobIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            Course invalidCourse = new Course()
            {
                Name = invalidText
            };

            var invalidCourseException = new InvalidCourseException();

				invalidCourseException.AddData(
					key: nameof(Course.Id),
					values: "Id is required");

				invalidCourseException.AddData(
					key: nameof(Course.Name),
					values: "Text is required");

				invalidCourseException.AddData(
					key: nameof(Course.CreatedDate),
					values: "Date is required");

				invalidCourseException.AddData(
					key: nameof(Course.UpdatedDate),
					values: "Date is required");



            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(addCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(It.IsAny<Course>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudlThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNumber();
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Course randomCourse = CreateRandomCourse(randomDate);
            Course invalidCourse = randomCourse;
            invalidCourse.UpdatedDate = randomDate.AddMinutes(randomMinutes);
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.CreatedDate),
                values: $"Date is not same as {nameof(Course.UpdatedDate)}");

            var expectedCourseValidationException = new CourseValidationException(invalidCourseException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Returns(randomDate);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(addCourseTask.AsTask);

            // then
            actualCourseValidationException.Should().BeEquivalentTo(expectedCourseValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(It.IsAny<Course>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinutes))]
        public async Task ShouldThrowValidationExceptionIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidMinutes)
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            DateTimeOffset invalidDateTime = randomDate.AddMinutes(invalidMinutes);
            Course randomCourse = CreateRandomCourse(invalidDateTime);
            Course invalidCourse = randomCourse;
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.CreatedDate),
                values: "Date is not recent");

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(addCourseTask.AsTask);

            // then
            actualCourseValidationException.Should().
                BeEquivalentTo(expectedCourseValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCourseAsync(It.IsAny<Course>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}