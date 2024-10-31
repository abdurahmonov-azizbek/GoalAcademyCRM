// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
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
        public async Task ShouldRetrieveAttendanceByIdAsync()
        {
            //given
            Guid randomAttendanceId = Guid.NewGuid();
            Guid inputAttendanceId = randomAttendanceId;
            Attendance randomAttendance = CreateRandomAttendance();
            Attendance storageAttendance = randomAttendance;
            Attendance excpectedAttendance = randomAttendance.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId)).ReturnsAsync(storageAttendance);

            //when
            Attendance actuallAttendance = await this.attendanceService.RetrieveAttendanceByIdAsync(inputAttendanceId);

            //then
            actuallAttendance.Should().BeEquivalentTo(excpectedAttendance);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}