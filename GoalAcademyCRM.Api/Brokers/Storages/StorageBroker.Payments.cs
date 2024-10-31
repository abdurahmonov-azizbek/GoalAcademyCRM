// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Payments;
using Microsoft.EntityFrameworkCore;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Payment> Payments { get; set; }

        public async ValueTask<Payment> InsertPaymentAsync(Payment payment) =>
            await InsertAsync(payment);

        public IQueryable<Payment> SelectAllPayments() =>
            SelectAll<Payment>();

        public async ValueTask<Payment> SelectPaymentByIdAsync(Guid paymentId) =>
            await SelectAsync<Payment>(paymentId);

        public async ValueTask<Payment> DeletePaymentAsync(Payment payment) =>
            await DeleteAsync(payment);

        public async ValueTask<Payment> UpdatePaymentAsync(Payment payment) =>
            await UpdateAsync(payment);
    }
}