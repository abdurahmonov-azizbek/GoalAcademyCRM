// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class AlreadyExistsAttendanceException : Xeption
    {
        public AlreadyExistsAttendanceException(Exception innerException)
            : base(message: "Attendance already exists.", innerException)
        { }
    }
}
