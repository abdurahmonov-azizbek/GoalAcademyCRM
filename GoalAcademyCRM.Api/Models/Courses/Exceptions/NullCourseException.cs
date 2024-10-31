// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class NullCourseException : Xeption
    {
        public NullCourseException()
            : base(message: "Course is null.")
        { }
    }
}

