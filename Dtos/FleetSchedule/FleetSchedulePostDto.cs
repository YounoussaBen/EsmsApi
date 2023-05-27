﻿namespace ServiceManagerApi.Dtos.FleetSchedule;

public class FleetSchedulePostDto
{
  public long EntryId { get; set; }

  public string FleetId { get; set; } = null!;

  public string VmModel { get; set; } = null!;

  public string VmClass { get; set; } = null!;

  public int? ServiceTypeId { get; set; }

  public string LocationId { get; set; } = null!;

  public string? Description { get; set; }

  public DateTime? TimeStart { get; set; }

  public DateTime? TimeEnd { get; set; }

  public string? Responsible { get; set; }

  public string? ReferenceId { get; set; }

  public string? TenantId { get; set; }
}