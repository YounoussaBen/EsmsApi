﻿using System;
using System.Collections.Generic;

namespace ServiceManagerApi.Data;

public partial class ProBlast
{
    public int Id { get; set; }

    public DateTime? BlastDate { get; set; }

    public int? PitLocationId { get; set; }

    public int? BenchLevel { get; set; }

    public int? Depth { get; set; }

    public string? PatternSize { get; set; }

    public int? ExpansionFactor { get; set; }

    public int? NumberOfHoles { get; set; }

    public int? SurveyProductionHoles { get; set; }

    public double? Area { get; set; }

    public double? SurveyVol { get; set; }

    public double? CummulativeBlastVol { get; set; }

    public string? LostRodGet { get; set; }

    public string? TenantId { get; set; }

    public string? BlastNumber { get; set; }

    public virtual ProductionOrigin? PitLocation { get; set; }
}
