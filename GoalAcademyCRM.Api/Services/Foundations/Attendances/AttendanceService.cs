// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Brokers.DateTimes;
using GoalAcademyCRM.Api.Brokers.Loggings;
using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Models.Attendances;

namespace GoalAcademyCRM.Api.Services.Foundations.Attendances
{
    public partial class AttendanceService : IAttendanceService
    {

        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AttendanceService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Attendance> AddAttendanceAsync(Attendance attendance) =>
        TryCatch(async () =>
        {
            ValidateAttendanceOnAdd(attendance);

            return await this.storageBroker.InsertAttendanceAsync(attendance);
        });

        public IQueryable<Attendance> RetrieveAllAttendances() =>
            TryCatch(() => this.storageBroker.SelectAllAttendances());

        public ValueTask<Attendance> RetrieveAttendanceByIdAsync(Guid attendanceId) =>
           TryCatch(async () =>
           {
               ValidateAttendanceId(attendanceId);

               Attendance maybeAttendance =
                   await storageBroker.SelectAttendanceByIdAsync(attendanceId);

               ValidateStorageAttendance(maybeAttendance, attendanceId);

               return maybeAttendance;
           });

        public ValueTask<Attendance> ModifyAttendanceAsync(Attendance attendance) =>
            TryCatch(async () =>
            {
                ValidateAttendanceOnModify(attendance);

                Attendance maybeAttendance =
                    await this.storageBroker.SelectAttendanceByIdAsync(attendance.Id);

                ValidateAgainstStorageAttendanceOnModify(inputAttendance: attendance, storageAttendance: maybeAttendance);

                return await this.storageBroker.UpdateAttendanceAsync(attendance);
            });

        public ValueTask<Attendance> RemoveAttendanceByIdAsync(Guid attendanceId) =>
           TryCatch(async () =>
           {
               ValidateAttendanceId(attendanceId);

               Attendance maybeAttendance =
                   await this.storageBroker.SelectAttendanceByIdAsync(attendanceId);

               ValidateStorageAttendance(maybeAttendance, attendanceId);

               return await this.storageBroker.DeleteAttendanceAsync(maybeAttendance);
           });
    }
}