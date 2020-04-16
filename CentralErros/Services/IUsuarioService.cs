using CentralErros.Api.Models;
using System.Collections.Generic;

namespace CentralErros.Services
{
    public interface IUsuarioService
    {
        Usuario ProcurarPorId(int usuarioId);
        IList<Usuario> ProcurarPorNome(string nome);
        Usuario Salvar(Usuario usuario);
    }
}