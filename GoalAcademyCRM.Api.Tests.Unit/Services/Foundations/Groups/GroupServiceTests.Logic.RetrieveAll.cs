// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System.Linq;
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
        public void ShouldRetrieveAllGroups()
        {
            //given
            IQueryable<Group> randomGroups = CreateRandomGroups();
            IQueryable<Group> storageGroups = randomGroups;
            IQueryable<Group> expectedGroups = storageGroups.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups()).Returns(storageGroups);

            //when
            IQueryable<Group> actualGroups =
                this.groupService.RetrieveAllGroups();

            //then
            actualGroups.Should().BeEquivalentTo(expectedGroups);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
