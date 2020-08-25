
using System.Linq;
using AutoMapper;
using ProAgil.api.DTOs;
using ProAgil.Dominio;

namespace ProAgil.api.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDTO>()
                .ForMember(destino => destino.Palestrantes, opcoes => {
                    opcoes.MapFrom(origem => origem.PalestranteEventos.Select(s => s.Paletrante).ToList());
                    }).ReverseMap();

            //Mapeamento N x N
            CreateMap<Palestrante, PalestranteDTO>()
                .ForMember(destino => destino.Eventos, opcoes => {
                    opcoes.MapFrom(origem => origem.PalestranteEventos.Select(s => s.Evento).ToList());
                }).ReverseMap();
            
            CreateMap<Lote, LoteDTO>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDTO>().ReverseMap();           
        }


    }
}