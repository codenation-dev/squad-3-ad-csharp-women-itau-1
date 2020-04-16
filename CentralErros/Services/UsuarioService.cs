using CentralErros.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CentralErros.Services
{
    public class UsuarioService : IUsuarioService
    {
        private CentralErroContexto _context;

        public UsuarioService(CentralErroContexto contexto)
        {
            _context = contexto;
        }

        public Usuario ProcurarPorId(int usuarioId)
        {
            //utilzar metodo Find
            return _context.Usuarios.Find(usuarioId);
        }

        public IList<Usuario> ProcurarPorNome(string nome)
        {
            //utilizar método Where
            return _context.Usuarios.Where(x => x.Nome == nome).ToList();
        }

        public Usuario Salvar(Usuario usuario)
        {
            var existe = _context.Usuarios
                                .Where(x => x.Id == usuario.Id)
                                .FirstOrDefault();

            if (existe == null)
                _context.Usuarios.Add(usuario);
            else
            {
                existe.Nome = usuario.Nome;
            }

            _context.SaveChanges();

            return usuario;
        }
    }
}
