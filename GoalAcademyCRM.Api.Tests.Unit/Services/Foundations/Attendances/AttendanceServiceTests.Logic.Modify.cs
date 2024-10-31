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
        public async Task ShouldModifyAttendanceAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomModifyAttendance(randomDate);
            Attendance inputAttendance = randomAttendance;
            Attendance storageAttendance = inputAttendance.DeepClone();
            storageAttendance.UpdatedDate = randomAttendance.CreatedDate;
            Attendance updatedAttendance = inputAttendance;
            Attendance expectedAttendance = updatedAttendance.DeepClone();
            Guid attendanceId = inputAttendance.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(attendanceId))
                    .ReturnsAsync(storageAttendance);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateAttendanceAsync(inputAttendance))
                    .ReturnsAsync(updatedAttendance);

            // when
            Attendance actualAttendance =
               await this.attendanceService.ModifyAttendanceAsync(inputAttendance);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(attendanceId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAttendanceAsync(inputAttendance), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
