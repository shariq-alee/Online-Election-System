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
    
    public partial class Candidate_Requests
    {
        public int Id { get; set; }
        public Nullable<int> CitizenId { get; set; }
        public Nullable<int> ConstitutionId { get; set; }
        public string RequestNote { get; set; }
        public string Decision { get; set; }
        public string PoliticalParty { get; set; }
        public byte[] Documents { get; set; }
        public Citizen Citizen { get; set; }
        public Constitution Constitution { get; set; }
    }
}
