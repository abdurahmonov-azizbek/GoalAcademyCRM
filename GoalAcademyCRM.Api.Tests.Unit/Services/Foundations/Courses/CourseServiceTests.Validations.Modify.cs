// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCourseIsNullAndLogItAsync()
        {
            // given
            Course nullCourse = null;
            var nullCourseException = new NullCourseException();

            var expectedCourseValidationException =
                new CourseValidationException(nullCourseException);

            // when
            ValueTask<Course> modifyCourseTask = this.courseService.ModifyCourseAsync(nullCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfCourseIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            Course invalidCourse = new Course
            {
                Name = invalidString
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
                values: new[]
                    {
                        "Date is required",
                        "Date is not recent",
                        $"Date is the same as {nameof(Course.CreatedDate)}"
                    }
                );

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Course> modifyCourseTask = this.courseService.ModifyCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDateTime);
            Course invalidCourse = randomCourse;
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.UpdatedDate),
                values: $"Date is the same as {nameof(Course.CreatedDate)}");

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                 await Assert.ThrowsAsync<CourseValidationException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course inputCourse = randomCourse;
            inputCourse.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.UpdatedDate),
                values: "Date is not recent");

            var expectedCourseValidatonException =
                new CourseValidationException(invalidCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(inputCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidatonException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCourseDoesNotExistAndLogItAsync()
        {
            // given
            int negativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Course nonExistCourse = randomCourse;
            nonExistCourse.CreatedDate = dateTime.AddMinutes(negativeMinutes);
            Course nullCourse = null;

            var notFoundCourseException = new NotFoundCourseException(nonExistCourse.Id);

            var expectedCourseValidationException =
                new CourseValidationException(notFoundCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(nonExistCourse.Id))
                    .ReturnsAsync(nullCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(nonExistCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(nonExistCourse.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomModifyCourse(randomDateTime);
            Course invalidCourse = randomCourse.DeepClone();
            Course storageCourse = invalidCourse.DeepClone();
            storageCourse.CreatedDate = storageCourse.CreatedDate.AddMinutes(randomMinutes);
            storageCourse.UpdatedDate = storageCourse.UpdatedDate.AddMinutes(randomMinutes);
            var invalidCourseException = new InvalidCourseException();
            Guid courseId = invalidCourse.Id;

            invalidCourseException.AddData(
                key: nameof(Course.CreatedDate),
                values: $"Date is not same as {nameof(Course.CreatedDate)}");

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(courseId)).ReturnsAsync(storageCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomModifyCourse(randomDateTime);
            Course invalidCourse = randomCourse;
            Course storageCourse = randomCourse.DeepClone();
            invalidCourse.UpdatedDate = storageCourse.UpdatedDate;
            Guid courseId = invalidCourse.Id;
            var invalidCourseException = new InvalidCourseException();

            invalidCourseException.AddData(
                key: nameof(Course.UpdatedDate),
                values: $"Date is the same as {nameof(Course.UpdatedDate)}");

            var expectedCourseValidationException =
                new CourseValidationException(invalidCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(invalidCourse.Id)).ReturnsAsync(storageCourse);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(invalidCourse);

            CourseValidationException actualCourseValidationException =
                await Assert.ThrowsAsync<CourseValidationException>(modifyCourseTask.AsTask);

            // then
            actualCourseValidationException.Should()
                .BeEquivalentTo(expectedCourseValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(courseId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
