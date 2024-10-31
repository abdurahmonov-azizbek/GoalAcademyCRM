// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class NotFoundAttendanceException : Xeption
    {
        public NotFoundAttendanceException(Guid attendanceId)
            : base(message: $"Couldn't find attendance with id: {attendanceId}.")
        { }
    }
}
