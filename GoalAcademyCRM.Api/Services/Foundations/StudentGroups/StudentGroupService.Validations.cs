// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.StudentGroups;
using GoalAcademyCRM.Api.Models.StudentGroups.Exceptions;

namespace GoalAcademyCRM.Api.Services.Foundations.StudentGroups
{
    public partial class StudentGroupService
    {
        private void ValidateStudentGroupOnAdd(StudentGroup studentGroup)
        {
            ValidateStudentGroupNotNull(studentGroup);

            Validate(
				(Rule: IsInvalid(studentGroup.Id), Parameter: nameof(StudentGroup.Id)),
				(Rule: IsInvalid(studentGroup.StudentId), Parameter: nameof(StudentGroup.StudentId)),
				(Rule: IsInvalid(studentGroup.GroupId), Parameter: nameof(StudentGroup.GroupId)),
				(Rule: IsInvalid(studentGroup.CreatedDate), Parameter: nameof(StudentGroup.CreatedDate)),
				(Rule: IsInvalid(studentGroup.UpdatedDate), Parameter: nameof(StudentGroup.UpdatedDate)),

                (Rule: IsNotRecent(studentGroup.CreatedDate), Parameter: nameof(StudentGroup.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: studentGroup.CreatedDate,
                    secondDate: studentGroup.UpdatedDate,
                    secondDateName: nameof(StudentGroup.UpdatedDate)),

                    Parameter: nameof(StudentGroup.CreatedDate)));
        }

        private void ValidateStudentGroupOnModify(StudentGroup studentGroup)
        {
            ValidateStudentGroupNotNull(studentGroup);

            Validate(
				(Rule: IsInvalid(studentGroup.Id), Parameter: nameof(StudentGroup.Id)),
				(Rule: IsInvalid(studentGroup.StudentId), Parameter: nameof(StudentGroup.StudentId)),
				(Rule: IsInvalid(studentGroup.GroupId), Parameter: nameof(StudentGroup.GroupId)),
				(Rule: IsInvalid(studentGroup.CreatedDate), Parameter: nameof(StudentGroup.CreatedDate)),
				(Rule: IsInvalid(studentGroup.UpdatedDate), Parameter: nameof(StudentGroup.UpdatedDate)),

                (Rule: IsNotRecent(studentGroup.UpdatedDate), Parameter: nameof(StudentGroup.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: studentGroup.UpdatedDate,
                    secondDate: studentGroup.CreatedDate,
                    secondDateName: nameof(studentGroup.CreatedDate)),
                    Parameter: nameof(studentGroup.UpdatedDate)));
        }

        private static void ValidateAgainstStorageStudentGroupOnModify(StudentGroup inputStudentGroup, StudentGroup storageStudentGroup)
        {
            ValidateStorageStudentGroup(storageStudentGroup, inputStudentGroup.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputStudentGroup.CreatedDate,
                    secondDate: storageStudentGroup.CreatedDate,
                    secondDateName: nameof(StudentGroup.CreatedDate)),
                    Parameter: nameof(StudentGroup.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputStudentGroup.UpdatedDate,
                        secondDate: storageStudentGroup.UpdatedDate,
                        secondDateName: nameof(StudentGroup.UpdatedDate)),
                        Parameter: nameof(StudentGroup.UpdatedDate)));
        }

        private static void ValidateStorageStudentGroup(StudentGroup maybeStudentGroup, Guid studentGroupId)
        {
            if (maybeStudentGroup is null)
            {
                throw new NotFoundStudentGroupException(studentGroupId);
            }
        }

        private void ValidateStudentGroupId(Guid studentGroupId) =>
             Validate((Rule: IsInvalid(studentGroupId), Parameter: nameof(StudentGroup.Id)));

        private void ValidateStudentGroupNotNull(StudentGroup studentGroup)
        {
            if (studentGroup is null)
            {
                throw new NullStudentGroupException();
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
            var invalidStudentGroupException = new InvalidStudentGroupException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidStudentGroupException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidStudentGroupException.ThrowIfContainsErrors();
        }
    }
}
