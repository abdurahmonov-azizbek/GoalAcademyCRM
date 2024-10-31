// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class NotFoundStudentGroupException : Xeption
    {
        public NotFoundStudentGroupException(Guid studentGroupId)
            : base(message: $"Couldn't find studentGroup with id: {studentGroupId}.")
        { }
    }
}
