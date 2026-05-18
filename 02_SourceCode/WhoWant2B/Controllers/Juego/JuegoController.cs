using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoWant2B.Data;
using WhoWant2B.Models;
using WhoWant2B.Services;
using static WhoWant2B.Comunes;

namespace WhoWant2B.Controllers
{
    public class JuegoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityService _securityService;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="JuegoController"/> e inyecta el contexto de la base de datos.
        /// </summary>
        /// <param name="context">El contexto de la base de datos de la aplicación (<see cref="ApplicationDbContext"/>) utilizado para interactuar con los datos de los juegos.</param>
        /// <param name="securityService">El servicio encargado de la seguridad y el hashing de datos.</param>
        public JuegoController(ApplicationDbContext context, ISecurityService securityService)
        {
            _context = context;
            _securityService = securityService;
        }


        /// <summary>
        /// Gestiona el flujo principal del ciclo de juego, cargando la vista inicial o la siguiente pregunta disponible.
        /// Almacena el estado actual en el <see cref="ViewBag"/> y filtra de forma aleatoria las preguntas de la base de datos 
        /// según la complejidad actual y las preguntas que el usuario ya ha respondido.
        /// </summary>
        /// <param name="idCategoria">El identificador opcional de la categoría seleccionada para el juego.</param>
        /// <param name="puntaje">El puntaje acumulado por el jugador hasta el momento. Por defecto es 0.</param>
        /// <param name="perdiste">Indica si el jugador ha perdido la partida actual. Por defecto es <c>false</c>.</param>
        /// <param name="idComplejidad">El nivel de complejidad actual de las preguntas a mostrar. Por defecto es 1.</param>
        /// <param name="conteoNivel">El contador de preguntas respondidas o niveles avanzados en la sesión actual. Por defecto es 0.</param>
        /// <param name="idsRespondidas">Cadena de texto con los IDs de las preguntas ya respondidas, separados por comas, para evitar repeticiones. Por defecto está vacío.</param>
        /// <param name="juegoIniciado">Indica si la partida ya ha comenzado formalmente. Por defecto es <c>false</c>.</param>
        /// <returns>
        /// Una vista con la siguiente pregunta a responder si el juego está activo; 
        /// la vista por defecto (sin modelo) si el juego no ha iniciado o el jugador perdió;
        /// o un resultado <see cref="NotFoundResult"/> si no se encuentran más preguntas para la complejidad dada.
        /// </returns>
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

            if (perdiste || ganado || retirado)
            {
                return View();
            }

            if (!juegoIniciado && puntaje == 0 && !perdiste)
            {
                return View(null);
            }

            var respondidasList = string.IsNullOrEmpty(idsRespondidas)
                ? new List<int>()
                : idsRespondidas.Split(',').Select(int.Parse).ToList();

            var pregunta = await _context.Preguntas
                .Include(p => p.Opciones)
                .Include(p => p.Complejidad)
                .Include(p => p.Categoria)
                .Where(p => p.IdComplejidad == idComplejidad && !respondidasList.Contains(p.IdPregunta))
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefaultAsync();

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
        /// Procesa de forma asíncrona la respuesta seleccionada por el jugador. 
        /// Si es correcta, incrementa el puntaje (+100), registra la pregunta para evitar que se repita y gestiona el aumento de nivel 
        /// de complejidad (cada 3 aciertos). Si es incorrecta, marca la partida como perdida.
        /// </summary>
        /// <param name="idOpcion">El identificador único de la opción de respuesta seleccionada por el usuario.</param>
        /// <param name="puntajeActual">El puntaje acumulado por el jugador antes de validar esta respuesta.</param>
        /// <param name="idComplejidad">El nivel de complejidad actual del juego (de 1 a 5).</param>
        /// <param name="conteoNivel">El contador de respuestas consecutivas correctas en el nivel de complejidad actual.</param>
        /// <param name="idsRespondidas">Cadena de texto con los IDs de las preguntas ya respondidas, separados por comas.</param>
        /// <returns>
        /// Una redirección a la acción <see cref="Index(int?, int, bool, int, int, string, bool)"/> con el estado del juego actualizado 
        /// (nuevos valores de puntaje, complejidad y racha si acertó, o bandera de derrota si falló).
        /// </returns>
        [HttpPost]        
        public async Task<IActionResult> ValidarRespuesta(int idOpcion, int puntajeActual, int idComplejidad, int conteoNivel, string idsRespondidas)
        {            
            var opcion = await _context.Opciones.FirstOrDefaultAsync(o => o.IdOpcion == idOpcion);

            if (opcion == null || !opcion.Valida)
            {
                return RedirectToAction("Index", new { perdiste = true, puntaje = puntajeActual, juegoIniciado = true });
            }
            
            string nuevasRespondidas = string.IsNullOrEmpty(idsRespondidas)
                ? opcion.IdPregunta.ToString()
                : $"{idsRespondidas},{opcion.IdPregunta}";

            conteoNivel++;
            int nuevaComplejidad = idComplejidad;
            int puntajeFinal = puntajeActual + 100; // Sumamos el puntaje de esta respuesta
            
            if (conteoNivel >= 3)
            {
                conteoNivel = 0; // Reiniciamos el contador de nivel para la nueva dificultad
                
                if (nuevaComplejidad < 5)
                {
                    nuevaComplejidad++; // Sube a la siguiente complejidad
                }
                else
                {                    
                    var idSesion = HttpContext.Session.GetString("IdJugadorActual");

                    var registroVictoria = new Historico_model
                    {
                        PuntosAcumulados = puntajeFinal,
                        DineroAcumulado = puntajeFinal,
                        Fecha = DateTime.UtcNow,
                        IdJugador = !string.IsNullOrEmpty(idSesion) ? int.Parse(idSesion) : 0,
                        IdEstadoJuego = (int)EstadoJuegoEnum.Ganado,
                        Usuario = null!,
                        EstadoJuego = null!
                    };

                    _context.Historicos.Add(registroVictoria);
                    await _context.SaveChangesAsync();
                    
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
        /// Gestiona de forma asíncrona la autenticación y el registro de usuarios. 
        /// Transforma la contraseña ingresada en texto plano a un Hash SHA-512 para mayor seguridad. 
        /// Si el usuario ya existe, valida la contraseña (comparación de hashes) e inicia sesión; si el usuario no existe, 
        /// lo registra en la base de datos e inicia sesión de forma automática.
        /// </summary>
        /// <param name="login">El alias, nombre de usuario o credencial única de acceso del jugador.</param>
        /// <param name="password">La contraseña en texto plano proporcionada por el usuario.</param>
        /// <param name="idRol">El identificador del rol asignado al usuario (por ejemplo: 1 para Administrador/Configuración).</param>
        /// <param name="nombreReal">El nombre real o completo de la persona que se está registrando.</param>
        /// <returns>
        /// Un <see cref="IActionResult"/> que redirige al panel de configuración (si el usuario es administrador), 
        /// al ciclo principal del juego con la bandera <c>juegoIniciado = true</c> (si la autenticación o registro son exitosos), 
        /// o a la vista de inicio con un mensaje de error en <see cref="TempData"/> si la contraseña no coincide.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(string login, string password, int idRol, string nombreReal)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nombreReal))
            {
                return RedirectToAction("Index");
            }

            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Login.ToLower() == login.Trim().ToLower());

            if (usuarioExistente != null)
            {
                if (_securityService.VerifyHash(password, usuarioExistente.Password))
                {
                    HttpContext.Session.SetString("IdJugadorActual", usuarioExistente.IdUsuario.ToString());
                    if (usuarioExistente.IdRol == 1)
                    {
                        return RedirectToAction("Index", "Configuracion");
                    }
                    return RedirectToAction("Index", new { juegoIniciado = true });
                }
                else
                {
                    TempData["ErrorMessage"] = "La contraseña es incorrecta para este jugador.";
                    return RedirectToAction("Index", new { juegoIniciado = false });
                }
            }
            else
            {
                var nuevoUsuario = new Usuario_model
                {
                    Login = login.Trim(),
                    Password = _securityService.GetHash(password),
                    IdRol = idRol,
                    NombreReal = nombreReal.Trim()
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("IdJugadorActual", nuevoUsuario.IdUsuario.ToString());
                return RedirectToAction("Index", new { juegoIniciado = true });
            }
        }

        /// <summary>
        /// Verifica de forma asíncrona si existe un usuario registrado con el nombre real proporcionado.
        /// Este método se utiliza habitualmente mediante peticiones AJAX desde la vista para validaciones en tiempo real.
        /// </summary>
        /// <param name="nombreReal">El nombre real o completo del usuario que se desea buscar en la base de datos.</param>
        /// <returns>
        /// Un <see cref="JsonResult"/> que contiene un objeto anónimo con el estado de la búsqueda:
        /// <list type="bullet">
        /// <item><description>Si el usuario existe: <c>{ existe = true, loginExistente = "..." }</c></description></item>
        /// <item><description>Si el usuario no existe o el parámetro está vacío: <c>{ existe = false }</c></description></item>
        /// </list>
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> VerificarUsuarioExistente(string nombreReal)
        {
            if (string.IsNullOrWhiteSpace(nombreReal))
            {
                return Json(new { existe = false });
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreReal.ToLower() == nombreReal.Trim().ToLower());

            if (usuario != null)
            {
                return Json(new { existe = true, loginExistente = usuario.Login });
            }

            return Json(new { existe = false });
        }

        /// <summary>
        /// Procesa de forma asíncrona la solicitud de retiro voluntario del jugador. 
        /// Valida la identidad del usuario mediante la sesión para evitar manipulaciones en el cliente, 
        /// registra la partida en el histórico con el estado "Retirado", guarda los puntos obtenidos 
        /// y prepara las variables de entorno necesarias para renderizar la pantalla de fin de juego con éxito.
        /// </summary>
        /// <param name="puntajeFinal">El puntaje total acumulado por el jugador hasta el momento de su retiro.</param>
        /// <param name="idJugador">El identificador del jugador enviado por el formulario (usado como respaldo si la sesión expira).</param>
        /// <returns>
        /// La vista "Index" configurada con las banderas de retiro activas y un modelo nulo, indicando que la partida actual ha concluido.
        /// </returns>
        [HttpPost] // ◄ CRUCIAL: Para que intercepte el envío del formulario
        public async Task<IActionResult> Retirarse(int puntajeFinal, int idJugador)
        {

            var idSesion = HttpContext.Session.GetString("IdJugadorActual");
            int jugadorIdReal = !string.IsNullOrEmpty(idSesion) ? int.Parse(idSesion) : idJugador;

            Historico_model registro = new Historico_model
            {
                PuntosAcumulados = puntajeFinal,
                DineroAcumulado = puntajeFinal,
                Fecha = DateTime.UtcNow,
                IdJugador = jugadorIdReal,
                IdEstadoJuego = (int)EstadoJuegoEnum.Retirado,
                Usuario = null,
                EstadoJuego = null
            };

            _context.Historicos.Add(registro);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { puntaje = puntajeFinal, retirado = true, juegoIniciado = true });
        }
    }
}
