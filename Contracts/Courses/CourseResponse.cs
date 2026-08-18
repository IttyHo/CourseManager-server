namespace CourseManager.Api.Contracts.Courses;

public sealed record CourseResponse(
    int Id,
    string Name,
    string Description,
    int DurationHours,
    DateTime CreatedAt);
