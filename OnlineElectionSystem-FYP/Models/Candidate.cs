//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Candidate
    {
        public int Id { get; set; }
        public string CNIC { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public Nullable<int> ConstituteId { get; set; }
        public string PoliticalParty { get; set; }
        public string Symbol { get; set; }
        public Constitution Constitution { get; set; }
        public int numberOfVotes { get; set; }
    }
}
