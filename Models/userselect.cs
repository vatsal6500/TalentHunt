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
    
    public partial class userselect
    {
        public int usid { get; set; }
        public int peid { get; set; }
        public int userid { get; set; }
        public int finalpay { get; set; }
    
        public virtual productionevent productionevent { get; set; }
        public virtual user user { get; set; }
    }
}
