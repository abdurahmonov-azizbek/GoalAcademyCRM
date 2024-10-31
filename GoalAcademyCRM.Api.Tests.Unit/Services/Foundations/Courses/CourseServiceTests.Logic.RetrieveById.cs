// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Courses
{
    public partial class CourseServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveCourseByIdAsync()
        {
            //given
            Guid randomCourseId = Guid.NewGuid();
            Guid inputCourseId = randomCourseId;
            Course randomCourse = CreateRandomCourse();
            Course storageCourse = randomCourse;
            Course excpectedCourse = randomCourse.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId)).ReturnsAsync(storageCourse);

            //when
            Course actuallCourse = await this.courseService.RetrieveCourseByIdAsync(inputCourseId);

            //then
            actuallCourse.Should().BeEquivalentTo(excpectedCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}