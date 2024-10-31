// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
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
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedAttendanceStorageException(sqlException);

            var expectedAttendanceDependencyException =
                new AttendanceDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttendances()).Throws(sqlException);

            //when
            Action retrieveAllAttendancesAction = () =>
                this.attendanceService.RetrieveAllAttendances();

            AttendanceDependencyException actualAttendanceDependencyException =
                Assert.Throws<AttendanceDependencyException>(retrieveAllAttendancesAction);

            //then
            actualAttendanceDependencyException.Should().BeEquivalentTo(
                expectedAttendanceDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttendances(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedAttendanceDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedAttendanceServiceException =
                new FailedAttendanceServiceException(serviceException);

            var expectedAttendanceServiceException =
                new AttendanceServiceException(failedAttendanceServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllAttendances()).Throws(serviceException);

            // when
            Action retrieveAllAttendancesAction = () =>
                this.attendanceService.RetrieveAllAttendances();

            AttendanceServiceException actualAttendanceServiceException =
                Assert.Throws<AttendanceServiceException>(retrieveAllAttendancesAction);

            // then
            actualAttendanceServiceException.Should().BeEquivalentTo(expectedAttendanceServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllAttendances(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}