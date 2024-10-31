// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using GoalAcademyCRM.Api.Models.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Payment> InsertPaymentAsync(Payment payment);
        IQueryable<Payment> SelectAllPayments();
        ValueTask<Payment> SelectPaymentByIdAsync(Guid paymentId);
        ValueTask<Payment> DeletePaymentAsync(Payment payment);
        ValueTask<Payment> UpdatePaymentAsync(Payment payment);
    }
}