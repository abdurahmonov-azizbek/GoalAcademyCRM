// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class InvalidPaymentException : Xeption
    {
        public InvalidPaymentException()
            : base(message: "Payment is invalid.")
        { }
    }
}
