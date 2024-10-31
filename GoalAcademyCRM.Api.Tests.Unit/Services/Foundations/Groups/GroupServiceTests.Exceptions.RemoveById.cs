// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someGroupId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGroupException =
                new LockedGroupException(databaseUpdateConcurrencyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(lockedGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Group> removeGroupByIdTask =
               this.groupService.RemoveGroupByIdAsync(someGroupId);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                await Assert.ThrowsAsync<GroupDependencyValidationException>(
                    removeGroupByIdTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
               expectedGroupDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupAsync(It.IsAny<Group>()), Times.Never);

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

            var failedGroupStorageException =
                new FailedGroupStorageException(sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(someId))
                    .Throws(sqlException);
            // when
            ValueTask<Group> removeGroupTask =
                this.groupService.RemoveGroupByIdAsync(someId);

            GroupDependencyException actualGroupDependencyException =
                await Assert.ThrowsAsync<GroupDependencyException>(
                    removeGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(expectedGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someGroupId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedGroupServiceException =
                new FailedGroupServiceException(serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(failedGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(someGroupId))
                    .Throws(serviceException);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(someGroupId);

            GroupServiceException actualGroupServiceException =
                await Assert.ThrowsAsync<GroupServiceException>(
                    removeGroupByIdTask.AsTask);

            // then
            actualGroupServiceException.Should().BeEquivalentTo(expectedGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}