﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Diiezer2._0.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DiiezerDBEntities : DbContext
    {
        public DiiezerDBEntities()
            : base("name=DiiezerDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Artiste> Artiste { get; set; }
        public virtual DbSet<Chanson> Chanson { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Note> Note { get; set; }
    }
}
