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
        public async Task ShouldRetrieveStudentGroupByIdAsync()
        {
            //given
            Guid randomStudentGroupId = Guid.NewGuid();
            Guid inputStudentGroupId = randomStudentGroupId;
            StudentGroup randomStudentGroup = CreateRandomStudentGroup();
            StudentGroup storageStudentGroup = randomStudentGroup;
            StudentGroup excpectedStudentGroup = randomStudentGroup.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGroupByIdAsync(inputStudentGroupId)).ReturnsAsync(storageStudentGroup);

            //when
            StudentGroup actuallStudentGroup = await this.studentGroupService.RetrieveStudentGroupByIdAsync(inputStudentGroupId);

            //then
            actuallStudentGroup.Should().BeEquivalentTo(excpectedStudentGroup);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(inputStudentGroupId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}