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
    
    public partial class Citizen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CNIC { get; set; }
        public string Password { get; set; }
        public string DateOfBirth { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentAreaAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PermanentAreaAddress { get; set; }
        public string Type { get; set; }
        public string lastLogin { get; set; }
        public string occupation { get; set; }
        public Nullable<int> ConstituencyId { get; set; }
        public bool rememberMe { get; set; }
    }
}
