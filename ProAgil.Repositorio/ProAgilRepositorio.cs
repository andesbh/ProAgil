using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Dominio;

namespace ProAgil.Repositorio
{
    public class ProAgilRepositorio : IProAgilRepositorio
    {
        public ProAgilContext _Contexto { get; }

        public ProAgilRepositorio(ProAgilContext Contexto)
        {
            this._Contexto = Contexto;

            //Não travar as transação de forma geral 
            //this._Contexto.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }


        public void Add<T>(T Entity) where T : class
        {
            this._Contexto.Add(Entity);
        }

        public void Delete<T>(T Entity) where T : class
        {
            this._Contexto.Remove(Entity);
        }

        public void Update<T>(T Entity) where T : class
        {
           this._Contexto.Update(Entity);
        }

        public async Task<bool> SalveChangesAsync()
        {
            return (await this._Contexto.SaveChangesAsync()) > 0;
        }


        //Eventos

        public async Task<Evento[]> GetAllEventoAsync()
        {
            return await GetAllEventoAsync(false);
        }

        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes)
        {
            IQueryable<Evento> query = this._Contexto.Eventos
                        .Include(c => c.Lotes)
                        .Include(c => c.RedeSociais);
            
            if(includePalestrantes) 
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Paletrante);
            }

            query.AsNoTracking().OrderByDescending(o => o.DataEvento);


            return await query.ToArrayAsync();   

        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = this._Contexto.Eventos
                        .Include(c => c.Lotes)
                        .Include(c => c.RedeSociais);
            
            if(includePalestrantes) 
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Paletrante);
            }

            query.AsNoTracking().OrderByDescending(o => o.DataEvento)
                        .Where (w => w.Tema.ToUpper().Contains(tema.ToUpper()));


            return await query.ToArrayAsync();   
        }

        public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = this._Contexto.Eventos
                        .Include(c => c.Lotes)
                        .Include(c => c.RedeSociais);
            
            if(includePalestrantes) 
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(p => p.Paletrante);
            }

            query.AsNoTracking().OrderByDescending(o => o.DataEvento)
                        .Where (w => w.Id == EventoId);


            return await query.FirstOrDefaultAsync();   
        }
    
        //Palestrantes
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = this._Contexto.Palestrantes
                    .Include(i => i.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Nome)
                    .Where(w => w.Nome.ToUpper().Contains(name.ToUpper()));
            
            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteAsync(int PalestranteID, bool includeEventos)
        {
           IQueryable<Palestrante> query = this._Contexto.Palestrantes
                    .Include(i => i.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(pe => pe.PalestranteEventos)
                    .ThenInclude(e => e.Evento);
            }

            query = query.AsNoTracking().OrderBy(p => p.Nome)
                    .Where(w => w.Id == PalestranteID);
            
            return await query.FirstOrDefaultAsync();

        }


    }
}