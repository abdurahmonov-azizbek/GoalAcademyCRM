// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using GoalAcademyCRM.Api.Brokers.DateTimes;
using GoalAcademyCRM.Api.Brokers.Loggings;
using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Services.Foundations.Attendances;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace GoalAcademyCRM.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAttendanceService attendanceService;

        public AttendanceServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.attendanceService = new AttendanceService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        public static TheoryData<int> InvalidMinutes()
        {
            int minutesInFuture = GetRandomNumber();
            int minutesInPast = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                minutesInFuture,
                minutesInPast
            };
        }

        public static TheoryData<int> InvalidSeconds()
        {
            int secondsInPast = -1 * new IntRange(
                min: 60,
                max: short.MaxValue).GetValue();

            int secondsInFuture = new IntRange(
                min: 0,
                max: short.MaxValue).GetValue();

            return new TheoryData<int>
            {
                secondsInPast,
                secondsInFuture
            };
        }

        private IQueryable<Attendance> CreateRandomAttendances()
        {
            return CreateAttendanceFiller(dates: GetRandomDatetimeOffset())
                .Create(count: GetRandomNumber()).AsQueryable();
        }

        private string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private DateTimeOffset GetRandomDatetimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static SqlException CreateSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static Attendance CreateRandomModifyAttendance(DateTimeOffset dates)
        {
            int randomDaysInPast = GetRandomNegativeNumber();
            Attendance randomAttendance = CreateRandomAttendance(dates);

            randomAttendance.CreatedDate = randomAttendance.CreatedDate.AddDays(randomDaysInPast);

            return randomAttendance;
        }

        private static Attendance CreateRandomAttendance(DateTimeOffset dates) =>
           CreateAttendanceFiller(dates).Create();

        private static Attendance CreateRandomAttendance() =>
            CreateAttendanceFiller(GetRandomDateTime()).Create();

        private static Filler<Attendance> CreateAttendanceFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Attendance>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}
