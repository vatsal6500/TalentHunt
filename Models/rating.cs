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
    
    public partial class rating
    {
        public int rid { get; set; }
        public int userid { get; set; }
        public int peid { get; set; }
        public double rating1 { get; set; }
        public string comment { get; set; }
    
        public virtual user user { get; set; }
        public virtual productionevent productionevent { get; set; }
    }
}
