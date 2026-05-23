using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Models;

namespace WhoWant2B.Controllers.Configuracion
{
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionService _configuracionService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionController"/>.
        /// </summary>
        /// <param name="configuracionService">El servicio de negocio para la gestión de configuraciones del sistema.</param>
        public ConfiguracionController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        /// <summary>
        /// Muestra la vista principal de la configuración.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region CRUD: Categorías

        /// <summary>
        /// Obtiene de forma asíncrona la lista de categorías y muestra su vista correspondiente con paginación.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Categorias(int? page)
        {
            int numeroPagina = page ?? 1;
            int cantidadPorPagina = 10;

            var (categorias, totalCategorias) = await _configuracionService.ObtenerCategoriasPaginadasAsync(numeroPagina, cantidadPorPagina);

            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)System.Math.Ceiling((double)totalCategorias / cantidadPorPagina);
            ViewBag.TotalRegistros = totalCategorias;

            return View(categorias);
        }

        /// <summary>
        /// Muestra la vista con el formulario para crear una nueva categoría.
        /// </summary>
        [HttpGet]
        public IActionResult CrearCategoria()
        {
            return View();
        }

        /// <summary>
        /// Procesa de forma asíncrona los datos del formulario para registrar una nueva categoría.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(Categoria_model categoria)
        {
            if (ModelState.IsValid)
            {
                await _configuracionService.GuardarCategoriaAsync(categoria);
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        /// <summary>
        /// Muestra de forma asíncrona la vista de edición para una categoría específica basada en su identificador.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditarCategoria(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _configuracionService.ObtenerCategoriaPorIdAsync(id.Value);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        /// <summary>
        /// Procesa de forma asíncrona los cambios guardados en el formulario para actualizar una categoría existente.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(int id, Categoria_model categoria)
        {
            if (id != categoria.IdCategoria) return NotFound();

            if (ModelState.IsValid)
            {
                await _configuracionService.ActualizarCategoriaAsync(categoria);
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        /// <summary>
        /// Elimina de forma asíncrona una categoría específica de la base de datos basada en su identificador.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            await _configuracionService.EliminarCategoriaAsync(id);
            return RedirectToAction(nameof(Categorias));
        }

        #endregion

        #region CRUD: Preguntas

        /// <summary>
        /// Obtiene de forma asíncrona la lista de preguntas paginada de 10 en 10.
        /// </summary> 
        [HttpGet]
        public async Task<IActionResult> Preguntas(int? page)
        {
            int numeroPagina = page ?? 1;
            int cantidadPorPagina = 10;

            var (preguntas, totalPreguntas) = await _configuracionService.ObtenerPreguntasPaginadasAsync(numeroPagina, cantidadPorPagina);

            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)System.Math.Ceiling((double)totalPreguntas / cantidadPorPagina);
            ViewBag.TotalRegistros = totalPreguntas;

            return View(preguntas);
        }

        /// <summary>
        /// Muestra de forma asíncrona la vista para crear una nueva pregunta, preparando los listados de selección.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CrearPregunta()
        {
            var categorias = await _configuracionService.ObtenerTodasLasCategoriasAsync();
            var complejidades = await _configuracionService.ObtenerTodasLasComplejidadesAsync();

            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre");
            ViewBag.IdComplejidad = new SelectList(complejidades, "IdComplejidad", "Nombre");

            var viewModel = new PreguntaConOpciones_viewModel();
            for (int i = 0; i < 4; i++)
            {
                viewModel.Opciones.Add(new Opcion_model { Texto = string.Empty });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Procesa de forma asíncrona la creación de una nueva pregunta junto con su listado de opciones asociadas.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPregunta(PreguntaConOpciones_viewModel model, int CorrectaIndex)
        {
            if (ModelState.IsValid)
            {
                bool guardadoExitoso = await _configuracionService.GuardarPreguntaConOpcionesAsync(model.Pregunta, model.Opciones, CorrectaIndex);
                if (guardadoExitoso)
                {
                    return RedirectToAction(nameof(Preguntas));
                }

                ModelState.AddModelError("", "Error interno al guardar la nueva pregunta con sus opciones.");
            }

            var categorias = await _configuracionService.ObtenerTodasLasCategoriasAsync();
            var complejidades = await _configuracionService.ObtenerTodasLasComplejidadesAsync();

            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", model.Pregunta.IdCategoria);
            ViewBag.IdComplejidad = new SelectList(complejidades, "IdComplejidad", "Nombre", model.Pregunta.IdComplejidad);
            return View(model);
        }

        /// <summary>
        /// Muestra de forma asíncrona la vista de edición para una pregunta específica, cargando sus opciones relacionadas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditarPregunta(int? id)
        {
            if (id == null) return NotFound();

            var pregunta = await _configuracionService.ObtenerPreguntaConOpcionesPorIdAsync(id.Value);
            if (pregunta == null) return NotFound();

            var categorias = await _configuracionService.ObtenerTodasLasCategoriasAsync();
            var complejidades = await _configuracionService.ObtenerTodasLasComplejidadesAsync();

            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", pregunta.IdCategoria);
            ViewBag.IdComplejidad = new SelectList(complejidades, "IdComplejidad", "Nombre", pregunta.IdComplejidad);

            var viewModel = new PreguntaConOpciones_viewModel
            {
                Pregunta = pregunta,
                Opciones = pregunta.Opciones.ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Procesa de forma asíncrona las modificaciones de una pregunta existente y sus opciones asociadas.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPregunta(int id, PreguntaConOpciones_viewModel model, int CorrectaIndex)
        {
            if (id != model.Pregunta.IdPregunta) return NotFound();

            if (ModelState.IsValid)
            {
                bool actualizacionExitosa = await _configuracionService.ActualizarPreguntaConOpcionesAsync(model.Pregunta, model.Opciones, CorrectaIndex);
                if (actualizacionExitosa)
                {
                    return RedirectToAction(nameof(Preguntas));
                }

                ModelState.AddModelError("", "Error interno al actualizar la pregunta y sus opciones.");
            }

            var categorias = await _configuracionService.ObtenerTodasLasCategoriasAsync();
            var complejidades = await _configuracionService.ObtenerTodasLasComplejidadesAsync();

            ViewBag.IdCategoria = new SelectList(categorias, "IdCategoria", "Nombre", model.Pregunta.IdCategoria);
            ViewBag.IdComplejidad = new SelectList(complejidades, "IdComplejidad", "Nombre", model.Pregunta.IdComplejidad);
            return View(model);
        }

        /// <summary>
        /// Elimina de forma asíncrona una pregunta específica y todas sus opciones asociadas.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            await _configuracionService.EliminarPreguntaConOpcionesAsync(id);
            return RedirectToAction(nameof(Preguntas));
        }

        #endregion
    }
}