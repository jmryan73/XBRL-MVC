//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XBRLApp.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class MemoryUtilize
    {
        public int PK_MemoryUtilize_Id { get; set; }
        public Nullable<long> TotalMemoryInMB { get; set; }
        public Nullable<long> AvailablePhysicalMemoryInMB { get; set; }
        public Nullable<decimal> OccupiedMemory { get; set; }
        public Nullable<decimal> FreeMemory { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
    }
}
