﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineElectionSystem_FYP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ElectionCommissionDatabaseEntities : DbContext
    {
        public ElectionCommissionDatabaseEntities()
            : base("name=ElectionCommissionDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Constitution> Constitutions { get; set; }
        public virtual DbSet<Election_Votes_Candidate> Election_Votes_Candidate { get; set; }
        public virtual DbSet<PoliticalParty> PoliticalParties { get; set; }
        public virtual DbSet<Voter> Voters { get; set; }
        public virtual DbSet<Constituency_Locations> Constituency_Locations { get; set; }
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<VoterSentiment> VoterSentiments { get; set; }
        public virtual DbSet<Candidate_Requests> Candidate_Requests { get; set; }
    }
}
