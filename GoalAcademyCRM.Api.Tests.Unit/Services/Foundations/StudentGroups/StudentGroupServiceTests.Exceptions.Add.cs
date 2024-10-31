// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            StudentGroup someStudentGroup = CreateRandomStudentGroup();
            Guid studentGroupId = someStudentGroup.Id;
            SqlException sqlException = CreateSqlException();

            FailedStudentGroupStorageException failedStudentGroupStorageException =
                new FailedStudentGroupStorageException(sqlException);

            StudentGroupDependencyException expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<StudentGroup> addStudentGroupTask = this.studentGroupService
                .AddStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(addStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(expectedStudentGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentGroupDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccurredAndLogItAsync()
        {
            // given
            StudentGroup someStudentGroup = CreateRandomStudentGroup();
            string someMessage = GetRandomString();
            var duplicateKeyException = new DuplicateKeyException(someMessage);

            var alreadyExistsStudentGroupException =
                new AlreadyExistsStudentGroupException(duplicateKeyException);

            var expectedStudentGroupDependencyValidationException =
                new StudentGroupDependencyValidationException(alreadyExistsStudentGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(duplicateKeyException);

            // when
            ValueTask<StudentGroup> addStudentGroupTask = this.studentGroupService.AddStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyValidationException actualStudentGroupDependencyValidationException =
                await Assert.ThrowsAsync<StudentGroupDependencyValidationException>(
                    addStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyValidationException.Should().BeEquivalentTo(
                expectedStudentGroupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyValidationException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateErrorOccursAndLogItAsync()
        {
            // given
            StudentGroup someStudentGroup = CreateRandomStudentGroup();
            var dbUpdateException = new DbUpdateException();

            var failedStudentGroupStorageException = new FailedStudentGroupStorageException(dbUpdateException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(dbUpdateException);

            // when
            ValueTask<StudentGroup> addStudentGroupTask = this.studentGroupService.AddStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                 await Assert.ThrowsAsync<StudentGroupDependencyException>(addStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should()
                .BeEquivalentTo(expectedStudentGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedStudentGroupDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentGroupAsync(It.IsAny<StudentGroup>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDbUpdateExceptionOccuredAndLogItAsync()
        {
            // given
            StudentGroup someStudentGroup = CreateRandomStudentGroup();
            string someMessage = GetRandomString();

            var dbUpdateException =
                new DbUpdateException(someMessage);

            var failedStudentGroupStorageException =
                new FailedStudentGroupStorageException(dbUpdateException);

            var expectedStudentGroupDependencyException =
                new StudentGroupDependencyException(failedStudentGroupStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                    broker.GetCurrentDateTimeOffset()).Throws(dbUpdateException);

            // when
            ValueTask<StudentGroup> addStudentGroupTask =
                this.studentGroupService.AddStudentGroupAsync(someStudentGroup);

            StudentGroupDependencyException actualStudentGroupDependencyException =
                await Assert.ThrowsAsync<StudentGroupDependencyException>(addStudentGroupTask.AsTask);

            // then
            actualStudentGroupDependencyException.Should().BeEquivalentTo(expectedStudentGroupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupDependencyException))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogItAsync()
        {
            //given
            StudentGroup someStudentGroup = CreateRandomStudentGroup();
            var serviceException = new Exception();
            var failedStudentGroupException = new FailedStudentGroupServiceException(serviceException);

            var expectedStudentGroupServiceExceptions =
                new StudentGroupServiceException(failedStudentGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Throws(serviceException);

            //when
            ValueTask<StudentGroup> addStudentGroupTask = this.studentGroupService.AddStudentGroupAsync(someStudentGroup);

            StudentGroupServiceException actualStudentGroupServiceException =
                await Assert.ThrowsAsync<StudentGroupServiceException>(addStudentGroupTask.AsTask);

            //then
            actualStudentGroupServiceException.Should().BeEquivalentTo(
                expectedStudentGroupServiceExceptions);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedStudentGroupServiceExceptions))), Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}