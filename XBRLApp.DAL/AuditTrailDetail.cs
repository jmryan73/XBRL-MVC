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
    
    public partial class AuditTrailDetail
    {
        public long PK_AuditTrailDetail_Id { get; set; }
        public Nullable<long> FK_AuditTrailHeader_id { get; set; }
        public string FieldName { get; set; }
        public string FieldValue_Old { get; set; }
        public string FieldValue_New { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}
