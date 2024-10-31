// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class NullAttendanceException : Xeption
    {
        public NullAttendanceException()
            : base(message: "Attendance is null.")
        { }
    }
}

