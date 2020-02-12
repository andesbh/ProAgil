using System;
using System.ComponentModel.DataAnnotations;


namespace ProAgil.api.Models
{
       public class Evento
    {
        [Key]
        public int EventoID {get; set;}
        public string Tema {get; set;}
        public string Local {get; set;}
    }

}