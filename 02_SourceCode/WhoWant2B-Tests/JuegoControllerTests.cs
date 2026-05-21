using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using WhoWant2B.Controllers;
using WhoWant2B.Data;
using WhoWant2B.Models;
using WhoWant2B.Services;

namespace WhoWant2B.Tests
{
    public class JuegoControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        private ControllerContext GetMockControllerContext(string sessionKey, string sessionValue)
        {
            var mockSession = new Mock<ISession>();
            var valueBytes = Encoding.UTF8.GetBytes(sessionValue);

            // Corrección del Mock para que Session.GetString funcione transparentemente
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

            var controller = new JuegoController(context, mockSecurity.Object);         
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

        [Fact]
        public async Task VerificarUsuarioExistente_UsuarioNoExiste_DevuelveJsonConExisteFalse()
        {
            
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);            
            var result = await controller.VerificarUsuarioExistente("Usuario Fantasma");            
            var jsonResult = Assert.IsType<JsonResult>(result);
            var data = jsonResult.Value;
            Assert.NotNull(data);

            var propiedadExiste = data.GetType().GetProperty("existe");
            var existeValue = propiedadExiste?.GetValue(data, null);

            Assert.NotNull(existeValue);
            Assert.False((bool)existeValue);
        }

        [Fact]
        public async Task Retirarse_JugadorAutenticado_GuardaHistoricoYRedirigeAIndex()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);

            controller.ControllerContext = GetMockControllerContext("IdJugadorActual", "99");

            // Act
            var result = await controller.Retirarse(puntajeFinal: 500, idJugador: 0);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(500, redirectResult.RouteValues!["puntaje"]);
            Assert.Equal(true, redirectResult.RouteValues["retirado"]);

            var registroGuardado = await context.Historicos.FirstOrDefaultAsync();
            Assert.NotNull(registroGuardado);
            Assert.Equal(500, registroGuardado.PuntosAcumulados);
            Assert.Equal(99, registroGuardado.IdJugador);
        }

        [Fact]
        public async Task Retirarse_JugadorNoAutenticado_UsaIdJugadorDeRespaldoYRedirigeAIndex()
        {            
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);         
            var mockSession = new Mock<ISession>();
            byte[]? valueBytes = null;
            mockSession.Setup(s => s.TryGetValue("IdJugadorActual", out valueBytes)).Returns(false);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Session).Returns(mockSession.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };            
            var result = await controller.Retirarse(puntajeFinal: 100, idJugador: 45);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal(100, redirectResult.RouteValues!["puntaje"]);

            var registroGuardado = await context.Historicos.FirstOrDefaultAsync();
            Assert.NotNull(registroGuardado);
            Assert.Equal(100, registroGuardado.PuntosAcumulados);
            Assert.Equal(45, registroGuardado.IdJugador);
        }

        [Fact]
        public async Task GuardarUsuario_DatosValidos_InsertaUsuarioYRedirige()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var hashSimulado = new byte[] { 0x20, 0x20, 0x20, 0x20 };
            mockSecurity
                .Setup(s => s.GetHash(It.IsAny<string>()))
                .Returns(hashSimulado);

            var controller = new JuegoController(context, mockSecurity.Object);
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
            Assert.Equal("Index", redirectResult.ActionName);


            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["juegoIniciado"]));
            var usuarioEnDb = await context.Usuarios.FirstOrDefaultAsync(u => u.Login == "nuevojugador");
            
            Assert.NotNull(usuarioEnDb);
            Assert.Equal("Carlos Pérez", usuarioEnDb.NombreReal);
            Assert.Equal(2, usuarioEnDb.IdRol);
        }

        [Fact]
        public async Task GuardarUsuario_DatosIncompletos_RedirigeAIndex()
        {            
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);

            var result = await controller.GuardarUsuario(null, "pass123", 2, "Carlos");


            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

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

            var controller = new JuegoController(context, mockSecurity.Object);

            
            var mockSession = new Mock<Microsoft.AspNetCore.Http.ISession>();
            var httpContext = new DefaultHttpContext { Session = mockSession.Object };
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            
            var result = await controller.GuardarUsuario("carlosp", "password123", 2, "Carlos Pérez");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["juegoIniciado"]));
        }

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

            var controller = new JuegoController(context, mockSecurity.Object);

            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.GuardarUsuario("carlosp", "password_erronea", 2, "Carlos Pérez");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.False(Convert.ToBoolean(redirectResult.RouteValues["juegoIniciado"]));
            Assert.Equal("La contraseña es incorrecta para este jugador.", controller.TempData["ErrorMessage"]);
        }

        [Fact]
        public async Task ValidarRespuesta_RespuestaCorrecta_AvanzaNivelYRedirigeAIndex()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionCorrecta = new Opcion_model { IdOpcion = 10, Texto = "56", Valida = true, IdPregunta = 1 };
            context.Opciones.Add(opcionCorrecta);
            await context.SaveChangesAsync();

            var controller = new JuegoController(context, mockSecurity.Object);
            controller.ControllerContext = GetMockControllerContext("IdNivelActual", "1");

            var result = await controller.ValidarRespuesta(10, 1, 1, 0, "");

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task ValidarRespuesta_AlcanzaNivelTres_GuardaHistoricoYMarcaGanado()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionCorrecta = new Opcion_model { IdOpcion = 50, Texto = "Correcta", Valida = true, IdPregunta = 2 };
            context.Opciones.Add(opcionCorrecta);
            await context.SaveChangesAsync();

            var controller = new JuegoController(context, mockSecurity.Object);

            var mockSession = new Mock<Microsoft.AspNetCore.Http.ISession>();
            var sessionValue = System.Text.Encoding.UTF8.GetBytes("10");
            mockSession
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
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["ganado"]));
            Assert.False(Convert.ToBoolean(redirectResult.RouteValues["juegoIniciado"]));

            var historicoEnDb = await context.Historicos.FirstOrDefaultAsync();
            Assert.NotNull(historicoEnDb);
            Assert.Equal(1000, historicoEnDb.PuntosAcumulados);
            Assert.Equal(10, historicoEnDb.IdJugador);
        }

        [Fact]
        public async Task ValidarRespuesta_RespuestaIncorrecta_RedirigeAPerdiste()
        {            
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();

            var opcionIncorrecta = new Opcion_model { IdOpcion = 99, Texto = "Respuesta Mal", Valida = false, IdPregunta = 1 };
            context.Opciones.Add(opcionIncorrecta);
            await context.SaveChangesAsync();

            var controller = new JuegoController(context, mockSecurity.Object);

            var result = await controller.ValidarRespuesta(
                idOpcion: 99,
                puntajeActual: 500,
                idComplejidad: 1,
                conteoNivel: 1,
                idsRespondidas: ""
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["perdiste"]));
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["juegoIniciado"]));
        }

        [Fact]
        public async Task ValidarRespuesta_OpcionNoExiste_RedirigeAPerdiste()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);
         
            var result = await controller.ValidarRespuesta(
                idOpcion: 999,
                puntajeActual: 0,
                idComplejidad: 1,
                conteoNivel: 0,
                idsRespondidas: ""
            );

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(Convert.ToBoolean(redirectResult.RouteValues["perdiste"]));
        }

        [Fact]
        public async Task Index_JuegoTerminadoPorPerdida_RetornaVistaInmediatamente()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);
            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(idCategoria: null, puntaje: 500, perdiste: true);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Retorna la vista por defecto (View())
            Assert.Equal(500, controller.ViewBag.Puntaje);
            Assert.True(controller.ViewBag.Perdiste);
        }

        [Fact]
        public async Task Index_JuegoNoIniciadoPuntajeCero_RetornaVistaSinBuscarPreguntas()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);

            var tempDataMock = new Mock<ITempDataProvider>();
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), tempDataMock.Object);

            var result = await controller.Index(idCategoria: null, puntaje: 0, perdiste: false, juegoIniciado: false);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

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
            object complejidadInstancia = null;

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

            var controller = new JuegoController(context, mockSecurity.Object);

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

        [Fact]
        public async Task Index_JuegoActivoSinPreguntasDisponibles_EstableceSinPreguntasEnViewBag()
        {
            using var context = GetInMemoryDbContext();
            var mockSecurity = new Mock<ISecurityService>();
            var controller = new JuegoController(context, mockSecurity.Object);
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
    }
}