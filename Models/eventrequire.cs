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
    
    public partial class eventrequire
    {
        public int erid { get; set; }
        public int pid { get; set; }
        public int peid { get; set; }
        public int tid { get; set; }
        public string agerange { get; set; }
        public string gender { get; set; }
        public string payrange { get; set; }
    
        public virtual production production { get; set; }
        public virtual talent talent { get; set; }
        public virtual productionevent productionevent { get; set; }
    }
}
