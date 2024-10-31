// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedCourseStorageException(sqlException);

            var expectedCourseDependencyException =
                new CourseDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourses()).Throws(sqlException);

            //when
            Action retrieveAllCoursesAction = () =>
                this.courseService.RetrieveAllCourses();

            CourseDependencyException actualCourseDependencyException =
                Assert.Throws<CourseDependencyException>(retrieveAllCoursesAction);

            //then
            actualCourseDependencyException.Should().BeEquivalentTo(
                expectedCourseDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedCourseDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedCourseServiceException =
                new FailedCourseServiceException(serviceException);

            var expectedCourseServiceException =
                new CourseServiceException(failedCourseServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllCourses()).Throws(serviceException);

            // when
            Action retrieveAllCoursesAction = () =>
                this.courseService.RetrieveAllCourses();

            CourseServiceException actualCourseServiceException =
                Assert.Throws<CourseServiceException>(retrieveAllCoursesAction);

            // then
            actualCourseServiceException.Should().BeEquivalentTo(expectedCourseServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllCourses(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCourseServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}