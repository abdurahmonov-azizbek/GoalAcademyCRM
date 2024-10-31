// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class LockedCourseException : Xeption
    {
        public LockedCourseException(Exception innerException)
            : base(message: "Course is locked, please try again.", innerException)
        { }
    }
}
