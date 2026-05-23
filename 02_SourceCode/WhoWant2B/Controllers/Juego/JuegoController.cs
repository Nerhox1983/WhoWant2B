using Microsoft.AspNetCore.Mvc;
using WhoWant2B.Core.Interfaces;
using static WhoWant2B.Core.Enums.Comunes;

namespace WhoWant2B.Controllers
{
    public class JuegoController : Controller
    {
        private readonly IJuegoService _juegoService;
        private readonly IAutenticacionService _autenticacionService;
        private readonly ISecurityService _securityService;

        public JuegoController(
            IJuegoService juegoService,
            IAutenticacionService autenticacionService,
            ISecurityService securityService)
        {
            _juegoService = juegoService;
            _autenticacionService = autenticacionService;
            _securityService = securityService;
        }

        public async Task<IActionResult> Index(int? idCategoria, int puntaje = 0, bool perdiste = false, int idComplejidad = 1, int conteoNivel = 0, string idsRespondidas = "", bool juegoIniciado = false, bool ganado = false, bool retirado = false)
        {
            ViewBag.Puntaje = puntaje;
            ViewBag.Perdiste = perdiste;
            ViewBag.Retirado = retirado;
            ViewBag.IdComplejidad = idComplejidad;
            ViewBag.ConteoNivel = conteoNivel;
            ViewBag.IdsRespondidas = idsRespondidas;
            ViewBag.JuegoIniciado = juegoIniciado;
            ViewBag.Ganado = ganado;

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorLogin = TempData["ErrorMessage"].ToString();
            }

            if (perdiste || ganado || retirado) return View();
            if (!juegoIniciado && puntaje == 0 && !perdiste) return View(null);

            var respondidasList = string.IsNullOrEmpty(idsRespondidas)
                ? new List<int>()
                : idsRespondidas.Split(',').Select(int.Parse).ToList();

            var pregunta = await _juegoService.ObtenerSiguientePreguntaAsync(idComplejidad, respondidasList);

            if (pregunta == null && !perdiste)
            {
                ViewBag.SinPreguntas = true;
                return View(null);
            }

            if (pregunta != null)
            {
                ViewBag.NombreCategoria = pregunta.Categoria?.Nombre ?? "General";
                ViewBag.NombreComplejidad = pregunta.Complejidad?.Nombre ?? $"Nivel {idComplejidad}";
            }

            return View(pregunta);
        }

        [HttpPost]
        public async Task<IActionResult> ValidarRespuesta(int idOpcion, int puntajeActual, int idComplejidad, int conteoNivel, string idsRespondidas)
        {
            var opcion = await _juegoService.ObtenerOpcionAsync(idOpcion);

            if (opcion == null || !opcion.Valida)
            {
                return RedirectToAction("Index", new { perdiste = true, puntaje = puntajeActual, juegoIniciado = true });
            }

            string nuevasRespondidas = string.IsNullOrEmpty(idsRespondidas)
                ? opcion.IdPregunta.ToString()
                : $"{idsRespondidas},{opcion.IdPregunta}";

            conteoNivel++;
            int nuevaComplejidad = idComplejidad;
            int puntajeFinal = puntajeActual + 100;

            if (conteoNivel >= 3)
            {
                conteoNivel = 0;

                if (nuevaComplejidad < 5)
                {
                    nuevaComplejidad++;
                }
                else
                {
                    var idSesion = HttpContext.Session.GetString("IdJugadorActual");
                    int jugadorId = !string.IsNullOrEmpty(idSesion) ? int.Parse(idSesion) : 0;

                    await _juegoService.RegistrarHistoricoAsync(jugadorId, puntajeFinal, EstadoJuegoEnum.Ganado);

                    return RedirectToAction("Index", new { puntaje = puntajeFinal, juegoIniciado = false, ganado = true });
                }
            }

            return RedirectToAction("Index", new
            {
                puntaje = puntajeFinal,
                idComplejidad = nuevaComplejidad,
                conteoNivel = conteoNivel,
                idsRespondidas = nuevasRespondidas,
                juegoIniciado = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(string login, string password, int idRol, string nombreReal)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nombreReal))
            {
                return RedirectToAction("Index");
            }

            var usuarioExistente = await _autenticacionService.ObtenerPorLoginAsync(login);

            if (usuarioExistente != null)
            {
                if (_securityService.VerifyHash(password, usuarioExistente.Password))
                {
                    HttpContext.Session.SetString("IdJugadorActual", usuarioExistente.IdUsuario.ToString());
                    return RedirectToAction("Index", usuarioExistente.IdRol == 1 ? "Configuracion" : "Juego", new { juegoIniciado = true });
                }

                TempData["ErrorMessage"] = "La contraseña es incorrecta para este jugador.";
                return RedirectToAction("Index", new { juegoIniciado = false });
            }

            var passwordHash = _securityService.GetHash(password);
            var nuevoUsuario = await _autenticacionService.RegistrarJugadorAsync(login, passwordHash, idRol, nombreReal);

            HttpContext.Session.SetString("IdJugadorActual", nuevoUsuario.IdUsuario.ToString());
            return RedirectToAction("Index", new { juegoIniciado = true });
        }

        [HttpGet]
        public async Task<IActionResult> VerificarUsuarioExistente(string nombreReal)
        {
            if (string.IsNullOrWhiteSpace(nombreReal)) return Json(new { existe = false });

            var usuario = await _autenticacionService.ObtenerPorNombreRealAsync(nombreReal);
            return usuario != null
                ? Json(new { existe = true, loginExistente = usuario.Login })
                : Json(new { existe = false });
        }

        [HttpPost]
        public async Task<IActionResult> Retirarse(int puntajeFinal, int idJugador)
        {
            var idSesion = HttpContext.Session.GetString("IdJugadorActual");
            int jugadorIdReal = !string.IsNullOrEmpty(idSesion) ? int.Parse(idSesion) : idJugador;

            await _juegoService.RegistrarHistoricoAsync(jugadorIdReal, puntajeFinal, EstadoJuegoEnum.Retirado);

            return RedirectToAction("Index", new { puntaje = puntajeFinal, retirado = true, juegoIniciado = true });
        }
    }
}