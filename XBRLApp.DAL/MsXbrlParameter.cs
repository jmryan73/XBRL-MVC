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
    
    public partial class MsXbrlParameter
    {
        public int PK_MsXbrlParameter_Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public Nullable<byte> ParameterType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdateDate { get; set; }
        public string LastUpdateBy { get; set; }
    }
}