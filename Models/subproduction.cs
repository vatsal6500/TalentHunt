//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TalentHunt.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class subproduction
    {
        public int spid { get; set; }
        public int planid { get; set; }
        public int pid { get; set; }
        public System.DateTime startdate { get; set; }
        public System.DateTime enddate { get; set; }
    
        public virtual plan plan { get; set; }
        public virtual production production { get; set; }
    }
}
