// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class NullPaymentException : Xeption
    {
        public NullPaymentException()
            : base(message: "Payment is null.")
        { }
    }
}

