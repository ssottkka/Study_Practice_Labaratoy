//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace task1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Статус_Услуги
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Статус_Услуги()
        {
            this.Состояние_услуги = new HashSet<Состояние_услуги>();
        }
    
        public int Код_статуса_услуги { get; set; }
        public string Наименование_статуса_услуги { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Состояние_услуги> Состояние_услуги { get; set; }
    }
}
