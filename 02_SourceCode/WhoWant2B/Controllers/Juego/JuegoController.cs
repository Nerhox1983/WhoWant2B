using Microsoft.AspNetCore.Mvc;
using WhoWant2B.Core.Interfaces;
using static WhoWant2B.Core.Enums.Comunes;

namespace WhoWant2B.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar el flujo principal del juego de trivia, 
    /// incluyendo la lógica de niveles, validación de respuestas y autenticación de jugadores.
    /// </summary>
    public class JuegoController : Controller
    {
        private readonly IJuegoService _juegoService;
        private readonly IAutenticacionService _autenticacionService;
        private readonly ISecurityService _securityService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="JuegoController"/>.
        /// </summary>
        /// <param name="juegoService">Servicio para la lógica del motor de juego.</param>
        /// <param name="autenticacionService">Servicio para la gestión de usuarios.</param>
        /// <param name="securityService">Servicio para el manejo de hashes y seguridad.</param>
        public JuegoController(
            IJuegoService juegoService,
            IAutenticacionService autenticacionService,
            ISecurityService securityService)
        {
            _juegoService = juegoService;
            _autenticacionService = autenticacionService;
            _securityService = securityService;
        }

        /// <summary>
        /// Acción principal que renderiza la vista del juego. Gestiona los diferentes estados (inicio, progreso, victoria, derrota o retiro).
        /// </summary>
        /// <param name="idCategoria">Identificador opcional de la categoría seleccionada.</param>
        /// <param name="puntaje">Puntaje acumulado actual.</param>
        /// <param name="perdiste">Flag que indica si el jugador ha fallado una respuesta.</param>
        /// <param name="idComplejidad">Nivel de dificultad actual (1 a 5).</param>
        /// <param name="conteoNivel">Número de preguntas respondidas correctamente en el nivel de complejidad actual.</param>
        /// <param name="idsRespondidas">Cadena con los IDs de preguntas ya respondidas para evitar repeticiones.</param>
        /// <param name="juegoIniciado">Flag que indica si hay una partida activa.</param>
        /// <param name="ganado">Flag que indica si el jugador completó todos los niveles.</param>
        /// <param name="retirado">Flag que indica si el jugador decidió retirarse con el puntaje actual.</param>
        /// <returns>La vista del juego con el modelo de la pregunta actual o nulo si el juego no ha iniciado o ha terminado.</returns>
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

        /// <summary>
        /// Procesa la respuesta seleccionada por el usuario y determina si avanza de nivel, gana o pierde.
        /// </summary>
        /// <param name="idOpcion">ID de la opción elegida por el jugador.</param>
        /// <param name="puntajeActual">Puntaje antes de validar la respuesta.</param>
        /// <param name="idComplejidad">Dificultad actual de la pregunta.</param>
        /// <param name="conteoNivel">Progreso actual dentro de la complejidad (3 aciertos para subir).</param>
        /// <param name="idsRespondidas">Historial de preguntas respondidas en la sesión.</param>
        /// <returns>Redirección a <see cref="Index"/> con el nuevo estado del juego.</returns>
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

        /// <summary>
        /// Gestiona el registro de un nuevo jugador o el inicio de sesión si el login ya existe.
        /// </summary>
        /// <param name="login">Nombre de usuario/login.</param>
        /// <param name="password">Contraseña en texto plano.</param>
        /// <param name="idRol">Rol asignado (Jugador por defecto).</param>
        /// <param name="nombreReal">Nombre completo del usuario.</param>
        /// <returns>Redirección al juego si es exitoso, o a la vista actual con mensaje de error si falla.</returns>
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

        /// <summary>
        /// Verifica mediante una llamada asíncrona (AJAX) si un jugador ya existe basándose en su nombre real.
        /// </summary>
        /// <param name="nombreReal">Nombre completo a buscar.</param>
        /// <returns>Objeto JSON indicando si existe y cuál es su login asociado.</returns>
        [HttpGet]
        public async Task<IActionResult> VerificarUsuarioExistente(string nombreReal)
        {
            if (string.IsNullOrWhiteSpace(nombreReal)) return Json(new { existe = false });

            var usuario = await _autenticacionService.ObtenerPorNombreRealAsync(nombreReal);
            return usuario != null
                ? Json(new { existe = true, loginExistente = usuario.Login })
                : Json(new { existe = false });
        }

        /// <summary>
        /// Registra el fin de la partida por decisión del jugador, salvaguardando el puntaje obtenido hasta el momento.
        /// </summary>
        /// <param name="puntajeFinal">Puntaje acumulado al momento del retiro.</param>
        /// <param name="idJugador">ID del jugador (usado como respaldo si no hay sesión activa).</param>
        /// <returns>Redirección a la vista final de retiro.</returns>
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