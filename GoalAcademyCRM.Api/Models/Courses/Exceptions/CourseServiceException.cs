// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class CourseServiceException : Xeption
    {
        public CourseServiceException(Exception innerException)
            : base(message: "Course service error occured, contact support.", innerException)
        { }
    }
}