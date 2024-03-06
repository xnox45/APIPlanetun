using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace APIPlanetun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetunController : ControllerBase
    {
        private readonly ILogger<PlanetunController> _logger;

        private readonly object _lock = new object();

        private List<string> tabuada = new List<string>();

        public PlanetunController(ILogger<PlanetunController> logger)
        {
            _logger = logger;
        }

        [HttpPost("processar-async")]
        public async Task<IActionResult> ProcessarTabuadaAsync([FromBody] List<int> numeros)
        {
            Retorno<List<string>> retorno = new();
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                List<Task> tasks = new();

                foreach (var numero in numeros)
                    tasks.Add(ProcessarNumero(numero));
                
                await Task.WhenAll(tasks);

                tabuada.Sort();

                stopwatch.Stop();
                TimeSpan tempoDecorrido = stopwatch.Elapsed;

                retorno.Sucesso = true;
                retorno.Mensagem = $"Tempo decorrido: {tempoDecorrido.TotalSeconds} segundos.";
                retorno.Objeto = tabuada;
                
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar: {ex.Message}");
            }
        }

        [HttpPost("processar-sync")]
        public async Task<IActionResult> ProcessarTabuadaSync([FromBody] List<int> numeros)
        {
            Retorno<List<string>> retorno = new();
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                foreach (var numero in numeros)
                    await ProcessarNumero(numero);
                
                tabuada.Sort();

                stopwatch.Stop();
                TimeSpan tempoDecorrido = stopwatch.Elapsed;

                retorno.Sucesso = true;
                retorno.Mensagem = $"Tempo decorrido: {tempoDecorrido.TotalSeconds} segundos.";
                retorno.Objeto = tabuada;

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar: {ex.Message}");
            }
        }

        private async Task ProcessarNumero(int numero)
        {
            await Task.Delay(1000);

            string filePath = $"tabuada_de_{numero}.txt";

            var resultado = new StringBuilder();

            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        string conta = $"{numero} x {i} = {numero * i}";

                        resultado.AppendLine(conta);

                        writer.WriteLine(conta);
                    }

                    tabuada.Add(resultado.ToString());
                }
            }
        }
    }
}