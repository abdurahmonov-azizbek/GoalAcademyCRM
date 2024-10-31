// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

namespace GoalAcademyCRM.Api.Models.Groups
{
	public class Group
	{
	    public Guid Id { get; set; }
	    public string Name { get; set; }
	    public Guid CourseId { get; set; }
	    public Guid TeacherId { get; set; }
	    public DateOnly StartDate { get; set; }
	    public DateOnly EndDate { get; set; }
	    public TimeOnly StartTime { get; set; }
	    public TimeOnly EndTime { get; set; }
	    public bool Odd { get; set; }
	    public DateTimeOffset CreatedDate { get; set; }
	    public DateTimeOffset UpdatedDate { get; set; }
	}
}