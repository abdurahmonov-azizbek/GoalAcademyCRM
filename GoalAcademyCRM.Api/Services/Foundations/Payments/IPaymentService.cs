// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using GoalAcademyCRM.Api.Models.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoalAcademyCRM.Api.Services.Foundations.Payments
{
    public interface IPaymentService
    {
        /// <exception cref="Models.Payments.Exceptions.PaymentValidationException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyValidationException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentServiceException"></exception>
        ValueTask<Payment> AddPaymentAsync(Payment payment);

        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentServiceException"></exception>
        IQueryable<Payment> RetrieveAllPayments();

        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentServiceException"></exception>
        ValueTask<Payment> RetrievePaymentByIdAsync(Guid paymentId);

        /// <exception cref="Models.Payments.Exceptions.PaymentValidationException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyValidationException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentServiceException"></exception>
        ValueTask<Payment> ModifyPaymentAsync(Payment payment);

        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyValidationException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentDependencyException"></exception>
        /// <exception cref="Models.Payments.Exceptions.PaymentServiceException"></exception>
        ValueTask<Payment> RemovePaymentByIdAsync(Guid paymentId);
    }
}