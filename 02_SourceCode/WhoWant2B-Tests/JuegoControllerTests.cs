using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using WhoWant2B.Controllers;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;
using WhoWant2B.Infrastructure.Services;

namespace WhoWant2B.Tests
{
    /// <summary>
    /// Pruebas unitarias para el controlador del Juego, encargado de la lógica de trivia,
    /// autenticación de jugadores y persistencia de resultados.
    /// </summary>
    public class JuegoControllerTests
    {
        private const string SessionIdJugador = "IdJugadorActual";
        private const string ErrorPasswordIncorrecta = "La contraseña es incorrecta para este jugador.";
        private const string RouteJuegoIniciado = "juegoIniciado";
        private const string RouteGanado = "ganado";
        private const string RoutePerdiste = "perdiste";
        private const string RouteRetirado = "retirado";
        private const string RoutePuntaje = "puntaje";
        private const string TempDataError = "ErrorMessage";

        #region Métodos de Soporte / Mocks

        /// <summary>
        /// Crea un contexto de base de datos en memoria único para una prueba.
        /// </summary>
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        /// <summary>
        /// Simula el contexto del controlador inyectando un mock de Sesión con valores predefinidos.
        /// </summary>
        private ControllerContext GetMockControllerContext(string sessionKey, string sessionValue)
        {
            var mockSession = new Mock<ISession>();
            var valueBytes = Encoding.UTF8.GetBytes(sessionValue);

            mockSession
                .Setup(s => s.TryGetValue(sessionKey, out It.Ref<byte[]?>.IsAny))
                .Callback(new OutAction<string, byte[]?>((string key, out byte[]? val) => val = valueBytes))
                .Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);

            return new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
        }

        private delegate void OutAction<T1, T2>(T1 outVal1, out T2 outVal2);
        
        /// <summary>
        /// Instancia el controlador inyectando los servicios reales con el contexto InMemory.
        /// </summary>
        private JuegoController CrearControllerConServiciosReales(ApplicationDbContext context, ISecurityService securityService)
        {
            var juegoService = new JuegoService(context);
            var autenticacionService = new AutenticacionService(context);

            return new JuegoController(juegoService, autenticacionService, securityService);
        }

        #endregion

        #region Pruebas: VerificarUsuarioExistente

        [Fact]
        public async Task VerificarUsuarioExistente_UsuarioExiste_DevuelveJsonConExisteTrue()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var usuarioPrueba = new Usuario_model
            {
                IdUsuario = 1,
                Login = "testuser",
                NombreReal = "Juan Perez",
                Password = Encoding.UTF8.GetBytes("hashed_password"),
                IdRol = 2
            };
            context.Usuarios.Add(usuarioPrueba);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var result = await controller.VerificarUsuarioExistente("Juan Perez");
            var jsonResult = Assert.IsType<JsonResult>(result);
            var data = jsonResult.Value;
            Assert.NotNull(data);

            var propiedadExiste = data.GetType().GetProperty("existe");
            var propiedadLogin = data.GetType().GetProperty("loginExistente");

            var existeValue = propiedadExiste?.GetValue(data, null);
            var loginValue = propiedadLogin?.GetValue(data, null);

            Assert.NotNull(existeValue);
            Assert.True((bool)existeValue);
            Assert.Equal("testuser", loginValue?.ToString());
        }

        /// <summary>
        /// Verifica que el servicio retorne false si el nombre del jugador es nuevo.
        /// </summary>
        [Fact]
        public async Task VerificarUsuarioExistente_UsuarioNoExiste_DevuelveJsonConExisteFalse()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var result = await controller.VerificarUsuarioExistente("Usuario Fantasma");
            var jsonResult = Assert.IsType<JsonResult>(result);
            var data = jsonResult.Value;
            Assert.NotNull(data);

            var propiedadExiste = data.GetType().GetProperty("existe");
            var existeValue = propiedadExiste?.GetValue(data, null);

            Assert.NotNull(existeValue);
            Assert.False((bool)existeValue);
        }

        #endregion

        #region Pruebas: GuardarUsuario

        [Fact]
        public async Task GuardarUsuario_DatosValidos_InsertaUsuarioYRedirige()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var hashSimulado = new byte[] { 0x20, 0x20, 0x20, 0x20 };
            mockSecurity
                .Setup(s => s.GetHash(It.IsAny<string>()))
                .Returns(hashSimulado);

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var mockSession = new Mock<Microsoft.AspNetCore.Http.ISession>();
            var sessionDictionary = new Dictionary<string, byte[]>();

            mockSession
                .Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) => sessionDictionary[key] = value);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = mockSession.Object;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GuardarUsuario(
                login: "nuevojugador",
                password: "password123",
                idRol: 2,
                nombreReal: "Carlos Pérez"
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);

            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RouteJuegoIniciado]));
            
            var usuarioEnDb = await context.Usuarios.FirstOrDefaultAsync(u => u.Login == "nuevojugador");

            var actualNombreReal = usuarioEnDb?.NombreReal;
            var actualIdRol = usuarioEnDb?.IdRol;

            Assert.NotNull(usuarioEnDb);
            Assert.Equal("Carlos Pérez", actualNombreReal);
            Assert.Equal(2, actualIdRol);
        }

        /// <summary>
        /// Verifica que un usuario existente pueda hacer login si proporciona la contraseña correcta.
        /// </summary>
        [Fact]
        public async Task GuardarUsuario_UsuarioExistentePasswordCorrecta_IniciaSesionYRedirige()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var hashDummy = new byte[] { 0x1, 0x2, 0x3, 0x4 };
            mockSecurity
                .Setup(s => s.VerifyHash(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(true);

            var usuarioExistente = new Usuario_model
            {
                IdUsuario = 5,
                Login = "carlosp",
                Password = hashDummy,
                IdRol = 2,
                NombreReal = "Carlos Pérez"
            };
            context.Usuarios.Add(usuarioExistente);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var mockSession = new Mock<Microsoft.AspNetCore.Http.ISession>();
            var httpContext = new DefaultHttpContext { Session = mockSession.Object };
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var result = await controller.GuardarUsuario("carlosp", "password123", 2, "Carlos Pérez");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RouteJuegoIniciado]));
        }

        /// <summary>
        /// Verifica que si faltan datos obligatorios, el sistema impida el registro y redirija al inicio.
        /// </summary>
        [Fact]
        public async Task GuardarUsuario_DatosIncompletos_RedirigeAIndex()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var result = await controller.GuardarUsuario(null, "pass123", 2, "Carlos");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
        }

        /// <summary>
        /// Verifica que el sistema notifique un error si la contraseña no coincide con el registro.
        /// </summary>
        [Fact]
        public async Task GuardarUsuario_UsuarioExistentePasswordIncorrecta_EstableceErrorYRedirige()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var hashDummy = new byte[] { 0x1, 0x2, 0x3, 0x4 };
            mockSecurity
                .Setup(s => s.VerifyHash(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Returns(false);

            var usuarioExistente = new Usuario_model
            {
                IdUsuario = 8,
                Login = "carlosp",
                Password = hashDummy,
                IdRol = 2,
                NombreReal = "Carlos Pérez"
            };
            context.Usuarios.Add(usuarioExistente);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.GuardarUsuario("carlosp", "password_erronea", 2, "Carlos Pérez");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.False(Convert.ToBoolean(redirectResult.RouteValues?[RouteJuegoIniciado]));
            Assert.Equal(ErrorPasswordIncorrecta, controller.TempData[TempDataError]);
        }

        #endregion

        #region Pruebas: ValidarRespuesta
        
        /// <summary>
        /// Verifica que una respuesta correcta permita continuar el juego y redirija al Index para la siguiente pregunta.
        /// </summary>
        [Fact]
        public async Task ValidarRespuesta_RespuestaCorrecta_AvanzaNivelYRedirigeAIndex()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionCorrecta = new Opcion_model { IdOpcion = 10, Texto = "56", Valida = true, IdPregunta = 1 };
            context.Opciones.Add(opcionCorrecta);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            controller.ControllerContext = GetMockControllerContext("IdNivelActual", "1");

            var result = await controller.ValidarRespuesta(10, 1, 1, 0, "");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
        }

        /// <summary>
        /// Verifica que al completar satisfactoriamente el nivel máximo, el juego registre el histórico
        /// y marque el estado como Ganado.
        /// </summary>
        [Fact]
        public async Task ValidarRespuesta_AlcanzaNivelTres_GuardaHistoricoYMarcaGanado()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionCorrecta = new Opcion_model { IdOpcion = 50, Texto = "Correcta", Valida = true, IdPregunta = 2 };
            context.Opciones.Add(opcionCorrecta);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var mockSession = new Mock<Microsoft.AspNetCore.Http.ISession>();
            var sessionValue = Encoding.UTF8.GetBytes("10");
            mockSession // Clave de sesión parametrizada
                .Setup(s => s.TryGetValue("IdJugadorActual", out sessionValue))
                .Returns(true);

            var httpContext = new DefaultHttpContext { Session = mockSession.Object };
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var result = await controller.ValidarRespuesta(
                idOpcion: 50,
                puntajeActual: 900,
                idComplejidad: 5,
                conteoNivel: 3,
                idsRespondidas: "1,2"
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RouteGanado]));
            Assert.False(Convert.ToBoolean(redirectResult.RouteValues?[RouteJuegoIniciado]));

            var historicoEnDb = await context.Historicos.FirstOrDefaultAsync();
            var actualPuntosAcumulados = historicoEnDb?.PuntosAcumulados;
            var actualIdJugador = historicoEnDb?.IdJugador;

            Assert.NotNull(historicoEnDb);
            Assert.Equal(1000, actualPuntosAcumulados);
            Assert.Equal(10, actualIdJugador);
        }

        /// <summary>
        /// Verifica que una respuesta errónea termine el juego y redirija al estado de derrota.
        /// </summary>
        [Fact]
        public async Task ValidarRespuesta_RespuestaIncorrecta_RedirigeAPerdiste()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionIncorrecta = new Opcion_model { IdOpcion = 99, Texto = "Respuesta Mal", Valida = false, IdPregunta = 1 };
            context.Opciones.Add(opcionIncorrecta);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var result = await controller.ValidarRespuesta(
                idOpcion: 99,
                puntajeActual: 500,
                idComplejidad: 1,
                conteoNivel: 1,
                idsRespondidas: ""
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RoutePerdiste]));
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RouteJuegoIniciado]));
        }

        /// <summary>
        /// Verifica el comportamiento de seguridad cuando se envía un ID de opción que no existe.
        /// </summary>
        [Fact]
        public async Task ValidarRespuesta_OpcionNoExiste_RedirigeAPerdiste()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var result = await controller.ValidarRespuesta(
                idOpcion: 999,
                puntajeActual: 0,
                idComplejidad: 1,
                conteoNivel: 0,
                idsRespondidas: ""
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues?[RoutePerdiste]));
        }

        #endregion

        #region Pruebas: Retirarse

        /// <summary>
        /// Verifica que el jugador pueda retirarse voluntariamente salvando sus puntos en el histórico.
        /// </summary>
        [Fact]
        public async Task Retirarse_JugadorAutenticado_GuardaHistoricoYRedirigeAIndex()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            controller.ControllerContext = GetMockControllerContext(SessionIdJugador, "99");

            var result = await controller.Retirarse(puntajeFinal: 500, idJugador: 0);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.Equal(500, redirectResult.RouteValues![RoutePuntaje]);
            Assert.Equal(true, redirectResult.RouteValues["retirado"]);

            var registroGuardado = await context.Historicos.FirstOrDefaultAsync();
            var actualPuntosAcumulados = registroGuardado?.PuntosAcumulados;
            var actualIdJugador = registroGuardado?.IdJugador;

            Assert.NotNull(registroGuardado);
            Assert.Equal(500, actualPuntosAcumulados);
            Assert.Equal(99, actualIdJugador);
        }

        /// <summary>
        /// Verifica que la funcionalidad de retiro funcione incluso si falla la recuperación del ID de sesión, usando el ID de respaldo.
        /// </summary>
        [Fact]
        public async Task Retirarse_JugadorNoAutenticado_UsaIdJugadorDeRespaldoYRedirigeAIndex()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var mockSession = new Mock<ISession>();
            byte[]? valueBytes = null;
            mockSession.Setup(s => s.TryGetValue(SessionIdJugador, out valueBytes)).Returns(false);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };
            var result = await controller.Retirarse(puntajeFinal: 100, idJugador: 45);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(JuegoController.Index), redirectResult.ActionName);
            Assert.Equal(100, redirectResult.RouteValues!["puntaje"]);

            var registroGuardado = await context.Historicos.FirstOrDefaultAsync();
            var actualPuntosAcumulados = registroGuardado?.PuntosAcumulados;
            var actualIdJugador = registroGuardado?.IdJugador;

            Assert.NotNull(registroGuardado);
            Assert.Equal(100, actualPuntosAcumulados);
            Assert.Equal(45, actualIdJugador);
        }

        #endregion

        #region Pruebas: Index

        [Fact]
        public async Task Index_JuegoTerminadoPorPerdida_RetornaVistaInmediatamente()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(idCategoria: null, puntaje: 500, perdiste: true);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(500, controller.ViewBag.Puntaje);
            Assert.True(controller.ViewBag.Perdiste);
        }

        /// <summary>
        /// Verifica que al entrar por primera vez sin iniciar sesión, no se intente buscar preguntas en la base de datos.
        /// </summary>
        [Fact]
        public async Task Index_JuegoNoIniciadoPuntajeCero_RetornaVistaSinBuscarPreguntas()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(idCategoria: null, puntaje: 0, perdiste: false, juegoIniciado: false);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        /// <summary>
        /// Verifica que durante un juego activo, el sistema recupere la pregunta correcta y asigne los nombres de categoría al ViewBag.
        /// </summary>
        [Fact]
        public async Task Index_JuegoActivo_BuscaYRetornaPreguntaConDatosEnViewBag()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var categoria = new Categoria_model
            {
                IdCategoria = 1,
                Nombre = "Historia",
                Descripcion = "Preguntas de historia general"
            };

            var tipoComplejidad = typeof(Pregunta_model).GetProperty("Complejidad")?.PropertyType;
            object? complejidadInstancia = null;

            if (tipoComplejidad != null)
            {
                complejidadInstancia = Activator.CreateInstance(tipoComplejidad);
                tipoComplejidad.GetProperty("IdComplejidad")?.SetValue(complejidadInstancia, 1);
                tipoComplejidad.GetProperty("Nombre")?.SetValue(complejidadInstancia, "Fácil");
                tipoComplejidad.GetProperty("Descripcion")?.SetValue(complejidadInstancia, "Nivel inicial");
            }

            var pregunta = new Pregunta_model
            {
                IdPregunta = 10,
                IdComplejidad = 1,
                Texto = "¿Cuál es la capital de Colombia?",
                Categoria = categoria
            };

            typeof(Pregunta_model).GetProperty("Complejidad")?.SetValue(pregunta, complejidadInstancia);

            context.Preguntas.Add(pregunta);
            await context.SaveChangesAsync();

            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);

            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(
                idCategoria: null,
                puntaje: 100,
                perdiste: false,
                idComplejidad: 1,
                conteoNivel: 0,
                idsRespondidas: "1,2",
                juegoIniciado: true
            );

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Pregunta_model>(viewResult.Model);
            Assert.Equal(10, model.IdPregunta);
            Assert.Equal("Historia", controller.ViewBag.NombreCategoria);
        }

        /// <summary>
        /// Verifica que el sistema maneje con elegancia el caso donde no hay más preguntas disponibles para una dificultad específica.
        /// </summary>
        [Fact]
        public async Task Index_JuegoActivoSinPreguntasDisponibles_EstableceSinPreguntasEnViewBag()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = CrearControllerConServiciosReales(context, mockSecurity.Object);
            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(
                idCategoria: null,
                puntaje: 200,
                perdiste: false,
                idComplejidad: 3,
                juegoIniciado: true
            );

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
            Assert.True(controller.ViewBag.SinPreguntas);
        }

        #endregion
    }
}