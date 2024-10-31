// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedStudentGroupStorageException =
                new FailedStudentGroupStorageException(sqlException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<StudentGroup> retrieveStudentGroupByIdTask =
                this.studentGroupService.RetrieveStudentGroupByIdAsync(someId);

            StudentGroupDependencyException actualStudentGroupDependencyexception =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(
                    retrieveStudentGroupByIdTask.AsTask);

            //then
            actualStudentGroupDependencyexception.Should().BeEquivalentTo(
                expectedStudentGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyException))), Times.Once);

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

            var failedStudentGroupServiceException =
                new FailedStudentGroupServiceException(serviceException);

            var expectedStudentGroupServiceException =
                new StudentGroupServiceException(failedStudentGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<StudentGroup> retrieveStudentGroupByIdTask =
                this.studentGroupService.RetrieveStudentGroupByIdAsync(someId);

            StudentGroupServiceException actualStudentGroupServiceException =
                await Assert.ThrowsAsync<StudentGroupServiceException>(retrieveStudentGroupByIdTask.AsTask);

            //then
            actualStudentGroupServiceException.Should().BeEquivalentTo(expectedStudentGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}