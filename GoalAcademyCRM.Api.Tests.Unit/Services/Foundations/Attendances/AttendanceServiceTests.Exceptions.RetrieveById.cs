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
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            SqlException sqlException = CreateSqlException();

            var failedAttendanceStorageException =
                new FailedAttendanceStorageException(sqlException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedAttendanceStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Attendance> retrieveAttendanceByIdTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(someId);

            AttendanceDependencyException actualAttendanceDependencyexception =
                await Assert.ThrowsAsync<AttendanceDependencyException>(
                    retrieveAttendanceByIdTask.AsTask);

            //then
            actualAttendanceDependencyexception.Should().BeEquivalentTo(
                expectedAttendanceDependencyException);

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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdAsyncIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedAttendanceServiceException =
                new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceException =
                new AttendanceServiceException(failedAttendanceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAttendanceByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

            //when
            ValueTask<Attendance> retrieveAttendanceByIdTask =
                this.attendanceService.RetrieveAttendanceByIdAsync(someId);

            AttendanceServiceException actualAttendanceServiceException =
                await Assert.ThrowsAsync<AttendanceServiceException>(retrieveAttendanceByIdTask.AsTask);

            //then
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