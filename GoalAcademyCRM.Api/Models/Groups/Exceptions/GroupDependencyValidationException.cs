// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class GroupDependencyValidationException : Xeption
    {
        public GroupDependencyValidationException(Xeption innerException)
            : base(message: "Group dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
