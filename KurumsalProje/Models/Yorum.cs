//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KurumsalProje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Yorum
    {
        public int YorumId { get; set; }
        public string AdSoyad { get; set; }
        public string Eposta { get; set; }
        public string İcerik { get; set; }
        public Nullable<int> BlogId { get; set; }
        public Nullable<bool> Onay { get; set; }
    
        public virtual Blog Blog { get; set; }
    }
}