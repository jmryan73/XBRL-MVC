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
    
    public partial class ProcessDownloadXbrl
    {
        public int PK_ProcessDownloadXbrl_Id { get; set; }
        public string BankCode { get; set; }
        public string Created7zName { get; set; }
        public string FullFilePath7z { get; set; }
        public Nullable<System.DateTime> ReportingDate { get; set; }
    }
}
