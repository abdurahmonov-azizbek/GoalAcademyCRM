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
        public async Task ShouldAddStudentGroupAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            StudentGroup randomStudentGroup = CreateRandomStudentGroup(randomDate);
            StudentGroup inputStudentGroup = randomStudentGroup;
            StudentGroup persistedStudentGroup = inputStudentGroup;
            StudentGroup expectedStudentGroup = persistedStudentGroup.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentGroupAsync(inputStudentGroup)).ReturnsAsync(persistedStudentGroup);

            // when
            StudentGroup actualStudentGroup = await this.studentGroupService
                .AddStudentGroupAsync(inputStudentGroup);

            // then
            actualStudentGroup.Should().BeEquivalentTo(expectedStudentGroup);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGroupAsync(inputStudentGroup), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}