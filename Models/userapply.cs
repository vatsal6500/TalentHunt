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
    using System.ComponentModel.DataAnnotations;

    public partial class userapply
    {
        public int uaid { get; set; }
        public int pid { get; set; }
        public int peid { get; set; }
        public int userid { get; set; }

        [DataType(DataType.Date)]
        public DateTime appdate { get; set; }
        public int expay { get; set; }
        public string message { get; set; }
    
        public virtual production production { get; set; }
        public virtual productionevent productionevent { get; set; }
        public virtual user user { get; set; }
    }
}
