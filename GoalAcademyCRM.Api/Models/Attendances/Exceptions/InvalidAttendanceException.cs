// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class InvalidAttendanceException : Xeption
    {
        public InvalidAttendanceException()
            : base(message: "Attendance is invalid.")
        { }
    }
}
