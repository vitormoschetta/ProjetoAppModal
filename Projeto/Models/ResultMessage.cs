namespace Projeto.Models
{
    public class ResultMessage
    {
        public ResultMessage(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
    }
}