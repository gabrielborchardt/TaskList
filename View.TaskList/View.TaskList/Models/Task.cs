using System;

namespace View.TaskList.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Status { get; set; } //A = Aberto / R = Removido / C = Concluido
        public string Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
        public DateTime? DataRemocao { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
