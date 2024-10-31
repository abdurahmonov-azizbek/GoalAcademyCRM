// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class FailedCourseServiceException : Xeption
    {
        public FailedCourseServiceException(Exception innerException)
            : base(message: "Failed course service error occurred, please contact support.", innerException)
        { }
    }
}