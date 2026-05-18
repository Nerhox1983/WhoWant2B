using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoWant2B.Data;
using WhoWant2B.Models;

namespace WhoWant2B.Controllers.Configuracion
{
    
    public class ConfiguracionController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ConfiguracionController"/>.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="ApplicationDbContext"/>) utilizado para interactuar con los datos.</param>
        public ConfiguracionController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Muestra la vista principal de la configuración.
        /// </summary>
        /// <returns>El resultado de la acción que renderiza la vista <see cref="Index"/>.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra la vista con el formulario para crear una nueva categoría.
        /// </summary>
        /// <returns>El resultado de la acción que renderiza la vista <see cref="CrearCategoria"/>.</returns>
        public IActionResult CrearCategoria()
        {
            return View();
        }

        /// <summary>
        /// Procesa de forma asíncrona los datos del formulario para registrar una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="categoria">El modelo <see cref="Categoria_model"/> que contiene los datos de la categoría enviados desde el formulario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// Si el modelo es válido, redirige a la acción <see cref="Categorias"/>; 
        /// de lo contrario, vuelve a mostrar la vista actual con el modelo para exhibir los errores de validación.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCategoria(Categoria_model categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        /// <summary>
        /// Muestra de forma asíncrona la vista de edición para una categoría específica basada en su identificador.
        /// </summary>
        /// <param name="id">El identificador único (ID) de la categoría que se desea editar. Puede ser nulo.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="NotFoundResult"/> si el ID es nulo o si la categoría no existe en la base de datos; 
        /// de lo contrario, devuelve un <see cref="ViewResult"/> con el modelo de la categoría encontrada.
        /// </returns>
        [HttpGet("Configuracion/EditarCategoria/{id:int}")]
        public async Task<IActionResult> EditarCategoria(int? id)
        {
            if (id == null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return NotFound();

            return View(categoria);
        }

        /// <summary>
        /// Procesa de forma asíncrona los cambios guardados en el formulario para actualizar una categoría existente.
        /// </summary>
        /// <param name="id">El identificador único de la categoría que se recibe a través de la URL de la ruta.</param>
        /// <param name="categoria">El modelo <see cref="Categoria_model"/> con los datos actualizados de la categoría desde el formulario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="NotFoundResult"/> si el ID de la ruta no coincide con el ID del modelo;
        /// si el modelo es válido, actualiza el registro y redirige a la acción <see cref="Categorias"/>;
        /// de lo contrario, vuelve a mostrar la vista con los errores de validación correspondientes.
        /// </returns>
        [HttpPost("Configuracion/EditarCategoria/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCategoria(int id, Categoria_model categoria)
        {
            if (id != categoria.IdCategoria) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categorias));
            }
            return View(categoria);
        }

        /// <summary>
        /// Elimina de forma asíncrona una categoría específica de la base de datos basada en su identificador.
        /// </summary>
        /// <param name="id">El identificador único (ID) de la categoría que se desea eliminar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="RedirectToActionResult"/> que redirige al usuario 
        /// a la acción <see cref="Categorias"/>, asegurando la actualización de la vista de la lista.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Categorias));
        }

        /// <summary>
        /// Obtiene de forma asíncrona la lista de preguntas paginada de 10 en 10 usando LINQ nativo.
        /// </summary>        
        public async Task<IActionResult> Preguntas(int? page)
        {
            int numeroPagina = page ?? 1;
            int cantidadPorPagina = 10;

            // 1. Contamos el total de preguntas en la base de datos
            int totalPreguntas = await _context.Preguntas.CountAsync();

            // 2. Traemos solo las 10 preguntas que corresponden a la página actual
            var preguntas = await _context.Preguntas
                .Include(p => p.Categoria)
                .Include(p => p.Complejidad)
                .OrderBy(p => p.IdPregunta)
                .Skip((numeroPagina - 1) * cantidadPorPagina) // Se salta las de las páginas anteriores
                .Take(cantidadPorPagina)                      // Toma solo las 10 siguientes
                .ToListAsync();

            // 3. Pasamos los datos de control de paginación a la vista usando el ViewBag
            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalPreguntas / cantidadPorPagina);
            ViewBag.TotalRegistros = totalPreguntas;

            return View(preguntas);
        }


        /// <summary>
        /// Muestra de forma asíncrona la vista para crear una nueva pregunta, preparando los listados de selección y la estructura inicial de las opciones.
        /// </summary>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene el <see cref="IActionResult"/> que renderiza la vista, 
        /// enviando un <see cref="PreguntaConOpciones_viewModel"/> inicializado con 4 opciones vacías y 
        /// las listas de categorías y complejidades cargadas en el <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary"/> (ViewBag).
        /// </returns>
        public async Task<IActionResult> CrearPregunta()
        {
            ViewBag.IdCategoria = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categorias.ToListAsync(), "IdCategoria", "Nombre");
            ViewBag.IdComplejidad = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Complejidades.ToListAsync(), "IdComplejidad", "Nombre");

            var viewModel = new PreguntaConOpciones_viewModel();
            for (int i = 0; i < 4; i++)
            {
                viewModel.Opciones.Add(new Opcion_model { Texto = string.Empty });
            }

            return View(viewModel);
        }

        /// <summary>
        /// Procesa de forma asíncrona la creación de una nueva pregunta junto con su listado de opciones asociadas bajo una transacción atómica.
        /// </summary>
        /// <param name="model">El modelo <see cref="PreguntaConOpciones_viewModel"/> que agrupa los datos de la pregunta y sus respectivas opciones enviados desde el formulario.</param>
        /// <param name="CorrectaIndex">El índice basado en cero (0-3) seleccionado en el formulario que indica cuál de las opciones es la respuesta correcta.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// Si el modelo es válido y la transacción se completa con éxito, confirma los cambios y redirige a la acción <see cref="Preguntas"/>; 
        /// de lo contrario (fallo en validación o excepción en base de datos), realiza un Rollback de la transacción, recarga los listados de selección en el <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary"/> (ViewBag) manteniendo las selecciones previas, y vuelve a mostrar la vista con el modelo actual.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearPregunta(PreguntaConOpciones_viewModel model, int CorrectaIndex)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Preguntas.Add(model.Pregunta);
                    await _context.SaveChangesAsync();

                    for (int i = 0; i < model.Opciones.Count; i++)
                    {
                        model.Opciones[i].IdPregunta = model.Pregunta.IdPregunta;
                        model.Opciones[i].Valida = (i == CorrectaIndex);
                        _context.Opciones.Add(model.Opciones[i]);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Preguntas));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Error interno al guardar la nueva pregunta con sus opciones.");
                }
            }

            ViewBag.IdCategoria = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categorias.ToListAsync(), "IdCategoria", "Nombre", model.Pregunta.IdCategoria);
            ViewBag.IdComplejidad = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Complejidades.ToListAsync(), "IdComplejidad", "Nombre", model.Pregunta.IdComplejidad);
            return View(model);
        }

        /// <summary>
        /// Muestra de forma asíncrona la vista de edición para una pregunta específica, cargando sus opciones relacionadas y los listados de selección.
        /// </summary>
        /// <param name="id">El identificador único (ID) de la pregunta que se desea editar. Puede ser nulo.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="NotFoundResult"/> si el ID es nulo o si la pregunta no existe en la base de datos; 
        /// de lo contrario, devuelve un <see cref="ViewResult"/> con el <see cref="PreguntaConOpciones_viewModel"/> cargado con la pregunta, 
        /// sus opciones asociadas y los listados de categorías y complejidades con sus respectivos elementos preseleccionados en el <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary"/> (ViewBag).
        /// </returns>
        public async Task<IActionResult> EditarPregunta(int? id)
        {
            if (id == null) return NotFound();

            var pregunta = await _context.Preguntas
                .Include(p => p.Opciones)
                .FirstOrDefaultAsync(p => p.IdPregunta == id);

            if (pregunta == null) return NotFound();

            ViewBag.IdCategoria = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categorias.ToListAsync(), "IdCategoria", "Nombre", pregunta.IdCategoria);
            ViewBag.IdComplejidad = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Complejidades.ToListAsync(), "IdComplejidad", "Nombre", pregunta.IdComplejidad);

            var viewModel = new PreguntaConOpciones_viewModel
            {
                Pregunta = pregunta,
                Opciones = pregunta.Opciones.ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Procesa de forma asíncrona las modificaciones de una pregunta existente y sus opciones asociadas bajo una transacción atómica.
        /// </summary>
        /// <param name="id">El identificador único (ID) de la pregunta recibido a través de los parámetros de la ruta de la solicitud.</param>
        /// <param name="model">El modelo <see cref="PreguntaConOpciones_viewModel"/> que contiene los datos actualizados de la pregunta y su colección de opciones.</param>
        /// <param name="CorrectaIndex">El índice basado en cero (0-3) proveniente del formulario que define cuál opción se debe marcar como la respuesta correcta.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="NotFoundResult"/> si el ID de la ruta no coincide con el ID de la pregunta en el modelo;
        /// si el modelo es válido y los cambios se guardan con éxito en la base de datos, confirma la transacción y redirige a la acción <see cref="Preguntas"/>;
        /// en caso de error de validación o excepción interna, revierte los cambios (Rollback), recarga las listas desplegables en el <see cref="Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary"/> (ViewBag) y vuelve a renderizar la vista con los datos actuales.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarPregunta(int id, PreguntaConOpciones_viewModel model, int CorrectaIndex)
        {
            if (id != model.Pregunta.IdPregunta) return NotFound();

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Preguntas.Update(model.Pregunta);

                    for (int i = 0; i < model.Opciones.Count; i++)
                    {
                        model.Opciones[i].Valida = (i == CorrectaIndex);
                        _context.Opciones.Update(model.Opciones[i]);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Preguntas));
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "Error interno al actualizar la pregunta y sus opciones.");
                }
            }

            ViewBag.IdCategoria = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Categorias.ToListAsync(), "IdCategoria", "Nombre", model.Pregunta.IdCategoria);
            ViewBag.IdComplejidad = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await _context.Complejidades.ToListAsync(), "IdComplejidad", "Nombre", model.Pregunta.IdComplejidad);
            return View(model);
        }

        /// <summary>
        /// Elimina de forma asíncrona una pregunta específica y todas sus opciones asociadas bajo una transacción atómica para preservar la integridad referencial.
        /// </summary>
        /// <param name="id">El identificador único (ID) de la pregunta que se desea eliminar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. 
        /// El resultado de la tarea contiene un <see cref="RedirectToActionResult"/> que redirige al usuario a la acción <see cref="Preguntas"/>, 
        /// independientemente de si la pregunta fue encontrada y eliminada con éxito o si ocurrió un error en el proceso.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> EliminarPregunta(int id)
        {
            var pregunta = await _context.Preguntas
                .Include(p => p.Opciones)
                .FirstOrDefaultAsync(p => p.IdPregunta == id);

            if (pregunta != null)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (pregunta.Opciones != null && pregunta.Opciones.Any())
                    {
                        _context.Opciones.RemoveRange(pregunta.Opciones);
                    }

                    _context.Preguntas.Remove(pregunta);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();                    
                    ModelState.AddModelError("", "No se pudo eliminar la pregunta debido a un error interno.");
                }
            }

            return RedirectToAction(nameof(Preguntas));
        }

        /// <summary>
        /// Obtiene de forma asíncrona la lista de categorías y muestra su vista correspondiente.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado de la tarea contiene el <see cref="IActionResult"/> que renderiza la vista con el listado de categorías.</returns>
        public async Task<IActionResult> Categorias(int? page)
        {
            int numeroPagina = page ?? 1;
            int cantidadPorPagina = 10;
            
            int totalCategorias = await _context.Categorias.CountAsync();
           
            var categorias = await _context.Categorias
                .OrderBy(c => c.IdCategoria)
                .Skip((numeroPagina - 1) * cantidadPorPagina) 
                .Take(cantidadPorPagina)                      
                .ToListAsync();

            ViewBag.PaginaActual = numeroPagina;
            ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalCategorias / cantidadPorPagina);
            ViewBag.TotalRegistros = totalCategorias;

            return View(categorias);
        }
    }
}
