// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class CourseDependencyValidationException : Xeption
    {
        public CourseDependencyValidationException(Xeption innerException)
            : base(message: "Course dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
