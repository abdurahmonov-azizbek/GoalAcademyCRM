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
        public async Task ShouldModifyStudentGroupAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            StudentGroup randomStudentGroup = CreateRandomModifyStudentGroup(randomDate);
            StudentGroup inputStudentGroup = randomStudentGroup;
            StudentGroup storageStudentGroup = inputStudentGroup.DeepClone();
            storageStudentGroup.UpdatedDate = randomStudentGroup.CreatedDate;
            StudentGroup updatedStudentGroup = inputStudentGroup;
            StudentGroup expectedStudentGroup = updatedStudentGroup.DeepClone();
            Guid studentGroupId = inputStudentGroup.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(studentGroupId))
                    .ReturnsAsync(storageStudentGroup);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentGroupAsync(inputStudentGroup))
                    .ReturnsAsync(updatedStudentGroup);

            // when
            StudentGroup actualStudentGroup =
               await this.studentGroupService.ModifyStudentGroupAsync(inputStudentGroup);

            // then
            actualStudentGroup.Should().BeEquivalentTo(expectedStudentGroup);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(studentGroupId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentGroupAsync(inputStudentGroup), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
