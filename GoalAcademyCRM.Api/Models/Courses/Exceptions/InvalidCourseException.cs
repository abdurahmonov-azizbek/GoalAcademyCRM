// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class InvalidCourseException : Xeption
    {
        public InvalidCourseException()
            : base(message: "Course is invalid.")
        { }
    }
}
