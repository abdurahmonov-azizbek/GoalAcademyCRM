// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Models.Attendances.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Attendance someAttendance = CreateRandomAttendance();
            Guid attendanceId = someAttendance.Id;
            SqlException sqlException = CreateSqlException();

            FailedAttendanceStorageException failedAttendanceStorageException =
                new FailedAttendanceStorageException(sqlException);

            AttendanceDependencyException expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Attendance> addAttendanceTask = this.attendanceService
                .AddAttendanceAsync(someAttendance);

            AttendanceDependencyException actualAttendanceDependencyException =
                await Assert.ThrowsAsync<AttendanceDependencyException>(addAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should().BeEquivalentTo(expectedAttendanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedAttendanceDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogItAsync()
        {
            // given
            Attendance someAttendance = CreateRandomAttendance();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsAttendanceException =
                new AlreadyExistsAttendanceException(duplicateKeyException);

            var expectedAttendanceDependencyValidationException =
                new AttendanceDependencyValidationException(alreadyExistsAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<Attendance> addAttendanceTask = this.attendanceService.AddAttendanceAsync(someAttendance);

            AttendanceDependencyValidationException actualAttendanceDependencyValidationException =
                await Assert.ThrowsAsync<AttendanceDependencyValidationException>(
                    addAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyValidationException.Should().BeEquivalentTo(
                expectedAttendanceDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            Attendance someAttendance = CreateRandomAttendance();
            var dbUpdateException = new DbUpdateException();

            var failedAttendanceStorageException = new FailedAttendanceStorageException(dbUpdateException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<Attendance> addAttendanceTask = this.attendanceService.AddAttendanceAsync(someAttendance);

            AttendanceDependencyException actualAttendanceDependencyException =
                 await Assert.ThrowsAsync<AttendanceDependencyException>(addAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should()
                .BeEquivalentTo(expectedAttendanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedAttendanceDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateAttendanceAsync(It.IsAny<Attendance>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateExceptionOccuredAndLogItAsync()
        {
            // given
            Attendance someAttendance = CreateRandomAttendance();
            string someMessage = GetRandomString();

            var dbUpdateException =
                new DbUpdateException(someMessage);

            var failedAttendanceStorageException =
                new FailedAttendanceStorageException(dbUpdateException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Throws(dbUpdateException);

            // when
            ValueTask<Attendance> addAttendanceTask =
                this.attendanceService.AddAttendanceAsync(someAttendance);

            AttendanceDependencyException actualAttendanceDependencyException =
                await Assert.ThrowsAsync<AttendanceDependencyException>(addAttendanceTask.AsTask);

            // then
            actualAttendanceDependencyException.Should().BeEquivalentTo(expectedAttendanceDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogItAsync()
        {
            //given
            Attendance someAttendance = CreateRandomAttendance();
            var serviceException = new Exception();
            var failedAttendanceException = new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceExceptions =
                new AttendanceServiceException(failedAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(serviceException);

            //when
            ValueTask<Attendance> addAttendanceTask = this.attendanceService.AddAttendanceAsync(someAttendance);

            AttendanceServiceException actualAttendanceServiceException =
                await Assert.ThrowsAsync<AttendanceServiceException>(addAttendanceTask.AsTask);

            //then
            actualAttendanceServiceException.Should().BeEquivalentTo(
                expectedAttendanceServiceExceptions);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceServiceExceptions))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}