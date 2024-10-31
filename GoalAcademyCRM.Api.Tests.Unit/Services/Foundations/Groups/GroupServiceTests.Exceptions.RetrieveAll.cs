// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            //given
            SqlException sqlException = CreateSqlException();

            var failedStorageException =
                new FailedGroupStorageException(sqlException);

            var expectedGroupDependencyException =
                new GroupDependencyException(failedStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups()).Throws(sqlException);

            //when
            Action retrieveAllGroupsAction = () =>
                this.groupService.RetrieveAllGroups();

            GroupDependencyException actualGroupDependencyException =
                Assert.Throws<GroupDependencyException>(retrieveAllGroupsAction);

            //then
            actualGroupDependencyException.Should().BeEquivalentTo(
                expectedGroupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedGroupDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogItAsync()
        {
            // given
            string exceptionMessage = GetRandomString();
            var serviceException = new Exception(exceptionMessage);

            var failedGroupServiceException =
                new FailedGroupServiceException(serviceException);

            var expectedGroupServiceException =
                new GroupServiceException(failedGroupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllGroups()).Throws(serviceException);

            // when
            Action retrieveAllGroupsAction = () =>
                this.groupService.RetrieveAllGroups();

            GroupServiceException actualGroupServiceException =
                Assert.Throws<GroupServiceException>(retrieveAllGroupsAction);

            // then
            actualGroupServiceException.Should().BeEquivalentTo(expectedGroupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllGroups(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}