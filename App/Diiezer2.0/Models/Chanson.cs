//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Chanson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chanson()
        {
            this.Achat = new HashSet<Achat>();
            this.Note = new HashSet<Note>();
        }
    
        public int Id { get; set; }
        public Nullable<int> Durée { get; set; }
        public string Titre { get; set; }
        public Nullable<int> Album { get; set; }
        public string Musique { get; set; }
        public string Extrait { get; set; }
    
        public virtual Album Album1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Achat> Achat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Note { get; set; }
    }
}
