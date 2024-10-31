// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Course> InsertCourseAsync(Course course);
        IQueryable<Course> SelectAllCourses();
        ValueTask<Course> SelectCourseByIdAsync(Guid courseId);
        ValueTask<Course> DeleteCourseAsync(Course course);
        ValueTask<Course> UpdateCourseAsync(Course course);
    }
}