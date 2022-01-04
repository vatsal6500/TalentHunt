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
    
    public partial class production
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public production()
        {
            this.eventrequires = new HashSet<eventrequire>();
            this.subproductions = new HashSet<subproduction>();
            this.userapplies = new HashSet<userapply>();
            this.productionevents = new HashSet<productionevent>();
        }
    
        public int pid { get; set; }
        public string pname { get; set; }
        public string pimage { get; set; }
        public string phead { get; set; }
        public string address { get; set; }
        public long contactno { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public string status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eventrequire> eventrequires { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<subproduction> subproductions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userapply> userapplies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<productionevent> productionevents { get; set; }
    }
}
