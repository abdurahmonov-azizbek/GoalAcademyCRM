// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class CourseValidationException : Xeption
    {
        public CourseValidationException(Xeption innerException)
            : base(message: "Course validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
