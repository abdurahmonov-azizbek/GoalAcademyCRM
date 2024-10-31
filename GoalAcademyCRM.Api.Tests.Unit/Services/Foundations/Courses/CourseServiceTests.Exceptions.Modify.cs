// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse();
            Course someCourse = randomCourse;
            Guid courseId = someCourse.Id;
            SqlException sqlException = CreateSqlException();

            var failedCourseStorageException =
                new FailedCourseStorageException(sqlException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(someCourse);

            CourseDependencyException actualCourseDependencyException =
                await Assert.ThrowsAsync<CourseDependencyException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should().BeEquivalentTo(
                expectedCourseDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(courseId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCourseAsync(someCourse), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Course randomCourse = CreateRandomCourse(randomDateTime);
            Course someCourse = randomCourse;
            someCourse.CreatedDate = someCourse.CreatedDate.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedStorageCourseException =
                new FailedCourseStorageException(databaseUpdateException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedStorageCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(someCourse);

            CourseDependencyException actualCourseDependencyException =
                await Assert.ThrowsAsync<CourseDependencyException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should().BeEquivalentTo(expectedCourseDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDateTime);
            Course someCourse = randomCourse;
            someCourse.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCourseException =
                new LockedCourseException(databaseUpdateConcurrencyException);

            var expectedCourseDependencyValidationException =
                new CourseDependencyValidationException(lockedCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(someCourse);

            CourseDependencyValidationException actualCourseDependencyValidationException =
                await Assert.ThrowsAsync<CourseDependencyValidationException>(modifyCourseTask.AsTask);

            // then
            actualCourseDependencyValidationException.Should()
                .BeEquivalentTo(expectedCourseDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            var randomDateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(randomDateTime);
            Course someCourse = randomCourse;
            someCourse.CreatedDate = someCourse.CreatedDate.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedCourseServiceException =
                new FailedCourseServiceException(serviceException);

            var expectedCourseServiceException =
                new CourseServiceException(failedCourseServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Course> modifyCourseTask =
                this.courseService.ModifyCourseAsync(someCourse);

            CourseServiceException actualCourseServiceException =
                await Assert.ThrowsAsync<CourseServiceException>(
                    modifyCourseTask.AsTask);

            // then
            actualCourseServiceException.Should().BeEquivalentTo(expectedCourseServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
