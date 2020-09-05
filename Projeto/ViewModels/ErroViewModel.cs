namespace Projeto.ViewModels
{
    public class ErroViewModel
    {
        public ErroViewModel(bool valido, string mensagem)
        {
            Valido = valido;
            Mensagem = mensagem;
        }
        public bool Valido { get; set; }
        public string Mensagem { get; set; }
    }
}