// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;
using Microsoft.EntityFrameworkCore;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Attendance> Attendances { get; set; }

        public async ValueTask<Attendance> InsertAttendanceAsync(Attendance attendance) =>
            await InsertAsync(attendance);

        public IQueryable<Attendance> SelectAllAttendances() =>
            SelectAll<Attendance>();

        public async ValueTask<Attendance> SelectAttendanceByIdAsync(Guid attendanceId) =>
            await SelectAsync<Attendance>(attendanceId);

        public async ValueTask<Attendance> DeleteAttendanceAsync(Attendance attendance) =>
            await DeleteAsync(attendance);

        public async ValueTask<Attendance> UpdateAttendanceAsync(Attendance attendance) =>
            await UpdateAsync(attendance);
    }
}