// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

namespace GoalAcademyCRM.Api.Models.Payments
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}