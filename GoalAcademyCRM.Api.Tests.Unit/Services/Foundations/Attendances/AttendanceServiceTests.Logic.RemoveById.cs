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
        public async Task ShouldRemoveAttendanceByIdAsync()
        {
            // given
            Guid randomAttendanceId = Guid.NewGuid();
            Guid inputAttendanceId = randomAttendanceId;
            Attendance randomAttendance = CreateRandomAttendance();
            Attendance storageAttendance = randomAttendance;
            Attendance expectedInputAttendance = storageAttendance;
            Attendance deletedAttendance = expectedInputAttendance;
            Attendance expectedAttendance = deletedAttendance.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId))
                    .ReturnsAsync(storageAttendance);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteAttendanceAsync(expectedInputAttendance))
                    .ReturnsAsync(deletedAttendance);

            // when
            Attendance actualAttendance = await this.attendanceService
                .RemoveAttendanceByIdAsync(inputAttendanceId);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(inputAttendanceId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttendanceAsync(expectedInputAttendance), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
