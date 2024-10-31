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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedCourseException =
                new LockedCourseException(databaseUpdateConcurrencyException);

            var expectedCourseDependencyValidationException =
                new CourseDependencyValidationException(lockedCourseException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Course> removeCourseByIdTask =
               this.courseService.RemoveCourseByIdAsync(someCourseId);

            CourseDependencyValidationException actualCourseDependencyValidationException =
                await Assert.ThrowsAsync<CourseDependencyValidationException>(
                    removeCourseByIdTask.AsTask);

            // then
            actualCourseDependencyValidationException.Should().BeEquivalentTo(
               expectedCourseDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(It.IsAny<Course>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedCourseStorageException =
                new FailedCourseStorageException(sqlException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someId))
                    .Throws(sqlException);
            // when
            ValueTask<Course> removeCourseTask =
                this.courseService.RemoveCourseByIdAsync(someId);

            CourseDependencyException actualCourseDependencyException =
                await Assert.ThrowsAsync<CourseDependencyException>(
                    removeCourseTask.AsTask);

            // then
            actualCourseDependencyException.Should().BeEquivalentTo(expectedCourseDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someCourseId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCourseServiceException =
                new FailedCourseServiceException(serviceException);

            var expectedCourseServiceException =
                new CourseServiceException(failedCourseServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(someCourseId))
                    .Throws(serviceException);

            // when
            ValueTask<Course> removeCourseByIdTask =
                this.courseService.RemoveCourseByIdAsync(someCourseId);

            CourseServiceException actualCourseServiceException =
                await Assert.ThrowsAsync<CourseServiceException>(
                    removeCourseByIdTask.AsTask);

            // then
            actualCourseServiceException.Should().BeEquivalentTo(expectedCourseServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}