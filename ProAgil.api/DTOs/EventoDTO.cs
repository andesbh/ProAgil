using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.api.DTOs
{
    public class EventoDTO
    {
            public int Id { get; set; }

            [Required(ErrorMessage="Campo Obrigatório"), StringLength(100, MinimumLength=3, ErrorMessage="O local é entre 3 e 100 caracteres")]
            public string Local { get; set; }
            public string DataEvento { get; set; }
            
            [Required(ErrorMessage="O campo {0} deve ser informado")]
            public string Tema { get; set; }

            [Range(1,150, ErrorMessage="A quantidade de pessoas deve ser entre 1 e 150")]
            public int QuantidadeDePessoas { get; set; }
            public string ImagemUrl { get; set; }
            public string Telefone { get; set; }
            
            [EmailAddress]
            public string Email { get; set; }
            public List<LoteDTO> Lotes { get; set; }
            public List<RedeSocialDTO> RedeSociais { get; set; }
            public List<PalestranteDTO> Palestrantes { get; set; }
    }
}