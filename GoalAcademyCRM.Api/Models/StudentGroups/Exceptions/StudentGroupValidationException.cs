// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class StudentGroupValidationException : Xeption
    {
        public StudentGroupValidationException(Xeption innerException)
            : base(message: "StudentGroup validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
