// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Models.Attendances.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance();
            Attendance someAttendance = randomAttendance;
            Guid attendanceId = someAttendance.Id;
            SqlException sqlException = CreateSqlException();

            var failedAttendanceStorageException =
                new FailedAttendanceStorageException(sqlException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(someAttendance);

            AttendanceDependencyException actualAttendanceDependencyException =
                await Assert.ThrowsAsync<AttendanceDependencyException>(
                    modifyAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should().BeEquivalentTo(
                expectedAttendanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(attendanceId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAttendanceAsync(someAttendance), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDatetimeOffset();
            Attendance randomAttendance = CreateRandomAttendance(randomDateTime);
            Attendance someAttendance = randomAttendance;
            someAttendance.CreatedDate = someAttendance.CreatedDate.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedStorageAttendanceException =
                new FailedAttendanceStorageException(databaseUpdateException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedStorageAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(someAttendance);

            AttendanceDependencyException actualAttendanceDependencyException =
                await Assert.ThrowsAsync<AttendanceDependencyException>(
                    modifyAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should().BeEquivalentTo(expectedAttendanceDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(randomDateTime);
            Attendance someAttendance = randomAttendance;
            someAttendance.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttendanceException =
                new LockedAttendanceException(databaseUpdateConcurrencyException);

            var expectedAttendanceDependencyValidationException =
                new AttendanceDependencyValidationException(lockedAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(someAttendance);

            AttendanceDependencyValidationException actualAttendanceDependencyValidationException =
                await Assert.ThrowsAsync<AttendanceDependencyValidationException>(modifyAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyValidationException.Should()
                .BeEquivalentTo(expectedAttendanceDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            var randomDateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(randomDateTime);
            Attendance someAttendance = randomAttendance;
            someAttendance.CreatedDate = someAttendance.CreatedDate.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedAttendanceServiceException =
                new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceException =
                new AttendanceServiceException(failedAttendanceServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Attendance> modifyAttendanceTask =
                this.attendanceService.ModifyAttendanceAsync(someAttendance);

            AttendanceServiceException actualAttendanceServiceException =
                await Assert.ThrowsAsync<AttendanceServiceException>(
                    modifyAttendanceTask.AsTask);

            // then
            actualAttendanceServiceException.Should().BeEquivalentTo(expectedAttendanceServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
