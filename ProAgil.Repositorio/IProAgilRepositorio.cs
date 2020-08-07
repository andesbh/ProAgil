using System.Threading.Tasks;
using ProAgil.Dominio;

namespace ProAgil.Repositorio
{
    public interface IProAgilRepositorio
    {
        void Add<T> (T Entity) where T: class;
        void Update<T>(T Entity) where T: class;
        void Delete<T> (T Entity) where T: class;

        Task<bool> SalveChangesAsync();

        Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes);
        Task<Evento[]> GetAllEventoAsync();
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes);
        Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes);


        Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos);
        Task<Palestrante> GetPalestranteAsync(int PalestranteID, bool includeEventos);        

    }
}