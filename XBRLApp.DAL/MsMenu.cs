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
    
    public partial class MsMenu
    {
        public int PK_MsMenu_Id { get; set; }
        public Nullable<int> FK_MsGroupMenu_Id { get; set; }
        public string MenuName { get; set; }
        public string MenuHyperlink { get; set; }
        public Nullable<int> ParentId { get; set; }
    }
}