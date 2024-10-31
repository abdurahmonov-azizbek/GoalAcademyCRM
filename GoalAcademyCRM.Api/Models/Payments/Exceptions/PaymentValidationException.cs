// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class PaymentValidationException : Xeption
    {
        public PaymentValidationException(Xeption innerException)
            : base(message: "Payment validation error occured, fix the errors and try again.", innerException)
        { }
    }
}
