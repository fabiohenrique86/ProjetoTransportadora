using ProjetoTransportadora.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ProjetoTransportadora.Repository
{
    public class UsuarioRepository
    {
        private ProjetoTransportadoraEntities projetoTransportadoraEntities = null;

        public UsuarioRepository()
        {
            projetoTransportadoraEntities = new ProjetoTransportadoraEntities();
        }

        public UsuarioDto Obter(UsuarioDto UsuarioDto)
        {
            IQueryable<Usuario> query = projetoTransportadoraEntities.Usuario;

            if (UsuarioDto.Id > 0)
                query = query.Where(x => x.Id == UsuarioDto.Id);

            return query.Select(x => new UsuarioDto() { Id = x.Id, Login = x.Login }).FirstOrDefault();
        }

        public bool Existe(UsuarioDto usuarioDto)
        {
            IQueryable<Usuario> query = projetoTransportadoraEntities.Usuario;

            if (usuarioDto.Id > 0)
                query = query.Where(x => x.Id == usuarioDto.Id);

            if (!string.IsNullOrEmpty(usuarioDto.Login))
                query = query.Where(x => x.Login == usuarioDto.Login);

            return query.FirstOrDefault() != null ? true : false;
        }

        public UsuarioDto Login(string login, string senha)
        {
            IQueryable<Usuario> query = projetoTransportadoraEntities.Usuario.Where(x => x.Login == login && x.Senha == senha);

            var usuarioDto = query.
                    Select(x => new UsuarioDto()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Senha = x.Senha,
                        Ativo = x.Ativo,
                        IdUsuarioCadastro = x.IdUsuarioCadastro,
                        DataCadastro = x.DataCadastro,
                        IdUsuarioInativacao = x.IdUsuarioInativacao,
                        DataInativacao = x.DataInativacao,
                        TrocarSenha = x.TrocarSenha,
                        UsuarioGrupoDto = x.UsuarioGrupo.Select(w => new UsuarioGrupoDto()
                        {
                            Id = w.Id,
                            IdUsuario = w.IdUsuario,
                            IdGrupo = w.IdGrupo,
                            GrupoDto = new GrupoDto()
                            {
                                Id = w.IdUsuario,
                                Nome = w.Grupo.Nome,
                                Ativo = w.Grupo.Ativo,
                                DataCadastro = w.Grupo.DataCadastro,
                                IdUsuarioCadastro = w.Grupo.IdUsuarioCadastro,
                                DataInativacao = w.Grupo.DataInativacao,
                                IdUsuarioInativacao = w.Grupo.IdUsuarioInativacao,
                                GrupoFuncionalidadeDto = w.Grupo.GrupoFuncionalidade.Select(y => new GrupoFuncionalidadeDto()
                                {
                                    Id = y.Id,
                                    IdFuncionalidade = y.IdFuncionalidade,
                                    IdGrupo = y.IdGrupo,
                                    Inserir = y.Inserir,
                                    Ler = y.Ler,
                                    Atualizar = y.Atualizar,
                                    Excluir = y.Excluir,
                                    Executar = y.Executar,
                                    FuncionalidadeDto = new FuncionalidadeDto() { Id = y.IdFuncionalidade, Nome = y.Funcionalidade.Nome, Descricao = y.Funcionalidade.Descricao }
                                }).ToList()
                            }
                        }).ToList()
                    }).FirstOrDefault();

            return usuarioDto;
        }

        public List<UsuarioDto> Listar(UsuarioDto usuarioDto)
        {
            IQueryable<Usuario> query = projetoTransportadoraEntities.Usuario;

            if (usuarioDto != null)
            {
                if (usuarioDto.Id > 0)
                    query = query.Where(x => x.Id == usuarioDto.Id);

                if (!string.IsNullOrEmpty(usuarioDto.Login))
                    query = query.Where(x => x.Login.Contains(usuarioDto.Login));

                if (!string.IsNullOrEmpty(usuarioDto.Senha))
                    query = query.Where(x => x.Senha.Contains(usuarioDto.Senha));

                if (usuarioDto.Ativo.HasValue)
                    query = query.Where(x => x.Ativo == usuarioDto.Ativo);

                if (usuarioDto.TrocarSenha.HasValue)
                    query = query.Where(x => x.TrocarSenha == usuarioDto.TrocarSenha);

                if (usuarioDto.UsuarioGrupoDto.FirstOrDefault()?.IdGrupo > 0)
                {
                    var idGrupo = usuarioDto.UsuarioGrupoDto.FirstOrDefault().IdGrupo;
                    query = query.Where(x => x.UsuarioGrupo.Any(w => w.IdGrupo == idGrupo));
                }
            }

            return query.
                    Select(x => new UsuarioDto()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Senha = x.Senha,
                        Ativo = x.Ativo,
                        IdUsuarioCadastro = x.IdUsuarioCadastro,
                        DataCadastro = x.DataCadastro,
                        IdUsuarioInativacao = x.IdUsuarioInativacao,
                        DataInativacao = x.DataInativacao,
                        TrocarSenha = x.TrocarSenha,
                        UsuarioGrupoDto = x.UsuarioGrupo.Select(w => new UsuarioGrupoDto()
                        {
                            Id = w.Id,
                            IdUsuario = w.IdUsuario,
                            IdGrupo = w.IdGrupo,
                            GrupoDto = new GrupoDto()
                            {
                                Id = w.IdUsuario,
                                Nome = w.Grupo.Nome,
                                Ativo = w.Grupo.Ativo,
                                DataCadastro = w.Grupo.DataCadastro,
                                IdUsuarioCadastro = w.Grupo.IdUsuarioCadastro,
                                DataInativacao = w.Grupo.DataInativacao,
                                IdUsuarioInativacao = w.Grupo.IdUsuarioInativacao,
                                GrupoFuncionalidadeDto = w.Grupo.GrupoFuncionalidade.Select(y => new GrupoFuncionalidadeDto()
                                {
                                    Id = y.Id,
                                    IdFuncionalidade = y.IdFuncionalidade,
                                    IdGrupo = y.IdGrupo,
                                    Inserir = y.Inserir,
                                    Ler = y.Ler,
                                    Atualizar = y.Atualizar,
                                    Excluir = y.Excluir,
                                    Executar = y.Executar,
                                    FuncionalidadeDto = new FuncionalidadeDto() { Id = y.IdFuncionalidade, Nome = y.Funcionalidade.Nome, Descricao = y.Funcionalidade.Descricao }
                                }).ToList()
                            }
                        }).ToList()
                    }).OrderBy(x => x.Login).ToList();
        }

        public int Incluir(UsuarioDto usuarioDto)
        {
            var usuario = new Usuario()
            {
                Id = usuarioDto.Id,
                Ativo = true,
                Login = usuarioDto.Login,
                Senha = usuarioDto.Senha,
                DataCadastro = usuarioDto.DataCadastro,
                IdUsuarioCadastro = usuarioDto.IdUsuarioCadastro,
                TrocarSenha = usuarioDto.TrocarSenha,
            };

            projetoTransportadoraEntities.Usuario.Add(usuario);
            projetoTransportadoraEntities.SaveChanges();

            return usuario.Id;
        }

        public void Alterar(UsuarioDto usuarioDto)
        {
            var usuario = projetoTransportadoraEntities.Usuario.FirstOrDefault(x => x.Id == usuarioDto.Id);

            if (!string.IsNullOrEmpty(usuarioDto.Login))
                usuario.Login = usuarioDto.Login;

            if (!string.IsNullOrEmpty(usuarioDto.Senha))
                usuario.Senha = usuarioDto.Senha;

            if (usuarioDto.TrocarSenha.HasValue)
                usuario.TrocarSenha = usuarioDto.TrocarSenha.Value;

            if (usuarioDto.Ativo.HasValue)
            {
                usuario.Ativo = usuarioDto.Ativo.Value;
                usuario.IdUsuarioInativacao = usuarioDto.IdUsuarioInativacao;
                usuario.DataInativacao = usuarioDto.DataInativacao;
            }

            projetoTransportadoraEntities.Entry(usuario).State = EntityState.Modified;
            projetoTransportadoraEntities.SaveChanges();
        }
    }
}
