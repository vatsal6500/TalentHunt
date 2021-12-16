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
    
    public partial class plan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public plan()
        {
            this.subproductions = new HashSet<subproduction>();
            this.subusers = new HashSet<subuser>();
        }
    
        public int planid { get; set; }
        public string plantype { get; set; }
        public string duration { get; set; }
        public int price { get; set; }
        public string description { get; set; }
        public string benefits { get; set; }
        public Nullable<int> maxevents { get; set; }
        public Nullable<int> maxbids { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subproduction> subproductions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subuser> subusers { get; set; }
    }
}
