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
        public async Task ShouldAddAttendanceAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Attendance randomAttendance = CreateRandomAttendance(randomDate);
            Attendance inputAttendance = randomAttendance;
            Attendance persistedAttendance = inputAttendance;
            Attendance expectedAttendance = persistedAttendance.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttendanceAsync(inputAttendance)).ReturnsAsync(persistedAttendance);

            // when
            Attendance actualAttendance = await this.attendanceService
                .AddAttendanceAsync(inputAttendance);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(inputAttendance), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}