using ProjetoTransportadora.Dto;
using System.Web.Mvc;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;

namespace ProjetoTransportadora.Web.Filters
{
    public class ProjetoTransportadoraHandleRequestAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.RequestContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RequestContext.RouteData.Values["action"].ToString();

            // home
            if (controller == "Home" && action == "Index")
                return;

            // log in
            if (controller == "Usuario" && action == "Login")
                return;

            // log off
            if (controller == "Usuario" && action == "Sair")
                return;

            // alterar senha
            if (controller == "Usuario" && action == "AlterarSenha")
                return;

            // sem permissão
            if (controller == "UsuarioSemPermissao" && action == "Index")
                return;

            // troca de senha
            if (controller == "UsuarioTrocarSenha" && action == "Index")
                return;

            // acesso via link direto
            if (filterContext.HttpContext.Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]] == null)
            {
                var urlHelper = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Index", "Home");
                filterContext.Result = new RedirectResult(urlHelper);
                return;
            }

            var usuarioDto = (UsuarioDto)filterContext.HttpContext.Session[ConfigurationManager.AppSettings["ProjetoTransportadora.Usuario"]];

            if (usuarioDto == null)
            {
                var urlHelper = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Index", "Home");
                filterContext.Result = new RedirectResult(urlHelper);
                return;
            }

            // usuário "adm" tem acesso à todas funcionalidades
            if (usuarioDto.Login?.ToLower() == "adm")
                return;

            // tela inicial pós login
            if (controller == "Transportadora" && action == "Index")
                return;

            // ações diversas
            if (action == "Exportar" || action == "ListarAutoComplete" || action == "Calcular" || action == "GerarParcelas" || action == "GerarContrato")
                return;

            var temPermissao = false;

            if (action == "Listar" || action == "ListarGrid" || action == "ListarGridParcela" || action == "Index")
                temPermissao = usuarioDto.UsuarioGrupoDto.Any(x => x.GrupoDto.Ativo.GetValueOrDefault() == true && x.GrupoDto.GrupoFuncionalidadeDto.Any(w => w.FuncionalidadeDto.Nome == controller && w.Ler.GetValueOrDefault() == true));
            else if (action == "Incluir" || action == "IncluirAuxiliar" || action == "Importar")
                temPermissao = usuarioDto.UsuarioGrupoDto.Any(x => x.GrupoDto.Ativo.GetValueOrDefault() == true && x.GrupoDto.GrupoFuncionalidadeDto.Any(w => w.FuncionalidadeDto.Nome == controller && w.Inserir.GetValueOrDefault() == true));
            else if (action == "Alterar")
                temPermissao = usuarioDto.UsuarioGrupoDto.Any(x => x.GrupoDto.Ativo.GetValueOrDefault() == true && x.GrupoDto.GrupoFuncionalidadeDto.Any(w => w.FuncionalidadeDto.Nome == controller && w.Atualizar.GetValueOrDefault() == true));
            else if (action == "AlterarStatus")
                temPermissao = usuarioDto.UsuarioGrupoDto.Any(x => x.GrupoDto.Ativo.GetValueOrDefault() == true && x.GrupoDto.GrupoFuncionalidadeDto.Any(w => w.FuncionalidadeDto.Nome == controller && w.Excluir.GetValueOrDefault() == true));
            else if (action == "Executar")
                temPermissao = usuarioDto.UsuarioGrupoDto.Any(x => x.GrupoDto.Ativo.GetValueOrDefault() == true && x.GrupoDto.GrupoFuncionalidadeDto.Any(w => w.FuncionalidadeDto.Nome == controller && w.Executar.GetValueOrDefault() == true));

            if (temPermissao)
                return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HttpContext.Current.Response.Clear();

                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                filterContext.Result = new JsonResult
                {
                    Data = new { Sucesso = false, Mensagem = "Usuário não tem permissão para executar tal ação", Data = "" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var urlHelper = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Index", "UsuarioSemPermissao");
                filterContext.Result = new RedirectResult(urlHelper);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}