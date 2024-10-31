// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Courses.Exceptions
{
    public class NotFoundCourseException : Xeption
    {
        public NotFoundCourseException(Guid courseId)
            : base(message: $"Couldn't find course with id: {courseId}.")
        { }
    }
}
