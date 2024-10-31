// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            Guid groupId = someGroup.Id;
            SqlException sqlException = CreateSqlException();

            FailedGroupStorageException failedGroupStorageException =
                new FailedGroupStorageException(sqlException);

            GroupDependencyException expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Group> addGroupTask = this.groupService
                .AddGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                await Assert.ThrowsAsync<GroupDependencyException>(addGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedGroupDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsGroupException =
                new AlreadyExistsGroupException(duplicateKeyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(alreadyExistsGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(someGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                await Assert.ThrowsAsync<GroupDependencyValidationException>(
                    addGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            var dbUpdateException = new DbUpdateException();

            var failedGroupStorageException = new FailedGroupStorageException(dbUpdateException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                 await Assert.ThrowsAsync<GroupDependencyException>(addGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should()
                .BeEquivalentTo(expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedGroupDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(It.IsAny<Group>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateExceptionOccuredAndLogItAsync()
        {
            // given
            Group someGroup = CreateRandomGroup();
            string someMessage = GetRandomString();

            var dbUpdateException =
                new DbUpdateException(someMessage);

            var failedGroupStorageException =
                new FailedGroupStorageException(dbUpdateException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Throws(dbUpdateException);

            // when
            ValueTask<Group> addGroupTask =
                this.groupService.AddGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                await Assert.ThrowsAsync<GroupDependencyException>(addGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(expectedGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogItAsync()
        {
            //given
            Group someGroup = CreateRandomGroup();
            var serviceException = new Exception();
            var failedGroupException = new FailedGroupServiceException(serviceException);

            var expectedGroupServiceExceptions =
                new GroupServiceException(failedGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(serviceException);

            //when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(someGroup);

            GroupServiceException actualGroupServiceException =
                await Assert.ThrowsAsync<GroupServiceException>(addGroupTask.AsTask);

            //then
            actualGroupServiceException.Should().BeEquivalentTo(
                expectedGroupServiceExceptions);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceExceptions))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}