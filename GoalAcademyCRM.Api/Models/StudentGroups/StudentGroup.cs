// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

namespace GoalAcademyCRM.Api.Models.StudentGroups
{
	public class StudentGroup
	{
	    public Guid Id { get; set; }
	    public Guid StudentId { get; set; }
	    public Guid GroupId { get; set; }
	    public DateTimeOffset CreatedDate { get; set; }
	    public DateTimeOffset UpdatedDate { get; set; }
	}
}