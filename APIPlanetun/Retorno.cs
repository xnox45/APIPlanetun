using System;

namespace APIPlanetun
{
    public class Retorno
    {
        public bool Sucesso { get; set; }
        public string? Mensagem { get; set; }
    }

    public class Retorno<T> : Retorno
    {
        public T? Objeto { get; set; }
    }

}
