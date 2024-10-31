// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.StudentGroups
{
    public partial class StudentGroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTime();
            StudentGroup randomStudentGroup = CreateRandomStudentGroup();
            StudentGroup someStudentGroup = randomStudentGroup;
            Guid studentGroupId = someStudentGroup.Id;
            SqlException sqlException = CreateSqlException();

            var failedStudentGroupStorageException =
                new FailedStudentGroupStorageException(sqlException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(sqlException);

            // when
            ValueTask<StudentGroup> modifyStudentGroupTask =
                this.studentGroupService.ModifyStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(
                    modifyStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(
                expectedStudentGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGroupByIdAsync(studentGroupId), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentGroupAsync(someStudentGroup), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyException))), Times.Once);

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
            StudentGroup randomStudentGroup = CreateRandomStudentGroup(randomDateTime);
            StudentGroup someStudentGroup = randomStudentGroup;
            someStudentGroup.CreatedDate = someStudentGroup.CreatedDate.AddMinutes(minutesInPast);
            var databaseUpdateException = new DbUpdateException();

            var failedStorageStudentGroupException =
                new FailedStudentGroupStorageException(databaseUpdateException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStorageStudentGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<StudentGroup> modifyStudentGroupTask =
                this.studentGroupService.ModifyStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(
                    modifyStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(expectedStudentGroupDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyException))), Times.Once);

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
            StudentGroup randomStudentGroup = CreateRandomStudentGroup(randomDateTime);
            StudentGroup someStudentGroup = randomStudentGroup;
            someStudentGroup.CreatedDate = randomDateTime.AddMinutes(minutesInPast);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedStudentGroupException =
                new LockedStudentGroupException(databaseUpdateConcurrencyException);

            var expectedStudentGroupDependencyValidationException =
                new StudentGroupDependencyValidationException(lockedStudentGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<StudentGroup> modifyStudentGroupTask =
                this.studentGroupService.ModifyStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyValidationException actualStudentGroupDependencyValidationException =
                await Assert.ThrowsAsync<StudentGroupDependencyValidationException>(modifyStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyValidationException.Should()
                .BeEquivalentTo(expectedStudentGroupDependencyValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyValidationException))), Times.Once);

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
            StudentGroup randomStudentGroup = CreateRandomStudentGroup(randomDateTime);
            StudentGroup someStudentGroup = randomStudentGroup;
            someStudentGroup.CreatedDate = someStudentGroup.CreatedDate.AddMinutes(minutesInPast);
            var serviceException = new Exception();

            var failedStudentGroupServiceException =
                new FailedStudentGroupServiceException(serviceException);

            var expectedStudentGroupServiceException =
                new StudentGroupServiceException(failedStudentGroupServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<StudentGroup> modifyStudentGroupTask =
                this.studentGroupService.ModifyStudentGroupAsync(someStudentGroup);

            StudentGroupServiceException actualStudentGroupServiceException =
                await Assert.ThrowsAsync<StudentGroupServiceException>(
                    modifyStudentGroupTask.AsTask);

            // then
            actualStudentGroupServiceException.Should().BeEquivalentTo(expectedStudentGroupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
