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
    
    public partial class EmailTemplate
    {
        public int PK_EmailTemplate_Id { get; set; }
        public string ProfileName { get; set; }
        public string EmailTemplate1 { get; set; }
        public Nullable<bool> IsActiveProfile { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}