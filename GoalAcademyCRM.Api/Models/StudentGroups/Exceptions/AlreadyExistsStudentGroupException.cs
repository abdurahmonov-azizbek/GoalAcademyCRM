// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class AlreadyExistsStudentGroupException : Xeption
    {
        public AlreadyExistsStudentGroupException(Exception innerException)
            : base(message: "StudentGroup already exists.", innerException)
        { }
    }
}
