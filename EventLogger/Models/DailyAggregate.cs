//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventLogger.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DailyAggregate
    {
        public Nullable<int> app_id { get; set; }
        public int event_type_id { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public Nullable<int> count { get; set; }
        public string app_name { get; set; }
        public string event_type_name { get; set; }
    }
}