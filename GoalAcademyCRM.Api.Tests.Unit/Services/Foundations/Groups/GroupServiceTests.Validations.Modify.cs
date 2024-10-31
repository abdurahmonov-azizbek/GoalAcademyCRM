// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupIsNullAndLogItAsync()
        {
            // given
            Group nullGroup = null;
            var nullGroupException = new NullGroupException();

            var expectedGroupValidationException =
                new GroupValidationException(nullGroupException);

            // when
            ValueTask<Group> modifyGroupTask = this.groupService.ModifyGroupAsync(nullGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupIsInvalidAndLogItAsync(string invalidString)
        {
            // given
            Group invalidGroup = new Group
            {
                Name = invalidString
            };

            var invalidGroupException = new InvalidGroupException();

				invalidGroupException.AddData(
					key: nameof(Group.Id),
					values: "Id is required");

				invalidGroupException.AddData(
					key: nameof(Group.Name),
					values: "Text is required");

				invalidGroupException.AddData(
					key: nameof(Group.CourseId),
					values: "Id is required");

				invalidGroupException.AddData(
					key: nameof(Group.TeacherId),
					values: "Id is required");



            invalidGroupException.AddData(
                key: nameof(Group.CreatedDate),
                values: "Date is required");

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: new[]
                    {
                        "Date is required",
                        "Date is not recent",
                        $"Date is the same as {nameof(Group.CreatedDate)}"
                    }
                );

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTime);

            // when
            ValueTask<Group> modifyGroupTask = this.groupService.ModifyGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomGroup(randomDateTime);
            Group invalidGroup = randomGroup;
            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: $"Date is the same as {nameof(Group.CreatedDate)}");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                 await Assert.ThrowsAsync<GroupValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(invalidGroup.Id), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidSeconds))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomGroup(dateTime);
            Group inputGroup = randomGroup;
            inputGroup.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: "Date is not recent");

            var expectedGroupValidatonException =
                new GroupValidationException(invalidGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(inputGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidatonException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfGroupDoesNotExistAndLogItAsync()
        {
            // given
            int negativeMinutes = GetRandomNegativeNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomGroup(dateTime);
            Group nonExistGroup = randomGroup;
            nonExistGroup.CreatedDate = dateTime.AddMinutes(negativeMinutes);
            Group nullGroup = null;

            var notFoundGroupException = new NotFoundGroupException(nonExistGroup.Id);

            var expectedGroupValidationException =
                new GroupValidationException(notFoundGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(nonExistGroup.Id))
                    .ReturnsAsync(nullGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(dateTime);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(nonExistGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(
                    modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(nonExistGroup.Id), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomModifyGroup(randomDateTime);
            Group invalidGroup = randomGroup.DeepClone();
            Group storageGroup = invalidGroup.DeepClone();
            storageGroup.CreatedDate = storageGroup.CreatedDate.AddMinutes(randomMinutes);
            storageGroup.UpdatedDate = storageGroup.UpdatedDate.AddMinutes(randomMinutes);
            var invalidGroupException = new InvalidGroupException();
            Guid groupId = invalidGroup.Id;

            invalidGroupException.AddData(
                key: nameof(Group.CreatedDate),
                values: $"Date is not same as {nameof(Group.CreatedDate)}");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(groupId)).ReturnsAsync(storageGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(invalidGroup.Id), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Group randomGroup = CreateRandomModifyGroup(randomDateTime);
            Group invalidGroup = randomGroup;
            Group storageGroup = randomGroup.DeepClone();
            invalidGroup.UpdatedDate = storageGroup.UpdatedDate;
            Guid groupId = invalidGroup.Id;
            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.UpdatedDate),
                values: $"Date is the same as {nameof(Group.UpdatedDate)}");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectGroupByIdAsync(invalidGroup.Id)).ReturnsAsync(storageGroup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDateTime);

            // when
            ValueTask<Group> modifyGroupTask =
                this.groupService.ModifyGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(modifyGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectGroupByIdAsync(groupId), Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
