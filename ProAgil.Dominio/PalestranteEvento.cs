namespace ProAgil.Dominio
{
    public class PalestranteEvento
    {
        public int PalestranteId { get; set; }
        public Palestrante Paletrante { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}