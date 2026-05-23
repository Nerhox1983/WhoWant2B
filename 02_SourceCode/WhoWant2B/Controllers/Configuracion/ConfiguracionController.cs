using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Models;

namespace WhoWant2B.Controllers.Configuracion
{
    /// <summary>
    /// Controlador administrativo encargado de gestionar el mantenimiento (CRUD) de las entidades 
    /// base del juego: Categorías y Preguntas con sus respectivas opciones.
    /// </summary>
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionService _configuracionService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ConfiguracionController"/> mediante inyección de dependencias.
        /// </summary>
        /// <param name="configuracionService">El servicio de negocio para la gestión de configuraciones del sistema.</param>
        public ConfiguracionController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }

        /// <summary>
        /// Muestra el panel principal de administración desde donde se puede navegar a la gestión de categorías o preguntas.
        /// </summary>
        /// <returns>La vista de inicio del panel de configuración.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region CRUD: Categorías

        /// <summary>
        /// Obtiene la lista de categorías registradas en el sistema aplicando una lógica de paginación.
        /// </summary>
        /// <param name="page">Número de página solicitado para la visualización de datos.</param>
        /// <returns>Una vista que contiene la colección de categorías para la página especificada.</returns>
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
        /// Muestra el formulario vacío para el registro de una nueva categoría de preguntas.
        /// </summary>
        /// <returns>La vista de creación de categorías.</returns>
        [HttpGet]
        public IActionResult CrearCategoria()
        {
            return View();
        }

        /// <summary>
        /// Procesa la información enviada desde el formulario de creación para persistir una nueva categoría.
        /// </summary>
        /// <param name="categoria">Modelo que contiene los datos de la categoría a registrar.</param>
        /// <returns>Redirección al listado de categorías si el proceso es exitoso; de lo contrario, retorna la misma vista con errores de validación.</returns>
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
        /// Recupera los datos de una categoría específica para su edición.
        /// </summary>
        /// <param name="id">Identificador único de la categoría.</param>
        /// <returns>La vista de edición con los datos cargados, o un resultado <see cref="NotFoundResult"/> si el ID es nulo o no existe.</returns>
        [HttpGet]
        public async Task<IActionResult> EditarCategoria(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _configuracionService.ObtenerCategoriaPorIdAsync(id.Value);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        /// <summary>
        /// Procesa y actualiza los cambios realizados en una categoría existente.
        /// </summary>
        /// <param name="id">Identificador de la categoría (para verificación de integridad).</param>
        /// <param name="categoria">Modelo con los datos actualizados.</param>
        /// <returns>Redirección al listado de categorías tras la actualización o la vista actual si hay errores en el modelo.</returns>
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
        /// Elimina físicamente una categoría del sistema.
        /// </summary>
        /// <param name="id">Identificador de la categoría a eliminar.</param>
        /// <returns>Redirección al listado de categorías una vez completada la acción.</returns>
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
        /// Obtiene el listado general de preguntas del banco de trivias con soporte para paginación de 10 registros.
        /// </summary> 
        /// <param name="page">Número de página actual a consultar.</param>
        /// <returns>Vista con la lista de preguntas paginada.</returns>
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
        /// Prepara el entorno para la creación de una nueva pregunta, inicializando las listas de selección y las opciones vacías.
        /// </summary>
        /// <returns>Vista de creación con un ViewModel que contiene la pregunta y 4 opciones inicializadas.</returns>
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
        /// Realiza la persistencia de una pregunta y sus cuatro opciones relacionadas en una sola operación transaccional.
        /// </summary>
        /// <param name="model">ViewModel que agrupa la entidad Pregunta y su lista de Opciones.</param>
        /// <param name="CorrectaIndex">Índice (0-3) que identifica cuál de las opciones marcadas es la respuesta válida.</param>
        /// <returns>Redirección al listado de preguntas si es exitoso; de lo contrario, recarga el formulario con los mensajes de error.</returns>
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
        /// Carga los datos de una pregunta existente y sus opciones asociadas para su modificación.
        /// </summary>
        /// <param name="id">Identificador de la pregunta a editar.</param>
        /// <returns>Vista de edición con el modelo cargado, o <see cref="NotFoundResult"/> si el registro no se encuentra.</returns>
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
        /// Actualiza la información de una pregunta y sincroniza sus opciones asociadas en la base de datos.
        /// </summary>
        /// <param name="id">Identificador de la pregunta para validación de ruta.</param>
        /// <param name="model">ViewModel con los datos modificados de la pregunta y opciones.</param>
        /// <param name="CorrectaIndex">Nuevo índice de la opción marcada como correcta.</param>
        /// <returns>Redirección al listado general de preguntas tras la operación exitosa.</returns>
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
        /// Elimina una pregunta del banco de datos. Las opciones asociadas se eliminan en cascada según la lógica del servicio.
        /// </summary>
        /// <param name="id">Identificador único de la pregunta a remover.</param>
        /// <returns>Redirección al listado de preguntas actualizado.</returns>
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