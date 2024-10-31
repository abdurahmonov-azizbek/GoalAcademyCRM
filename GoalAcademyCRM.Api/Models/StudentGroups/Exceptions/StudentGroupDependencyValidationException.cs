// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class StudentGroupDependencyValidationException : Xeption
    {
        public StudentGroupDependencyValidationException(Xeption innerException)
            : base(message: "StudentGroup dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
