// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class NotFoundPaymentException : Xeption
    {
        public NotFoundPaymentException(Guid paymentId)
            : base(message: $"Couldn't find payment with id: {paymentId}.")
        { }
    }
}
