// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System.Linq;
using GoalAcademyCRM.Api.Models.Attendances;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllAttendances()
        {
            //given
            IQueryable<Attendance> randomAttendances = CreateRandomAttendances();
            IQueryable<Attendance> storageAttendances = randomAttendances;
            IQueryable<Attendance> expectedAttendances = storageAttendances.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttendances()).Returns(storageAttendances);

            //when
            IQueryable<Attendance> actualAttendances =
                this.attendanceService.RetrieveAllAttendances();

            //then
            actualAttendances.Should().BeEquivalentTo(expectedAttendances);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttendances(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
