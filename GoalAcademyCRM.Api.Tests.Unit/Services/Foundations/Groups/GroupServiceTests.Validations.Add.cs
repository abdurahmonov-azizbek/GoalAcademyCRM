// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Groups
{
    public partial class GroupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInputIsNullAndLogItAsync()
        {
            // given
            Group nullGroup = null;
            var nullGroupException = new NullGroupException();

            var expectedGroupValidationException =
                new GroupValidationException(nullGroupException);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(nullGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(addGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedGroupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfJobIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            Group invalidGroup = new Group()
            {
                Name = invalidText
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
					values: "Date is required");



            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(addGroupTask.AsTask);

            // then
            actualGroupValidationException.Should()
                .BeEquivalentTo(expectedGroupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShoudlThrowValidationExceptionOnAddIfCreatedDateIsNotSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomMinutes = GetRandomNumber();
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            Group randomGroup = CreateRandomGroup(randomDate);
            Group invalidGroup = randomGroup;
            invalidGroup.UpdatedDate = randomDate.AddMinutes(randomMinutes);
            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.CreatedDate),
                values: $"Date is not same as {nameof(Group.UpdatedDate)}");

            var expectedGroupValidationException = new GroupValidationException(invalidGroupException);

            this.dateTimeBrokerMock.Setup(broker => broker.GetCurrentDateTimeOffset())
                .Returns(randomDate);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(addGroupTask.AsTask);

            // then
            actualGroupValidationException.Should().BeEquivalentTo(expectedGroupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinutes))]
        public async Task ShouldThrowValidationExceptionIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidMinutes)
        {
            // given
            DateTimeOffset randomDate = GetRandomDatetimeOffset();
            DateTimeOffset invalidDateTime = randomDate.AddMinutes(invalidMinutes);
            Group randomGroup = CreateRandomGroup(invalidDateTime);
            Group invalidGroup = randomGroup;
            var invalidGroupException = new InvalidGroupException();

            invalidGroupException.AddData(
                key: nameof(Group.CreatedDate),
                values: "Date is not recent");

            var expectedGroupValidationException =
                new GroupValidationException(invalidGroupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset()).Returns(randomDate);

            // when
            ValueTask<Group> addGroupTask = this.groupService.AddGroupAsync(invalidGroup);

            GroupValidationException actualGroupValidationException =
                await Assert.ThrowsAsync<GroupValidationException>(addGroupTask.AsTask);

            // then
            actualGroupValidationException.Should().
                BeEquivalentTo(expectedGroupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
            broker.GetCurrentDateTimeOffset(), Times.Once);

            this.loggingBrokerMock.Verify(broker => broker.LogError(It.Is(
                SameExceptionAs(expectedGroupValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertGroupAsync(It.IsAny<Group>()), Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}