namespace Projeto.Repository
{
    public class Resultado
    {
        public Resultado(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
    }
}