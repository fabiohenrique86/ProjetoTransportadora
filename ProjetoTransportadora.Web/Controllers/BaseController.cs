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
                if (Session[ConfigurationManager.AppSettings["ProjetoTransportadoraCookieLogin"]] == null)
                    return null;

                var usuarioDto = (UsuarioDto)Session[ConfigurationManager.AppSettings["ProjetoTransportadoraCookieLogin"]];

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
                Session[ConfigurationManager.AppSettings["ProjetoTransportadoraCookieLogin"]] = usuarioDto;
                Session[ConfigurationManager.AppSettings["ProjetoTransportadoraSessionUsuarioTrocarSenha"]] = null;
            }
            catch (Exception)
            {

            }
        }

        public void LogOut()
        {
            try
            {
                Session[ConfigurationManager.AppSettings["ProjetoTransportadoraCookieLogin"]] = null;
                Session[ConfigurationManager.AppSettings["ProjetoTransportadoraSessionUsuarioTrocarSenha"]] = null;
            }
            catch (Exception)
            {

            }
        }
    }
}