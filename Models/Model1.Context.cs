﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class huntdbEntities : DbContext
    {
        public huntdbEntities()
            : base("name=huntdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<eventrequire> eventrequires { get; set; }
        public virtual DbSet<image> images { get; set; }
        public virtual DbSet<plan> plans { get; set; }
        public virtual DbSet<production> productions { get; set; }
        public virtual DbSet<subproduction> subproductions { get; set; }
        public virtual DbSet<talent> talents { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<userapply> userapplies { get; set; }
        public virtual DbSet<userprofile> userprofiles { get; set; }
        public virtual DbSet<userselect> userselects { get; set; }
        public virtual DbSet<video> videos { get; set; }
        public virtual DbSet<rating> ratings { get; set; }
        public virtual DbSet<eventrate> eventrates { get; set; }
        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<productionevent> productionevents { get; set; }
        public virtual DbSet<subuser> subusers { get; set; }
    }
}
