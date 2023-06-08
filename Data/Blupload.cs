﻿using System;
using System.Collections.Generic;

namespace ServiceManagerApi.Data;

public partial class Blupload
{
    public double Idno { get; set; }

    public string? EnPJobNo { get; set; }

    public string? EquipNo { get; set; }

    public string? Sections { get; set; }

    public DateTime? EntryDate { get; set; }

    public string Days { get; set; } = null!;

    public string? Problem { get; set; }

    public string? Status { get; set; }

    public DateTime Month { get; set; }

    public string? ActionToBeTaken { get; set; }

    public string? Priority { get; set; }

    public double? MachineHours { get; set; }

    public string? SystemSympton { get; set; }

    public string? Source { get; set; }

    public string? OrderNo { get; set; }

    public string ScheduledDate { get; set; } = null!;

    public string PartsArrivalDate { get; set; } = null!;

    public DateTime ExecutionDate { get; set; }

    public string? PartsOnHand { get; set; }

    public string MakeAndModel { get; set; } = null!;

    public string? FleetManager { get; set; }

    public string? FleetAnalyst { get; set; }

    public string? GeneralComment { get; set; }

    public string Priority1 { get; set; } = null!;

    public string? MantracJobNo { get; set; }

    public string? Responsibility { get; set; }

    public string? Initiator { get; set; }
}
