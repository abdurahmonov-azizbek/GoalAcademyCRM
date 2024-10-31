// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGroupId = Guid.Empty;

            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.Id),
                values: "Id is required");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(invalidGroupId);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    removeGroupByIdTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteGroupAsync(It.IsAny<Group>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfGroupIsNotFoundAndLogItAsync()
        {
            // given
            Guid randomGroupId = Guid.NewGuid();
            Guid inputGroupId = randomGroupId;
            Group noGroup = null;

            var notFoundGroupException =
                new NotFoundGroupException(inputGroupId);

            var expectedGroupValidationException =
                new GroupValidationException(notFoundGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(inputGroupId)).ReturnsAsync(noGroup);

            // when
            ValueTask<Group> removeGroupByIdTask =
                this.groupService.RemoveGroupByIdAsync(inputGroupId);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    removeGroupByIdTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
