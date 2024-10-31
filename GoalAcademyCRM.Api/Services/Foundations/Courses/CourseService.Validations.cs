// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.Courses;
using GoalAcademyCRM.Api.Models.Courses.Exceptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Courses
{
    public partial class CourseService
    {
        private void ValidateCourseOnAdd(Course course)
        {
            ValidateCourseNotNull(course);

            Validate(
				(Rule: IsInvalid(course.Id), Parameter: nameof(Course.Id)),
				(Rule: IsInvalid(course.Name), Parameter: nameof(Course.Name)),
				(Rule: IsInvalid(course.CreatedDate), Parameter: nameof(Course.CreatedDate)),
				(Rule: IsInvalid(course.UpdatedDate), Parameter: nameof(Course.UpdatedDate)),

                (Rule: IsNotRecent(course.CreatedDate), Parameter: nameof(Course.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: course.CreatedDate,
                    secondDate: course.UpdatedDate,
                    secondDateName: nameof(Course.UpdatedDate)),

                    Parameter: nameof(Course.CreatedDate)));
        }

        private void ValidateCourseOnModify(Course course)
        {
            ValidateCourseNotNull(course);

            Validate(
				(Rule: IsInvalid(course.Id), Parameter: nameof(Course.Id)),
				(Rule: IsInvalid(course.Name), Parameter: nameof(Course.Name)),
				(Rule: IsInvalid(course.CreatedDate), Parameter: nameof(Course.CreatedDate)),
				(Rule: IsInvalid(course.UpdatedDate), Parameter: nameof(Course.UpdatedDate)),

                (Rule: IsNotRecent(course.UpdatedDate), Parameter: nameof(Course.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: course.UpdatedDate,
                    secondDate: course.CreatedDate,
                    secondDateName: nameof(course.CreatedDate)),
                    Parameter: nameof(course.UpdatedDate)));
        }

        private static void ValidateAgainstStorageCourseOnModify(Course inputCourse, Course storageCourse)
        {
            ValidateStorageCourse(storageCourse, inputCourse.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputCourse.CreatedDate,
                    secondDate: storageCourse.CreatedDate,
                    secondDateName: nameof(Course.CreatedDate)),
                    Parameter: nameof(Course.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputCourse.UpdatedDate,
                        secondDate: storageCourse.UpdatedDate,
                        secondDateName: nameof(Course.UpdatedDate)),
                        Parameter: nameof(Course.UpdatedDate)));
        }

        private static void ValidateStorageCourse(Course maybeCourse, Guid courseId)
        {
            if (maybeCourse is null)
            {
                throw new NotFoundCourseException(courseId);
            }
        }

        private void ValidateCourseId(Guid courseId) =>
             Validate((Rule: IsInvalid(courseId), Parameter: nameof(Course.Id)));

        private void ValidateCourseNotNull(Course course)
        {
            if (course is null)
            {
                throw new NullCourseException();
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
            var invalidCourseException = new InvalidCourseException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCourseException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidCourseException.ThrowIfContainsErrors();
        }
    }
}
