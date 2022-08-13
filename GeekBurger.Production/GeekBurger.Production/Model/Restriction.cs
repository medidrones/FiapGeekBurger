using System;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.Production.Model
{
    /// <summary>
    /// Classe modelo de restrições
    /// </summary>
    public class Restriction
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Para restrições, foi considerado apenas o nome e id, dessa forma qualquer validação será feita por comparação com o nome do elemento.
        /// </summary>
        public string Name { get; set; }
    }
}
