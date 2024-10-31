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
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.StudentGroups
{
    public partial class StudentGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someStudentGroupId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentGroupException =
                new LockedStudentGroupException(databaseUpdateConcurrencyException);

            var expectedStudentGroupDependencyValidationException =
                new StudentGroupDependencyValidationException(lockedStudentGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentGroup> removeStudentGroupByIdTask =
               this.studentGroupService.RemoveStudentGroupByIdAsync(someStudentGroupId);

            StudentGroupDependencyValidationException actualStudentGroupDependencyValidationException =
                await Assert.ThrowsAsync<StudentGroupDependencyValidationException>(
                    removeStudentGroupByIdTask.AsTask);

            // then
            actualStudentGroupDependencyValidationException.Should().BeEquivalentTo(
               expectedStudentGroupDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGroupAsync(It.IsAny<StudentGroup>()), Times.Never);

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

            var failedStudentGroupStorageException =
                new FailedStudentGroupStorageException(sqlException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(someId))
                    .Throws(sqlException);
            // when
            ValueTask<StudentGroup> removeStudentGroupTask =
                this.studentGroupService.RemoveStudentGroupByIdAsync(someId);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(
                    removeStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(expectedStudentGroupDependencyException);

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
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someStudentGroupId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedStudentGroupServiceException =
                new FailedStudentGroupServiceException(serviceException);

            var expectedStudentGroupServiceException =
                new StudentGroupServiceException(failedStudentGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(someStudentGroupId))
                    .Throws(serviceException);

            // when
            ValueTask<StudentGroup> removeStudentGroupByIdTask =
                this.studentGroupService.RemoveStudentGroupByIdAsync(someStudentGroupId);

            StudentGroupServiceException actualStudentGroupServiceException =
                await Assert.ThrowsAsync<StudentGroupServiceException>(
                    removeStudentGroupByIdTask.AsTask);

            // then
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