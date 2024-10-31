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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someAttendanceId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedAttendanceException =
                new LockedAttendanceException(databaseUpdateConcurrencyException);

            var expectedAttendanceDependencyValidationException =
                new AttendanceDependencyValidationException(lockedAttendanceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Attendance> removeAttendanceByIdTask =
               this.attendanceService.RemoveAttendanceByIdAsync(someAttendanceId);

            AttendanceDependencyValidationException actualAttendanceDependencyValidationException =
                await Assert.ThrowsAsync<AttendanceDependencyValidationException>(
                    removeAttendanceByIdTask.AsTask);

            // then
            actualAttendanceDependencyValidationException.Should().BeEquivalentTo(
               expectedAttendanceDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteAttendanceAsync(It.IsAny<Attendance>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedAttendanceStorageException =
                new FailedAttendanceStorageException(sqlException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(someId))
                    .Throws(sqlException);
            // when
            ValueTask<Attendance> removeAttendanceTask =
                this.attendanceService.RemoveAttendanceByIdAsync(someId);

            AttendanceDependencyException actualAttendanceDependencyException =
                await Assert.ThrowsAsync<AttendanceDependencyException>(
                    removeAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should().BeEquivalentTo(expectedAttendanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid someAttendanceId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAttendanceServiceException =
                new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceException =
                new AttendanceServiceException(failedAttendanceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(someAttendanceId))
                    .Throws(serviceException);

            // when
            ValueTask<Attendance> removeAttendanceByIdTask =
                this.attendanceService.RemoveAttendanceByIdAsync(someAttendanceId);

            AttendanceServiceException actualAttendanceServiceException =
                await Assert.ThrowsAsync<AttendanceServiceException>(
                    removeAttendanceByIdTask.AsTask);

            // then
            actualAttendanceServiceException.Should().BeEquivalentTo(expectedAttendanceServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}