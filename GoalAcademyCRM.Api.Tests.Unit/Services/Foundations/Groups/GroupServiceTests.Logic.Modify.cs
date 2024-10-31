// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldModifyGroupAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Group randomGroup = CreateRandomModifyGroup(randomDate);
            Group inputGroup = randomGroup;
            Group storageGroup = inputGroup.DeepClone();
            storageGroup.UpdatedDate = randomGroup.CreatedDate;
            Group updatedGroup = inputGroup;
            Group expectedGroup = updatedGroup.DeepClone();
            Guid groupId = inputGroup.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(groupId))
                    .ReturnsAsync(storageGroup);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateGroupAsync(inputGroup))
                    .ReturnsAsync(updatedGroup);

            // when
            Group actualGroup =
               await this.groupService.ModifyGroupAsync(inputGroup);

            // then
            actualGroup.Should().BeEquivalentTo(expectedGroup);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(groupId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateGroupAsync(inputGroup), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
