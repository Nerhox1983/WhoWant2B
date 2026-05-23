using WhoWant2B.Core.Models;

namespace WhoWant2B.Models
{
    public class PreguntaConOpciones_viewModel
    {
        
        public Pregunta_model Pregunta { get; set; } = new Pregunta_model { Texto = string.Empty };
        
        public List<Opcion_model> Opciones { get; set; } = new List<Opcion_model>();
    }
}
