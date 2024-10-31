// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.Groups;
using GoalAcademyCRM.Api.Models.Groups.Exceptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Groups
{
    public partial class GroupService
    {
        private void ValidateGroupOnAdd(Group group)
        {
            ValidateGroupNotNull(group);

            Validate(
				(Rule: IsInvalid(group.Id), Parameter: nameof(Group.Id)),
				(Rule: IsInvalid(group.Name), Parameter: nameof(Group.Name)),
				(Rule: IsInvalid(group.CourseId), Parameter: nameof(Group.CourseId)),
				(Rule: IsInvalid(group.TeacherId), Parameter: nameof(Group.TeacherId)),
				(Rule: IsInvalid(group.CreatedDate), Parameter: nameof(Group.CreatedDate)),
				(Rule: IsInvalid(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)),

                (Rule: IsNotRecent(group.CreatedDate), Parameter: nameof(Group.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: group.CreatedDate,
                    secondDate: group.UpdatedDate,
                    secondDateName: nameof(Group.UpdatedDate)),

                    Parameter: nameof(Group.CreatedDate)));
        }

        private void ValidateGroupOnModify(Group group)
        {
            ValidateGroupNotNull(group);

            Validate(
				(Rule: IsInvalid(group.Id), Parameter: nameof(Group.Id)),
				(Rule: IsInvalid(group.Name), Parameter: nameof(Group.Name)),
				(Rule: IsInvalid(group.CourseId), Parameter: nameof(Group.CourseId)),
				(Rule: IsInvalid(group.TeacherId), Parameter: nameof(Group.TeacherId)),
				(Rule: IsInvalid(group.CreatedDate), Parameter: nameof(Group.CreatedDate)),
				(Rule: IsInvalid(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)),

                (Rule: IsNotRecent(group.UpdatedDate), Parameter: nameof(Group.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: group.UpdatedDate,
                    secondDate: group.CreatedDate,
                    secondDateName: nameof(group.CreatedDate)),
                    Parameter: nameof(group.UpdatedDate)));
        }

        private static void ValidateAgainstStorageGroupOnModify(Group inputGroup, Group storageGroup)
        {
            ValidateStorageGroup(storageGroup, inputGroup.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputGroup.CreatedDate,
                    secondDate: storageGroup.CreatedDate,
                    secondDateName: nameof(Group.CreatedDate)),
                    Parameter: nameof(Group.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputGroup.UpdatedDate,
                        secondDate: storageGroup.UpdatedDate,
                        secondDateName: nameof(Group.UpdatedDate)),
                        Parameter: nameof(Group.UpdatedDate)));
        }

        private static void ValidateStorageGroup(Group maybeGroup, Guid groupId)
        {
            if (maybeGroup is null)
            {
                throw new NotFoundGroupException(groupId);
            }
        }

        private void ValidateGroupId(Guid groupId) =>
             Validate((Rule: IsInvalid(groupId), Parameter: nameof(Group.Id)));

        private void ValidateGroupNotNull(Group group)
        {
            if (group is null)
            {
                throw new NullGroupException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidGroupException = new InvalidGroupException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidGroupException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidGroupException.ThrowIfContainsErrors();
        }
    }
}
