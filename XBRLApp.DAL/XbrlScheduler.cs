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
    
    public partial class XbrlScheduler
    {
        public int PK_XbrlScheduler_Id { get; set; }
        public Nullable<int> FK_XbrlSchedulerMaster_Id { get; set; }
        public Nullable<System.DateTime> NextRunTime { get; set; }
        public Nullable<byte> SchedulerType { get; set; }
        public Nullable<int> SchedulerInterval { get; set; }
        public Nullable<bool> Enabled { get; set; }
    }
}
