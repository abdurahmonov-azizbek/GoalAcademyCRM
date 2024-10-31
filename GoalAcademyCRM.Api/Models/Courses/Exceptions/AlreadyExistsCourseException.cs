// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class AlreadyExistsCourseException : Xeption
    {
        public AlreadyExistsCourseException(Exception innerException)
            : base(message: "Course already exists.", innerException)
        { }
    }
}
