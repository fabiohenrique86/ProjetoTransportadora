using ProjetoTransportadora.Business.Exceptions;
using ProjetoTransportadora.Dto;
using ProjetoTransportadora.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ProjetoTransportadora.Business
{
    public class UsuarioBusiness
    {
        UsuarioRepository usuarioRepository;
        UsuarioGrupoBusiness usuarioGrupoBusiness;
        public UsuarioBusiness()
        {
            usuarioRepository = new UsuarioRepository();
            usuarioGrupoBusiness = new UsuarioGrupoBusiness();
        }

        public int ListarTotal(UsuarioDto usuarioDto = null)
        {
            return usuarioRepository.ListarTotal(usuarioDto);
        }

        public UsuarioDto Login(UsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                throw new BusinessException("usuarioDto é nulo");

            if (string.IsNullOrEmpty(usuarioDto.Login))
                throw new BusinessException("Login é obrigatório");

            if (string.IsNullOrEmpty(usuarioDto.Senha))
                throw new BusinessException("Senha é obrigatório");

            var usuario = usuarioRepository.Login(usuarioDto.Login, usuarioDto.Senha);

            if (usuario == null)
                throw new BusinessException("Usuário e/ou senha inválidos");

            if (!usuario.Ativo.GetValueOrDefault())
                throw new BusinessException("Usuário inativo");

            return usuario;
        }

        public List<UsuarioDto> Listar(UsuarioDto usuarioDto = null)
        {
            return usuarioRepository.Listar(usuarioDto);
        }

        public int Incluir(UsuarioDto usuarioDto)
        {
            var idUsuario = 0;

            if (usuarioDto == null)
                throw new BusinessException("usuarioDto é nulo");

            if (string.IsNullOrEmpty(usuarioDto.Login))
                throw new BusinessException("Login é obrigatório");

            if (string.IsNullOrEmpty(usuarioDto.Senha))
                throw new BusinessException("Senha é obrigatório");

            if (usuarioDto.UsuarioGrupoDto == null || usuarioDto.UsuarioGrupoDto.Count() <= 0)
                throw new BusinessException("Grupo é obrigatório");

            var existeGrupoPorLogin = usuarioRepository.Existe(new UsuarioDto() { Login = usuarioDto.Login });

            if (existeGrupoPorLogin)
                throw new BusinessException($"Usuário ({usuarioDto.Login}) já está cadastrado");

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                idUsuario = usuarioRepository.Incluir(usuarioDto);

                foreach (var usuarioGrupo in usuarioDto.UsuarioGrupoDto)
                {
                    usuarioGrupo.IdUsuario = idUsuario;
                    usuarioGrupoBusiness.Incluir(usuarioGrupo);
                }

                scope.Complete();
            }

            return idUsuario;
        }

        public void Alterar(UsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                throw new BusinessException("usuarioDto é nulo");

            if (usuarioDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            var existeUsuarioPorId = usuarioRepository.Obter(new UsuarioDto() { Id = usuarioDto.Id });

            if (existeUsuarioPorId == null)
                throw new BusinessException($"Usuário ({usuarioDto.Id}) não está cadastrado");

            if (!string.IsNullOrEmpty(usuarioDto.Login))
            {
                if (usuarioDto.Login != existeUsuarioPorId.Login)
                {
                    var existeUsuarioPorLogin = usuarioRepository.Existe(new UsuarioDto() { Login = usuarioDto.Login });

                    if (existeUsuarioPorLogin)
                        throw new BusinessException($"Usuário ({usuarioDto.Login}) já está cadastrado");
                }
            }

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                usuarioRepository.Alterar(usuarioDto);

                if (usuarioDto.UsuarioGrupoDto != null && usuarioDto.UsuarioGrupoDto.Count() > 0)
                {
                    // usuário grupo
                    usuarioGrupoBusiness.Excluir(usuarioDto.Id);

                    foreach (var usuarioGrupoDto in usuarioDto.UsuarioGrupoDto)
                        usuarioGrupoBusiness.Incluir(usuarioGrupoDto);
                }

                scope.Complete();
            }
        }

        public void AlterarStatus(UsuarioDto usuarioDto)
        {
            if (usuarioDto == null)
                throw new BusinessException("usuarioDto é nulo");

            if (usuarioDto.Id <= 0)
                throw new BusinessException("Id é obrigatório");

            if (!usuarioDto.Ativo.HasValue)
                throw new BusinessException("Ativo é obrigatório");

            var existeUsuarioPorId = usuarioRepository.Existe(new UsuarioDto() { Id = usuarioDto.Id });

            if (!existeUsuarioPorId)
                throw new BusinessException($"Usuário ({usuarioDto.Id}) não está cadastrado");

            if (usuarioDto.Ativo.HasValue)
            {
                if (!usuarioDto.Ativo.Value)
                {
                    if (usuarioDto.IdUsuarioInativacao <= 0)
                        throw new BusinessException("IdUsuarioInativação é obrigatório");

                    if (usuarioDto.DataInativacao.GetValueOrDefault() == DateTime.MinValue)
                        throw new BusinessException("Data Inativação é obrigatório");
                }
            }

            usuarioRepository.Alterar(usuarioDto);
        }
    }
}