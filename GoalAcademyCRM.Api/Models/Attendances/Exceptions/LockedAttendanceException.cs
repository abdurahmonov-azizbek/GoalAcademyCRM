// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class LockedAttendanceException : Xeption
    {
        public LockedAttendanceException(Exception innerException)
            : base(message: "Attendance is locked, please try again.", innerException)
        { }
    }
}
