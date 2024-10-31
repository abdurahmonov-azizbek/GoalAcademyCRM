// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class StudentGroupDependencyException : Xeption
    {
        public StudentGroupDependencyException(Exception innerException)
            : base(message: "StudentGroup dependency error occured, contact support.", innerException)
        { }
    }
}