using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//API de criptografia
using Org.BouncyCastle.Crypto.Prng;

namespace SysPecNSLib
{
    //private static mysqlcommand comando = Banco.Abrir();
    public class Usuario
    {

        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public Nivel Nivel { get; set; }
        public bool Ativo { get; set; }
        public Usuario()
        {
            Nivel = new();
        }
        public Usuario(string? nome, string? email, string? senha, Nivel nivel)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;

        }
        public Usuario(string? nome, string? email, string? senha, Nivel nivel, bool ativo)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;
            Ativo = ativo;
        }
        public Usuario(int id, string? nome, string? email, string? senha, Nivel nivel, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;
            Ativo = ativo;
        }

        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_usuario_insert";
            //AddWithValue: adicionar um parâmetro com nome e valor, ("nome do parametro no banco", objeto)
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("spemail", Email);
            cmd.Parameters.AddWithValue("spsenha", Senha);
            cmd.Parameters.AddWithValue("spnivel", Nivel.Id);
            //Para criar um SqlDataReader, você deve chamar o ExecuteReader: método do SqlCommand objeto, em vez de usar diretamente um construtor.
            //Lê um fluxo de encaminhamento de linhas com base em um banco de dados
            //dr = datareader
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Id = dr.GetInt32(0);
            }

        }
        /// <summary>
        /// Obter por Id é um método estático que retorna
        /// um objeto usuário completo baseado no id informado
        /// </summary>
        /// <param name="id">Id do usuário buscado</param>
        /// <returns>Objeto Usuário com todos os campos</returns>
        public static Usuario ObterPorId(int id)
        {
            Usuario usuario = new();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select * from usuarios where id = {id}";
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                usuario = new(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.GetString(3),
                    Nivel.ObterPorId(dr.GetInt32(4)),
                    dr.GetBoolean(5)
                    );
            }
            return usuario;
        }
        public static List<Usuario> ObterLista(string? nome = "")
        {
            List<Usuario> lista = new();
            var comandosSQL = Banco.Abrir();
            comandosSQL.CommandType = CommandType.Text;
            if (nome == "")
            {
                comandosSQL.CommandText = "select * from usuarios order by nome";

            }
            else
            {
                comandosSQL.CommandText = $"select * from usuarios where nome LIKE '%{nome}%' order by nome";

            }
            var dr = comandosSQL.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        Nivel.ObterPorId(dr.GetInt32(4)),
                        dr.GetBoolean(5)
                        )
                    );
            }
            return lista;
        }
        public static Usuario EfetuarLogin(string email, string senha)
        {
            Usuario usuario = new();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select * from usuarios where email = '{email}' and senha = md5('{senha}')";
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                usuario = new(
                   dr.GetInt32(0),
                   dr.GetString(1),
                   dr.GetString(2),
                   dr.GetString(3),
                   Nivel.ObterPorId(dr.GetInt32(4)),
                   dr.GetBoolean(5)
                   );
            }
            return usuario;
        }
        public void Atualizar()
        {
            // usuario: nome, senha, nível...
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_usuario_altera";
            cmd.Parameters.AddWithValue("spid", Id);
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("spsenha", Senha);
            cmd.Parameters.AddWithValue("spnivel", Nivel.Id);
            //Parâmetros de Saída
            //cmd.Parameters.Add("spid", MySqlDbType.VarChar).Direction = ParameterDirection.Output;
            //ExecuteNonQuery: quando a procedure não retorna nenhum dado
            cmd.ExecuteNonQuery();

            cmd.Connection.Close();

        }
        public static void Arquivar(int id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandType |= CommandType.Text;
            cmd.CommandText = $"update usuarios set ativo = 0 where id = {id}";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
        public static void Restaurar(int id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandType |= CommandType.Text;
            cmd.CommandText = $"update usuarios set ativo = 1 where id = {id}";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void Excluir()
        {
            // em geral nada se exclui de uma tabela...
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"delete from usuarios where id = {Id}";
            cmd.ExecuteNonQuery();


        }


    }
}
