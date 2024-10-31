// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;
using Microsoft.EntityFrameworkCore;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentGroup> StudentGroups { get; set; }

        public async ValueTask<StudentGroup> InsertStudentGroupAsync(StudentGroup studentGroup) =>
            await InsertAsync(studentGroup);

        public IQueryable<StudentGroup> SelectAllStudentGroups() =>
            SelectAll<StudentGroup>();

        public async ValueTask<StudentGroup> SelectStudentGroupByIdAsync(Guid studentGroupId) =>
            await SelectAsync<StudentGroup>(studentGroupId);

        public async ValueTask<StudentGroup> DeleteStudentGroupAsync(StudentGroup studentGroup) =>
            await DeleteAsync(studentGroup);

        public async ValueTask<StudentGroup> UpdateStudentGroupAsync(StudentGroup studentGroup) =>
            await UpdateAsync(studentGroup);
    }
}