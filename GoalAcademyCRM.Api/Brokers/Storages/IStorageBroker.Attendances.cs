// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Attendance> InsertAttendanceAsync(Attendance attendance);
        IQueryable<Attendance> SelectAllAttendances();
        ValueTask<Attendance> SelectAttendanceByIdAsync(Guid attendanceId);
        ValueTask<Attendance> DeleteAttendanceAsync(Attendance attendance);
        ValueTask<Attendance> UpdateAttendanceAsync(Attendance attendance);
    }
}