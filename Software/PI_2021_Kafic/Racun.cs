//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PI_2021_Kafic
{
    using System;
    using System.Collections.Generic;
    
    public partial class Racun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Racun()
        {
            this.Stavka_racuna = new HashSet<Stavka_racuna>();
        }
    
        public int ID_Racun { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<System.DateTime> Vrijeme { get; set; }
        public int Konobar_ID { get; set; }
        public string Stol { get; set; }
        public int Nacin_Placanja_ID { get; set; }
        public double Ukupna_cijena { get; set; }
        public int Kafic_ID { get; set; }
    
        public virtual Kafic Kafic { get; set; }
        public virtual Konobar Konobar { get; set; }
        public virtual Nacin_Placanja Nacin_Placanja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stavka_racuna> Stavka_racuna { get; set; }
    }
}
