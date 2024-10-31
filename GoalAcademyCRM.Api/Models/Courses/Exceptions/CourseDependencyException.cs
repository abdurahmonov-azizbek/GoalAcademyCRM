// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class CourseDependencyException : Xeption
    {
        public CourseDependencyException(Exception innerException)
            : base(message: "Course dependency error occured, contact support.", innerException)
        { }
    }
}