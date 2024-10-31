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
        public async Task ShouldRetrieveGroupByIdAsync()
        {
            //given
            Guid randomGroupId = Guid.NewGuid();
            Guid inputGroupId = randomGroupId;
            Group randomGroup = CreateRandomGroup();
            Group storageGroup = randomGroup;
            Group excpectedGroup = randomGroup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(inputGroupId)).ReturnsAsync(storageGroup);

            //when
            Group actuallGroup = await this.groupService.RetrieveGroupByIdAsync(inputGroupId);

            //then
            actuallGroup.Should().BeEquivalentTo(excpectedGroup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(inputGroupId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}