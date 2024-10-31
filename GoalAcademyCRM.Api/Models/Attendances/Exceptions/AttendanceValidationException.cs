// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class AttendanceValidationException : Xeption
    {
        public AttendanceValidationException(Xeption innerException)
            : base(message: "Attendance validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
