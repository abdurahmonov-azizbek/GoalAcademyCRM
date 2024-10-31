// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Groups.Exceptions
{
    public class NullGroupException : Xeption
    {
        public NullGroupException()
            : base(message: "Group is null.")
        { }
    }
}

