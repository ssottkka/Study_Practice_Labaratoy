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
    
    public partial class Состояние_услуги
    {
        public int Код_состояния_услуги { get; set; }
        public Nullable<int> Код_заказа { get; set; }
        public Nullable<int> Код_услуги { get; set; }
        public Nullable<int> Статус { get; set; }
        public string Результат { get; set; }
    
        public virtual Заказ Заказ { get; set; }
        public virtual Статус_Услуги Статус_Услуги { get; set; }
        public virtual Услуги Услуги { get; set; }
    }
}
