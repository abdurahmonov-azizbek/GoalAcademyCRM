// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using Xeptions;

namespace GoalAcademyCRM.Api.Models.Payments.Exceptions
{
    public class AlreadyExistsPaymentException : Xeption
    {
        public AlreadyExistsPaymentException(Exception innerException)
            : base(message: "Payment already exists.", innerException)
        { }
    }
}
