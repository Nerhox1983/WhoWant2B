namespace WhoWant2B.Models
{
    public class PreguntaConOpciones_viewModel
    {
        // Contiene los datos del enunciado, categoría y complejidad
        public Pregunta_model Pregunta { get; set; } = new Pregunta_model { Texto = string.Empty };

        // Colección de las 4 opciones de respuesta
        public List<Opcion_model> Opciones { get; set; } = new List<Opcion_model>();
    }
}
