// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class StudentGroupServiceException : Xeption
    {
        public StudentGroupServiceException(Exception innerException)
            : base(message: "StudentGroup service error occured, contact support.", innerException)
        { }
    }
}