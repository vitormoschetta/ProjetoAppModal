using System;
using System.Linq.Expressions;
using Projeto.Domain.Entities;

namespace Projeto.Domain.Queries
{
    public static class ClienteQueries
    {
        // Cria expressõe para facilitar a consulta
        public static Expression<Func<Cliente, bool>> InformaçõesDoCliente(string cpf)
        {
            return x => x.Cpf == cpf;
        }
    }
}