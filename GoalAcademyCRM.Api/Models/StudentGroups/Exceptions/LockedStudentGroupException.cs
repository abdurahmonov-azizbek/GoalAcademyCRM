// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class LockedStudentGroupException : Xeption
    {
        public LockedStudentGroupException(Exception innerException)
            : base(message: "StudentGroup is locked, please try again.", innerException)
        { }
    }
}
