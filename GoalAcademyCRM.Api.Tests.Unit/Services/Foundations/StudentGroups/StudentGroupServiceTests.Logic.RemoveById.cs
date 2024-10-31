// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldRemoveStudentGroupByIdAsync()
        {
            // given
            Guid randomStudentGroupId = Guid.NewGuid();
            Guid inputStudentGroupId = randomStudentGroupId;
            StudentGroup randomStudentGroup = CreateRandomStudentGroup();
            StudentGroup storageStudentGroup = randomStudentGroup;
            StudentGroup expectedInputStudentGroup = storageStudentGroup;
            StudentGroup deletedStudentGroup = expectedInputStudentGroup;
            StudentGroup expectedStudentGroup = deletedStudentGroup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(inputStudentGroupId))
                    .ReturnsAsync(storageStudentGroup);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentGroupAsync(expectedInputStudentGroup))
                    .ReturnsAsync(deletedStudentGroup);

            // when
            StudentGroup actualStudentGroup = await this.studentGroupService
                .RemoveStudentGroupByIdAsync(inputStudentGroupId);

            // then
            actualStudentGroup.Should().BeEquivalentTo(expectedStudentGroup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(inputStudentGroupId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentGroupAsync(expectedInputStudentGroup), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
