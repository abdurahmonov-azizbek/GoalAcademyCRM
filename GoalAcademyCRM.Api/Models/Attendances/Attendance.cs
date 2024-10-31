// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

namespace GoalAcademyCRM.Api.Models.Attendances
{
	public class Attendance
	{
	    public Guid Id { get; set; }
	    public DateTime Date { get; set; }
	    public Guid GroupId { get; set; }
	    public Guid StudentId { get; set; }
	    public bool IsCame { get; set; }
	    public string Reason { get; set; }
	    public int Score { get; set; }
	    public DateTimeOffset CreatedDate { get; set; }
	    public DateTimeOffset UpdatedDate { get; set; }
	}
}