using System.ComponentModel.DataAnnotations;

namespace ProAgil.api.DTOs
{
    public class LoteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        [Range(2,2000)]
        public int Quantidade { get; set; }
    }
}