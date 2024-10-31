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
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedCourseStorageException =
                new FailedCourseStorageException(sqlException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedCourseStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(someId);

            CourseDependencyException actualCourseDependencyexception =
                await Assert.ThrowsAsync<CourseDependencyException>(
                    retrieveCourseByIdTask.AsTask);

            //then
            actualCourseDependencyexception.Should().BeEquivalentTo(
                expectedCourseDependencyException);

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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedCourseServiceException =
                new FailedCourseServiceException(serviceException);

            var expectedCourseServiceException =
                new CourseServiceException(failedCourseServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Course> retrieveCourseByIdTask =
                this.courseService.RetrieveCourseByIdAsync(someId);

            CourseServiceException actualCourseServiceException =
                await Assert.ThrowsAsync<CourseServiceException>(retrieveCourseByIdTask.AsTask);

            //then
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