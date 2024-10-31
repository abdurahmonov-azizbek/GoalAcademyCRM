// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System.Linq;
using GoalAcademyCRM.Api.Models.StudentGroups;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.StudentGroups
{
    public partial class StudentGroupServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudentGroups()
        {
            //given
            IQueryable<StudentGroup> randomStudentGroups = CreateRandomStudentGroups();
            IQueryable<StudentGroup> storageStudentGroups = randomStudentGroups;
            IQueryable<StudentGroup> expectedStudentGroups = storageStudentGroups.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentGroups()).Returns(storageStudentGroups);

            //when
            IQueryable<StudentGroup> actualStudentGroups =
                this.studentGroupService.RetrieveAllStudentGroups();

            //then
            actualStudentGroups.Should().BeEquivalentTo(expectedStudentGroups);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentGroups(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
