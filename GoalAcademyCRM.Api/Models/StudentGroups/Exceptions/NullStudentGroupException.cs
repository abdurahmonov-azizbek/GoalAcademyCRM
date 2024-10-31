// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class NullStudentGroupException : Xeption
    {
        public NullStudentGroupException()
            : base(message: "StudentGroup is null.")
        { }
    }
}

