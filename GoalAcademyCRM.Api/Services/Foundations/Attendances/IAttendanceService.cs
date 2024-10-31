// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Attendances;

namespace GoalAcademyCRM.Api.Services.Foundations.Attendances
{
    public interface IAttendanceService  
    {
        /// <exception cref="Models.Attendances.Exceptions.AttendanceValidationException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyValidationException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceServiceException"></exception>
        ValueTask<Attendance> AddAttendanceAsync(Attendance attendance);

        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceServiceException"></exception>
        IQueryable<Attendance> RetrieveAllAttendances();

        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceServiceException"></exception>
        ValueTask<Attendance> RetrieveAttendanceByIdAsync(Guid attendanceId);

        /// <exception cref="Models.Attendances.Exceptions.AttendanceValidationException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyValidationException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceServiceException"></exception>
        ValueTask<Attendance> ModifyAttendanceAsync(Attendance attendance);

        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyValidationException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceDependencyException"></exception>
        /// <exception cref="Models.Attendances.Exceptions.AttendanceServiceException"></exception>
        ValueTask<Attendance> RemoveAttendanceByIdAsync(Guid attendanceId);
    }
}