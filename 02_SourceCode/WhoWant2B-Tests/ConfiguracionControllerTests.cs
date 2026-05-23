using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoWant2B.Controllers.Configuracion;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;
using WhoWant2B.Infrastructure.Services;
using WhoWant2B.Models;

namespace WhoWant2B.Tests
{
    /// <summary>
    /// Pruebas unitarias para el controlador de configuración, encargado de la gestión de categorías y preguntas.
    /// </summary>
    public class ConfiguracionControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ConfiguracionService _service;
        private readonly ConfiguracionController _controller;
        
        private const string ErrorMensajeNombreObligatorio = "El nombre es obligatorio";
        private const string ErrorMensajeTextoPreguntaObligatorio = "El texto de la pregunta es obligatorio";

        public ConfiguracionControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();            
            _service = new ConfiguracionService(_context);
            _controller = new ConfiguracionController(_service);
        }

        /// <summary>
        /// Libera los recursos de la base de datos en memoria después de cada prueba.
        /// </summary>
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region Pruebas de Categorías
        /// <summary>
        /// Verifica que si el modelo de categoría es inválido, se retorne la vista con el mismo modelo.
        /// </summary>
        [Fact]
        public async Task CrearCategoria_Post_ModelStateInvalido_RetornaVistaConModelo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            controller.ModelState.AddModelError(nameof(Categoria_model.Nombre), ErrorMensajeNombreObligatorio);

            var modeloInvalido = new Categoria_model { IdCategoria = 0, Nombre = "", Descripcion = "" };

            // Act
            var result = await controller.CrearCategoria(modeloInvalido) as ViewResult;

            // Assert
            result.Should().NotBeNull();
            result.Model.Should().BeEquivalentTo(modeloInvalido);
        }

        /// <summary>
        /// Verifica que la acción Categorías retorne la lista paginada correctamente.
        /// </summary>
        [Fact]
        public async Task Categorias_RetornaVistaConListaPaginada()
        {
            _context.Categorias.AddRange(new List<Categoria_model>
            {
                new Categoria_model { IdCategoria = 1, Nombre = "Ciencia", Descripcion = "Test" },
                new Categoria_model { IdCategoria = 2, Nombre = "Historia", Descripcion = "Test" }
            });
            await _context.SaveChangesAsync();

            var result = await _controller.Categorias(page: 1);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeAssignableTo<IEnumerable<Categoria_model>>().Subject;

            model.Should().HaveCount(2);
            ((int)_controller.ViewBag.PaginaActual).Should().Be(1);
            ((int)_controller.ViewBag.TotalPaginas).Should().Be(1);
        }

        /// <summary>
        /// Verifica que una categoría válida se agregue correctamente y redirija a la lista.
        /// </summary>
        [Fact]
        public async Task CrearCategoria_Post_ModeloValido_AgregaCategoriaYRedirige()
        {
            var nuevaCategoria = new Categoria_model { IdCategoria = 3, Nombre = "Geografía", Descripcion = "Test" };

            var result = await _controller.CrearCategoria(nuevaCategoria);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be(nameof(_controller.Categorias));

            var categoriaEnDb = await _context.Categorias.FindAsync(3);
            categoriaEnDb.Should().NotBeNull();
            categoriaEnDb!.Nombre.Should().Be("Geografía");
        }

        /// <summary>
        /// Verifica el retorno de NotFound cuando se busca una categoría inexistente.
        /// </summary>
        [Fact]
        public async Task EditarCategoria_Get_IdNoExistente_RetornaNotFound()
        {
            var result = await _controller.EditarCategoria(id: 99);

            result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// Verifica que la actualización de una categoría sea exitosa y redirija.
        /// </summary>
        [Fact]
        public async Task EditarCategoria_Post_IdValido_ActualizaYRedirige()
        {
            var categoria = new Categoria_model { IdCategoria = 1, Nombre = "Cine", Descripcion = "Test" };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            categoria.Nombre = "Cine y TV";

            var result = await _controller.EditarCategoria(1, categoria);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be(nameof(_controller.Categorias));

            var modificadoEnDb = await _context.Categorias.FindAsync(1);
            modificadoEnDb!.Nombre.Should().Be("Cine y TV");
        }

        /// <summary>
        /// Verifica que el controlador maneje correctamente un ID nulo en la edición.
        /// </summary>
        [Fact]
        public async Task EditarCategoria_Get_IdEsNull_RetornaNotFound()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            
            var result = await controller.EditarCategoria(null);

            
            result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// Verifica el comportamiento cuando se intenta editar una categoría que no existe en BD.
        /// </summary>
        [Fact]
        public async Task EditarCategoria_Get_CategoriaNoExiste_RetornaNotFound()
        {
           
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);
            
            var result = await controller.EditarCategoria(999);
            
            result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// Verifica que la eliminación de una categoría funcione y redirija.
        /// </summary>
        [Fact]
        public async Task EliminarCategoria_Post_EliminaRegistroYRedirige()
        {
            var categoria = new Categoria_model { IdCategoria = 5, Nombre = "Deportes", Descripcion = "Test" };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            var result = await _controller.EliminarCategoria(5);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be(nameof(_controller.Categorias));

            var eliminado = await _context.Categorias.FindAsync(5);
            eliminado.Should().BeNull();
        }

        /// <summary>
        /// Verifica que al intentar eliminar un ID inexistente se mantenga el flujo de redirección.
        /// </summary>
        [Fact]
        public async Task EliminarCategoria_Post_IdNoExiste_RetornaRedirect()
        {            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);
            
            var result = await controller.EliminarCategoria(888);

            var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
            redirectResult.ActionName.Should().Be(nameof(ConfiguracionController.Categorias));
        }

        #endregion

        #region Pruebas de Preguntas
        /// <summary>
        /// Verifica que la edición de pregunta cargue correctamente el modelo y las listas de selección.
        /// </summary>
        [Fact]
        public async Task EditarPregunta_Get_IdExiste_RetornaVistaConModeloYViewBag()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);

            var categoria = new Categoria_model { IdCategoria = 1, Nombre = "General", Descripcion = "Test" };
            context.Categorias.Add(categoria);

            var tipoComplejidad = typeof(Pregunta_model).GetProperty("Complejidad")?.PropertyType;
            if (tipoComplejidad != null)
            {
                var complejidadInstancia = Activator.CreateInstance(tipoComplejidad);
                tipoComplejidad.GetProperty("IdComplejidad")?.SetValue(complejidadInstancia, 1);
                tipoComplejidad.GetProperty("Nombre")?.SetValue(complejidadInstancia, "Fácil");
                tipoComplejidad.GetProperty("Descripcion")?.SetValue(complejidadInstancia, "Test");
                context.Add(complejidadInstancia);
            }
            await context.SaveChangesAsync();

            var pregunta = new Pregunta_model { IdPregunta = 1, Texto = "¿Pregunta Original?", IdCategoria = 1, IdComplejidad = 1 };
            context.Preguntas.Add(pregunta);
            await context.SaveChangesAsync();

            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var result = await controller.EditarPregunta(1) as ViewResult;

            result.Should().NotBeNull();

            var categorias = (result.ViewData["Categorias"] ?? result.ViewData["IdCategoria"]) as Microsoft.AspNetCore.Mvc.Rendering.SelectList;
            categorias.Should().NotBeNull();

            result.Model.Should().NotBeNull();
        }

        /// <summary>
        /// Verifica la lógica de paginación por defecto para el listado de preguntas.
        /// </summary>
        [Fact]
        public async Task Preguntas_SinEspecificarPagina_RetornaPrimerasDiezPreguntasYViewBag()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);

            var categoria = new Categoria_model { IdCategoria = 1, Nombre = "General", Descripcion = "Test" };
            context.Categorias.Add(categoria);

            var tipoComplejidad = typeof(Pregunta_model).GetProperty("Complejidad")?.PropertyType;
            if (tipoComplejidad != null)
            {
                var complejidadInstancia = Activator.CreateInstance(tipoComplejidad);
                tipoComplejidad.GetProperty("IdComplejidad")?.SetValue(complejidadInstancia, 1);
                tipoComplejidad.GetProperty("Nombre")?.SetValue(complejidadInstancia, "Fácil");
                tipoComplejidad.GetProperty("Descripcion")?.SetValue(complejidadInstancia, "Test");
                context.Add(complejidadInstancia);
            }

            await context.SaveChangesAsync();

            for (int i = 1; i <= 12; i++)
            {
                context.Preguntas.Add(new Pregunta_model
                {
                    IdPregunta = i,
                    Texto = $"Pregunta {i}",
                    IdCategoria = 1,
                    IdComplejidad = 1
                });
            }
            await context.SaveChangesAsync();

            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var result = await controller.Preguntas(null) as ViewResult;

            result.Should().NotBeNull();

            var model = result.Model as IEnumerable<Pregunta_model>;
            model.Should().NotBeNull();
            model.Should().HaveCount(10);

            ((int)controller.ViewBag.PaginaActual).Should().Be(1);
            ((int)controller.ViewBag.TotalPaginas).Should().Be(2);
            ((int)controller.ViewBag.TotalRegistros).Should().Be(12);
        }

        /// <summary>
        /// Verifica que la navegación a la segunda página de preguntas salte los registros correctos.
        /// </summary>
        [Fact]
        public async Task Preguntas_SegundaPagina_SaltaElementosYRetornaRegistrosRestantes()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);

            var categoria = new Categoria_model { IdCategoria = 1, Nombre = "General", Descripcion = "Test" };
            context.Categorias.Add(categoria);

            var tipoComplejidad = typeof(Pregunta_model).GetProperty("Complejidad")?.PropertyType;
            if (tipoComplejidad != null)
            {
                var complejidadInstancia = Activator.CreateInstance(tipoComplejidad);
                tipoComplejidad.GetProperty("IdComplejidad")?.SetValue(complejidadInstancia, 1);
                tipoComplejidad.GetProperty("Nombre")?.SetValue(complejidadInstancia, "Fácil");
                tipoComplejidad.GetProperty("Descripcion")?.SetValue(complejidadInstancia, "Test");
                context.Add(complejidadInstancia);
            }

            await context.SaveChangesAsync();

            for (int i = 1; i <= 12; i++)
            {
                context.Preguntas.Add(new Pregunta_model
                {
                    IdPregunta = i,
                    Texto = $"Pregunta {i}",
                    IdCategoria = 1,
                    IdComplejidad = 1
                });
            }
            await context.SaveChangesAsync();

            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var result = await controller.Preguntas(2) as ViewResult;

            result.Should().NotBeNull();

            var model = result.Model as IEnumerable<Pregunta_model>;
            model.Should().NotBeNull();
            model.Should().HaveCount(2);

            ((int)controller.ViewBag.PaginaActual).Should().Be(2);
            ((int)controller.ViewBag.TotalPaginas).Should().Be(2);
            ((int)controller.ViewBag.TotalRegistros).Should().Be(12);
        }

        /// <summary>
        /// Verifica que el formulario de creación de pregunta inicialice las 4 opciones requeridas.
        /// </summary>
        [Fact]
        public async Task CrearPregunta_Get_InicializaViewModelConCuatroOpcionesYViewBag()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var result = await controller.CrearPregunta() as ViewResult;

            result.Should().NotBeNull();

            var categories = (result.ViewData["Categorias"] ??
                             result.ViewData["IdCategoria"] ??
                             result.ViewData["ListaCategorias"]) as Microsoft.AspNetCore.Mvc.Rendering.SelectList;

            var complejidades = (result.ViewData["Complejidades"] ??
                                 result.ViewData["IdComplejidad"] ??
                                 result.ViewData["ListaComplejidades"]) as Microsoft.AspNetCore.Mvc.Rendering.SelectList;

            categories.Should().NotBeNull("el controlador debería asignar un SelectList para las Categorías en el ViewBag/ViewData");
            complejidades.Should().NotBeNull("el controlador debería asignar un SelectList para las Complejidades en el ViewBag/ViewData");

            result.Model.Should().NotBeNull();

            var propiedadOpciones = result.Model.GetType().GetProperty("Opciones");
            propiedadOpciones.Should().NotBeNull("el modelo de la vista debería tener una propiedad llamada 'Opciones'");

            var opcionesValue = propiedadOpciones.GetValue(result.Model) as System.Collections.IEnumerable;
            opcionesValue.Should().NotBeNull();

            var listaOpciones = opcionesValue.Cast<object>();
            listaOpciones.Should().HaveCount(4);
        }

        /// <summary>
        /// Verifica que una pregunta nueva con sus opciones se guarde correctamente.
        /// </summary>
        [Fact]
        public async Task CrearPregunta_Post_DatosValidos_GuardaPreguntaYSuOpcionCorrecta()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var viewModel = new PreguntaConOpciones_viewModel
            {
                Pregunta = new Pregunta_model { IdPregunta = 1, Texto = "¿De qué color es el cielo?", IdCategoria = 1, IdComplejidad = 1 },
                Opciones = new List<Opcion_model>
                {
                    new Opcion_model { Texto = "Rojo" },
                    new Opcion_model { Texto = "Azul" },
                    new Opcion_model { Texto = "Verde" },
                    new Opcion_model { Texto = "Amarillo" }
                }
            };

            var result = await controller.CrearPregunta(viewModel, CorrectaIndex: 1);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Preguntas), redirectResult.ActionName);

            var preguntaEnDb = await context.Preguntas.Include(p => p.Opciones).FirstOrDefaultAsync(p => p.IdPregunta == 1);
            Assert.NotNull(preguntaEnDb);
            Assert.Equal(4, preguntaEnDb.Opciones.Count);
            Assert.True(preguntaEnDb.Opciones.ElementAt(1).Valida);
            Assert.False(preguntaEnDb.Opciones.ElementAt(0).Valida);
        }

        /// <summary>
        /// Verifica que si falla la validación al editar una pregunta, se retorne el ViewModel original.
        /// </summary>
        [Fact]
        public async Task EditarPregunta_Post_ModelStateInvalido_RetornaVistaConModelo()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            controller.ModelState.AddModelError("Pregunta.Texto", ErrorMensajeTextoPreguntaObligatorio);

            var viewModelInvalido = new PreguntaConOpciones_viewModel
            {
                Pregunta = new Pregunta_model { IdPregunta = 1, Texto = "", IdCategoria = 1, IdComplejidad = 1 },
                Opciones = new List<Opcion_model>()
            };

            // Act
            var result = await controller.EditarPregunta(1, viewModelInvalido, CorrectaIndex: 0) as ViewResult;

            // Assert
            result.Should().NotBeNull("el controlador debería retornar la ViewResult original al fallar el ModelState");
            result.Model.Should().BeEquivalentTo(viewModelInvalido);
        }

        /// <summary>
        /// Verifica el manejo de errores para IDs nulos o inexistentes en la carga de edición de preguntas.
        /// </summary>
        [Fact]
        public async Task EditarPregunta_Get_IdEsNullOIdNoExiste_RetornaNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            // Act
            var resultNull = await controller.EditarPregunta(null);
            var resultNoExiste = await controller.EditarPregunta(999);

            // Assert
            resultNull.Should().BeOfType<NotFoundResult>();
            resultNoExiste.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// Verifica que el ID de la URL coincida con el ID del modelo en el POST de edición.
        /// </summary>
        [Fact]
        public async Task EditarPregunta_Post_IdNoCoincideConModelo_RetornaNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            // Act
            var result = await controller.EditarPregunta(99);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        /// <summary>
        /// Verifica que al eliminar una pregunta, se borren también en cascada sus opciones asociadas.
        /// </summary>
        [Fact]
        public async Task EliminarPregunta_Post_EliminaPreguntaYOpcionesAsociadas()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            using var context = new ApplicationDbContext(options);
            var service = new ConfiguracionService(context);
            var controller = new ConfiguracionController(service);

            var pregunta = new Pregunta_model { IdPregunta = 10, Texto = "Pregunta a borrar" };
            var opciones = new List<Opcion_model>
            {
                new Opcion_model { IdOpcion = 1, IdPregunta = 10, Texto = "Op1" },
                new Opcion_model { IdOpcion = 2, IdPregunta = 10, Texto = "Op2" }
            };

            context.Preguntas.Add(pregunta);
            context.Opciones.AddRange(opciones);
            await context.SaveChangesAsync();

            var result = await controller.EliminarPregunta(10);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(controller.Preguntas), redirectResult.ActionName);

            (await context.Preguntas.FindAsync(10)).Should().BeNull();

            var opcionesRestantes = await context.Opciones.Where(o => o.IdPregunta == 10).ToListAsync();
            opcionesRestantes.Should().BeEmpty();
        }
        #endregion
    }
}