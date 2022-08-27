using AgendaContatos.Data.Configurations;
using AgendaContatos.Data.Entites;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Repositories
{
    public class ContatoRepository
    {
        //criar método na base de dados
        public void Create(Contato contato)
        {
            var sql = @"
                INSERT INTO CONTATO(
                     IDCONTATO,
                     NOME,
                     EMAIL,
                     TELEFONE,
                     DATANASCIMENTO,
                     IDUSUARIO)
             VALUES(
                    @Idcontato,
                    @Nome,
                    @Email,
                    @Telefone,
                    @DataNascimento,
                    @IdUsuario)
             ";
            using (var connection = new SqlConnection(SqlServerConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, contato);

            }
        }
        

        //método para atualizar um contato na base de dados
             public void Update(Contato contato)
            {
            var sql = @"
                  UPDATE CONTATO
                  SET
                      NOME = @Nome,
                      EMAIL = @Email,
                      TELEFONE = @Telefone,
                      DATANASCIMENTO =@DataNascimento
                   WHERE
                        IDCONTATO = @Idcontato
                        ";
                using (var connection = new SqlConnection(SqlServerConfiguration.GetConnectionString()))
                {
                    connection.Execute(sql, contato);

                }


            }
        //Método para deletar um contato da base de dados
        public void Delete(Contato contato)
        {
            var sql = @"
                 DELETE FROM CONTATO
                 WHERE IDCONTATO = @Idcontato
                        ";
            using (var connection = new SqlConnection(SqlServerConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, contato);

            }


        }

        //Método para consultar todos os contatos de 1 usuário

        public  List<Contato> GetByUsuario(Guid idUsuario)
        {
            var sql = @"
                SELECT * FROM CONTATO
                WHERE IDUSUARIO = @idUsuario
                ORDER BY NOME
                   ";
            using (var  connection = new SqlConnection (SqlServerConfiguration.GetConnectionString()))
            {
                return connection.Query<Contato>(sql, new { idUsuario }).ToList();

            }
        }
           //Método para consultar 1 contato baseado no id do contato e id do usuário
           public  Contato GetById(Guid idContato, Guid idUsuario)
        {
            var sql = @"
                SELECT * FROM CONTATO
                WHERE IDCONTATO = @idContato
                AND IDUSUARIO   = @idUsuario
          ";
            using (var connection = new SqlConnection(SqlServerConfiguration.GetConnectionString()))
            {
                return connection.Query<Contato>(sql, new { idContato, idUsuario }).FirstOrDefault();
            }
        }
         }
        }
    
