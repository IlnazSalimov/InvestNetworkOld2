//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InvestNetwork.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int MessageID { get; set; }
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        public string MessageText { get; set; }
        public System.DateTime MessageDate { get; set; }
    }
}
