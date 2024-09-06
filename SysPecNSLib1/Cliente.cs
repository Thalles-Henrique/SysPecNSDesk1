using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysPecNSLib
{
    public class Cliente
    {
        //nome, cpf, telefone, email, data_nasc, data_cad, ativo
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? Data_Nasc { get; set; }
        public DateTime? Data_Cad { get; set; } = DateTime.Now;
        public bool Ativo { get; set; }


        //com id sem endereco

        //sem id sem endereco sem ativo

        //executescalar



            



        public Cliente()
        {
            //Construtor vazio
        }

        public Cliente(int id, string? nome, string? cpf, string? telefone, string? email, DateTime? data_Nasc, DateTime data_Cad, bool ativo)
        {
            //Construtor com todos os campos
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Telefone = telefone;
            Email = email;
            Data_Nasc = data_Nasc;
            Data_Cad = data_Cad;
            Ativo = ativo;

        }

        public Cliente(string? nome, string? cpf, string? telefone, string? email, DateTime? data_Nasc, DateTime? data_Cad)
        {
            //Construtor sem o ID, para inserção
            Nome = nome;
            Cpf = cpf;
            Telefone = telefone;
            Email = email;
            Data_Nasc = data_Nasc;
            Data_Cad = data_Cad;

        }

        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_cliente_insert";
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("spcpf", Cpf);
            cmd.Parameters.AddWithValue("sptelefone", Telefone);
            cmd.Parameters.AddWithValue("spemail", Email);
            cmd.Parameters.AddWithValue("spdatanasc", Data_Nasc);

            //Executa uma consulta e retorna a primeira coluna da primeira linha, retorna um único valor do banco de dados.
            //Id = Convert.ToInt32(cmd.ExecuteScalar());

            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Id = dr.GetInt32(0);
            }


        }

        public static Cliente ObterPorId(int id)
        {
            Cliente cliente = new();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"SELECT * from clientes where id = {id}";
            var dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                cliente = new(
                   dr.GetInt32(0),//id
                   dr.GetString(1),//nome
                   dr.GetString(2),//cpf
                   dr.GetString(3),//telefone
                   dr.GetString(4),//email
                   dr.GetDateTime(5),//data_Nasc
                   dr.GetDateTime(6),//data_Cad
                   dr.GetBoolean(7)//ativo
                                   //Endereco.ObterListaPorCliente(dr.GetInt32(0))
                   );

            }
            return cliente;
        }

        public static List<Cliente> ObterLista(string? nome = "")
        {
            List<Cliente> lista = new();
            var comandosSQL = Banco.Abrir();
            comandosSQL.CommandType = CommandType.Text;
            if (nome == "")
            {
                comandosSQL.CommandText = "select * from clientes order by nome";
            }
            else
            {
                comandosSQL.CommandText = $"select * from clientes where nome like '%{nome}%' order by nome";
            }

            var dr = comandosSQL.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new(
                   dr.GetInt32(0),//id
                   dr.GetString(1),//nome
                   dr.GetString(2),//cpf
                   dr.GetString(3),//telefone
                   dr.GetString(4),//email
                   dr.GetDateTime(5),//data_Nasc
                   dr.GetDateTime(6),//data_Cad
                   dr.GetBoolean(7)//ativo
                    )

               );
            }
            //clientes
            return lista;

        }

        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_cliente_update";
            cmd.Parameters.AddWithValue("spid", Id);
            cmd.Parameters.AddWithValue("spnome", Nome);
            cmd.Parameters.AddWithValue("sptelefone", Telefone);
            cmd.Parameters.AddWithValue("spdatanasc", Data_Nasc);

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public static void Arquivar(int id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"update clientes set ativo = 0 where id = {id}";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public static void Restaurar(int id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"update clientes set ativo = 1 where id = {id}";
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }
    }
}
