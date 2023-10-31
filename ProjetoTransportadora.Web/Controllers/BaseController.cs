using ProjetoTransportadora.Dto;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Controllers
{
    public class BaseController : Controller
    {
        public UsuarioDto UsuarioLogado()
        {
            try
            {
                if (Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]] == null)
                    return null;

                var usuarioDto = (UsuarioDto)Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]];

                return usuarioDto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void LogIn(UsuarioDto usuarioDto)
        {
            try
            {
                usuarioDto.Senha = null;
                Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]] = usuarioDto;
            }
            catch (Exception)
            {
                 
            }
        }

        public void LogOut()
        {
            try
            {
                Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]] = null;
            }
            catch (Exception)
            {

            }
        }
    }
}