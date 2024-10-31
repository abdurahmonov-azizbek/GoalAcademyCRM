// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.StudentGroups
{
    public partial class StudentGroupServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedStudentGroupStorageException(sqlException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGroups()).Throws(sqlException);

            //when
            Action retrieveAllStudentGroupsAction = () =>
                this.studentGroupService.RetrieveAllStudentGroups();

            StudentGroupDependencyException actualStudentGroupDependencyException =
                Assert.Throws<StudentGroupDependencyException>(retrieveAllStudentGroupsAction);

            //then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(
                expectedStudentGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedStudentGroupServiceException =
                new FailedStudentGroupServiceException(serviceException);

            var expectedStudentGroupServiceException =
                new StudentGroupServiceException(failedStudentGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGroups()).Throws(serviceException);

            // when
            Action retrieveAllStudentGroupsAction = () =>
                this.studentGroupService.RetrieveAllStudentGroups();

            StudentGroupServiceException actualStudentGroupServiceException =
                Assert.Throws<StudentGroupServiceException>(retrieveAllStudentGroupsAction);

            // then
            actualStudentGroupServiceException.Should().BeEquivalentTo(expectedStudentGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}