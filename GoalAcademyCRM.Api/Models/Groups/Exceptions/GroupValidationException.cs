// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class GroupValidationException : Xeption
    {
        public GroupValidationException(Xeption innerException)
            : base(message: "Group validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
