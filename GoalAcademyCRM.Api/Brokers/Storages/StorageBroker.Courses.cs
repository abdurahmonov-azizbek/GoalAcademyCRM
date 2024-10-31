// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Courses;
using Microsoft.EntityFrameworkCore;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Course> Courses { get; set; }

        public async ValueTask<Course> InsertCourseAsync(Course course) =>
            await InsertAsync(course);

        public IQueryable<Course> SelectAllCourses() =>
            SelectAll<Course>();

        public async ValueTask<Course> SelectCourseByIdAsync(Guid courseId) =>
            await SelectAsync<Course>(courseId);

        public async ValueTask<Course> DeleteCourseAsync(Course course) =>
            await DeleteAsync(course);

        public async ValueTask<Course> UpdateCourseAsync(Course course) =>
            await UpdateAsync(course);
    }
}