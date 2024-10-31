// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Attendances.Exceptions
{
    public class AttendanceDependencyValidationException : Xeption
    {
        public AttendanceDependencyValidationException(Xeption innerException)
            : base(message: "Attendance dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
