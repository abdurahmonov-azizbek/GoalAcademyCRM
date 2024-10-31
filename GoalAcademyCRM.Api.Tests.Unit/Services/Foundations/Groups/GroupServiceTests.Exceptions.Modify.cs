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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomGroup();
            Group someGroup = randomGroup;
            Guid groupId = someGroup.Id;
            SqlException sqlException = CreateSqlException();

            var failedGroupStorageException =
                new FailedGroupStorageException(sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                await Assert.ThrowsAsync<GroupDependencyException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(groupId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(someGroup), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))), Times.Once);

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
            Group randomGroup = CreateRandomGroup(randomDateTime);
            Group someGroup = randomGroup;
            someGroup.CreatedDate = someGroup.CreatedDate.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedStorageGroupException =
                new FailedGroupStorageException(databaseUpdateException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedStorageGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(someGroup);

            GroupDependencyException actualGroupDependencyException =
                await Assert.ThrowsAsync<GroupDependencyException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupDependencyException.Should().BeEquivalentTo(expectedGroupDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))), Times.Once);

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
            Group randomGroup = CreateRandomGroup(randomDateTime);
            Group someGroup = randomGroup;
            someGroup.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedGroupException =
                new LockedGroupException(databaseUpdateConcurrencyException);

            var expectedGroupDependencyValidationException =
                new GroupDependencyValidationException(lockedGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(someGroup);

            GroupDependencyValidationException actualGroupDependencyValidationException =
                await Assert.ThrowsAsync<GroupDependencyValidationException>(modifyGroupTask.AsTask);

            // then
            actualGroupDependencyValidationException.Should()
                .BeEquivalentTo(expectedGroupDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupDependencyValidationException))), Times.Once);

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
            Group randomGroup = CreateRandomGroup(randomDateTime);
            Group someGroup = randomGroup;
            someGroup.CreatedDate = someGroup.CreatedDate.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedGroupServiceException =
                new FailedGroupServiceException(serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(failedGroupServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(someGroup);

            GroupServiceException actualGroupServiceException =
                await Assert.ThrowsAsync<GroupServiceException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupServiceException.Should().BeEquivalentTo(expectedGroupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
