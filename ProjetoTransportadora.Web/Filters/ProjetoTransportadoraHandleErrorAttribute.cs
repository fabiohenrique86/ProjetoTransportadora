using ProjetoTransportadora.Business;
using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjetoTransportadora.Web.Filters
{
    public class ProjetoTransportadoraHandleErrorAttribute : HandleErrorAttribute
    {
        LogBusiness logBusiness;

        public ProjetoTransportadoraHandleErrorAttribute()
        {
            logBusiness = new LogBusiness();
        }

        public override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;

            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                base.OnException(filterContext);

            HttpContext.Current.Response.Clear();
            
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            filterContext.ExceptionHandled = true;
            
            if (exception is BusinessException)
            {
                filterContext.Result = new JsonResult { Data = new { Sucesso = false, Mensagem = exception.Message, Data = "", Erro = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                filterContext.Result = new JsonResult { Data = new { Sucesso = false, Mensagem = exception.Message, Data = "", Erro = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                try
                {
                    logBusiness.Incluir(new LogDto() { Exception = exception.ToString(), DataCadastro = DateTime.UtcNow });
                }
                catch (Exception)
                {

                }
            }
        }
    }
}