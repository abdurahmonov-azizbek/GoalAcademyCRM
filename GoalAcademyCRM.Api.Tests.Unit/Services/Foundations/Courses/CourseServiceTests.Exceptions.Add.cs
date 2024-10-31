// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Course someCourse = CreateRandomCourse();
            Guid courseId = someCourse.Id;
            SqlException sqlException = CreateSqlException();

            FailedCourseStorageException failedCourseStorageException =
                new FailedCourseStorageException(sqlException);

            CourseDependencyException expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Course> addCourseTask = this.courseService
                .AddCourseAsync(someCourse);

            CourseDependencyException actualCourseDependencyException =
                await Assert.ThrowsAsync<CourseDependencyException>(addCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should().BeEquivalentTo(expectedCourseDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCourseDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogItAsync()
        {
            // given
            Course someCourse = CreateRandomCourse();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsCourseException =
                new AlreadyExistsCourseException(duplicateKeyException);

            var expectedCourseDependencyValidationException =
                new CourseDependencyValidationException(alreadyExistsCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(someCourse);

            CourseDependencyValidationException actualCourseDependencyValidationException =
                await Assert.ThrowsAsync<CourseDependencyValidationException>(
                    addCourseTask.AsTask);

            // then
            actualCourseDependencyValidationException.Should().BeEquivalentTo(
                expectedCourseDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            Course someCourse = CreateRandomCourse();
            var dbUpdateException = new DbUpdateException();

            var failedCourseStorageException = new FailedCourseStorageException(dbUpdateException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(someCourse);

            CourseDependencyException actualCourseDependencyException =
                 await Assert.ThrowsAsync<CourseDependencyException>(addCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should()
                .BeEquivalentTo(expectedCourseDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedCourseDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateCourseAsync(It.IsAny<Course>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateExceptionOccuredAndLogItAsync()
        {
            // given
            Course someCourse = CreateRandomCourse();
            string someMessage = GetRandomString();

            var dbUpdateException =
                new DbUpdateException(someMessage);

            var failedCourseStorageException =
                new FailedCourseStorageException(dbUpdateException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Throws(dbUpdateException);

            // when
            ValueTask<Course> addCourseTask =
                this.courseService.AddCourseAsync(someCourse);

            CourseDependencyException actualCourseDependencyException =
                await Assert.ThrowsAsync<CourseDependencyException>(addCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should().BeEquivalentTo(expectedCourseDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogItAsync()
        {
            //given
            Course someCourse = CreateRandomCourse();
            var serviceException = new Exception();
            var failedCourseException = new FailedCourseServiceException(serviceException);

            var expectedCourseServiceExceptions =
                new CourseServiceException(failedCourseException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(serviceException);

            //when
            ValueTask<Course> addCourseTask = this.courseService.AddCourseAsync(someCourse);

            CourseServiceException actualCourseServiceException =
                await Assert.ThrowsAsync<CourseServiceException>(addCourseTask.AsTask);

            //then
            actualCourseServiceException.Should().BeEquivalentTo(
                expectedCourseServiceExceptions);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseServiceExceptions))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}