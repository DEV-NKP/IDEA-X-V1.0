﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDEA_X.Models.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IDEA_XEntities : DbContext
    {
        public IDEA_XEntities()
            : base("name=IDEA_XEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADMIN> ADMINS { get; set; }
        public virtual DbSet<ALL_USERS> ALL_USERS { get; set; }
        public virtual DbSet<CHAT_BOXS> CHAT_BOXS { get; set; }
        public virtual DbSet<CONTACT> CONTACTS { get; set; }
        public virtual DbSet<GENERAL_POSTS> GENERAL_POSTS { get; set; }
        public virtual DbSet<LOGIN> LOGINS { get; set; }
        public virtual DbSet<MESSAGE_REQUESTS> MESSAGE_REQUESTS { get; set; }
        public virtual DbSet<NOTE> NOTES { get; set; }
        public virtual DbSet<POST_ACTIONS> POST_ACTIONS { get; set; }
        public virtual DbSet<POST_REPORT> POST_REPORT { get; set; }
        public virtual DbSet<USER_ACCESS_CONTROLLER> USER_ACCESS_CONTROLLER { get; set; }
        public virtual DbSet<USER_DETAILS> USER_DETAILS { get; set; }
        public virtual DbSet<USER_MESSAGES> USER_MESSAGES { get; set; }
    }
}