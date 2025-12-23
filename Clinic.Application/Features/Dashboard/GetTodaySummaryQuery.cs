namespace ApiSitemaClinico.Clinic.Application.Features.Dashboard
{
  public record TodaySummaryDto(
  List<PatientTodayDto> PatientsToday,
  DailyStatsDto Stats
);

  public record PatientTodayDto(
      long Id,
      string Name,
      string Time,
      string Reason,
      string Status
  );

  public record DailyStatsDto(
      int TotalSlots,
      int BookedSlots,
      int Attended,
      int Pending,
      int Cancelled
  );
}
