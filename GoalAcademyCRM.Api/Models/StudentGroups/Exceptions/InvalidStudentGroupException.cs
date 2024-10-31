// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.StudentGroups.Exceptions
{
    public class InvalidStudentGroupException : Xeption
    {
        public InvalidStudentGroupException()
            : base(message: "StudentGroup is invalid.")
        { }
    }
}
