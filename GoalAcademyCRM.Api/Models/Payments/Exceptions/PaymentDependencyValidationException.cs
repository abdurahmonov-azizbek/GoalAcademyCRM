// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class PaymentDependencyValidationException : Xeption
    {
        public PaymentDependencyValidationException(Xeption innerException)
            : base(message: "Payment dependency validation error occurred, fix the errors and try again.",
                  innerException)
        { }
    }
}
