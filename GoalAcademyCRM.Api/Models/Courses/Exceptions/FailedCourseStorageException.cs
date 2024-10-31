// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class FailedCourseStorageException : Xeption
    {
        public FailedCourseStorageException(Exception innerException)
            : base(message: "Failed course storage error occurred, contact support.", innerException)
        { }
    }
}