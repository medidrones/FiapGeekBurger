using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Production.Model
{
    /// <summary>
    /// Classe modelo para área de produção
    /// </summary>
    public class ProductionArea
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        /// <summary>
        /// Restrições existentes para a respectiva área de produção
        /// </summary>
        public List<Restriction> Restrictions { get; set; }
    }
}
