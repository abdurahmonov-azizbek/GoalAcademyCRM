// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

namespace GoalAcademyCRM.Api.Models.Courses
{
	public class Course
	{
	    public Guid Id { get; set; }
	    public string Name { get; set; }
	    public int Price { get; set; }
	    public DateTimeOffset CreatedDate { get; set; }
	    public DateTimeOffset UpdatedDate { get; set; }
	}
}