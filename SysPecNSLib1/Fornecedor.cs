using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SysPecNSLib
{
    public class Fornecedor
    {
        public int Id { get; set; }
        public string? RazaoSocial { get; set; }
        public string? Fantasia { get; set; }
        public string? Cnpj { get; set; }
        public string? Contato { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        public Fornecedor()
        {

        }
        public Fornecedor(int id, string? razaoSocial, string? fantasia, string? cnpj, string? contato, string? telefone, string? email)
        {
            Id = id;
            RazaoSocial = razaoSocial;
            Fantasia = fantasia;
            Cnpj = cnpj;
            Contato = contato;
            Telefone = telefone;
            Email = email;
        }
        public Fornecedor(string? razaoSocial, string? fantasia, string? cnpj, string? contato, string? telefone, string? email)
        {
            RazaoSocial = razaoSocial;
            Fantasia = fantasia;
            Cnpj = cnpj;
            Contato = contato;
            Telefone = telefone;
            Email = email;
        }

        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "sp_fornecedor_insert";
            cmd.Parameters.AddWithValue("sprazao_social", RazaoSocial);
            cmd.Parameters.AddWithValue("spfantasia", Fantasia);
            cmd.Parameters.AddWithValue("spcnpj", Cnpj);
            cmd.Parameters.AddWithValue("spcontato", Contato);
            cmd.Parameters.AddWithValue("sptelefone", Telefone);
            cmd.Parameters.AddWithValue("spemail", Email);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Id = dr.GetInt32(0);
            }
        }
        public static Fornecedor ObterPorId(int id)
        {
            Fornecedor fornecedor = new();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = $"select * from fornecedores where id = {id}";
            var dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                fornecedor = new(
                   dr.GetInt32(0),
                   dr.GetString(1),
                   dr.GetString(2),
                   dr.GetString(3),
                   dr.GetString(4),
                   dr.GetString(5),
                   dr.GetString(6)
                   );
            }
            return fornecedor;

        }
        public static List<Fornecedor> ObterLista(string? fantasia = "")
        {
            List<Fornecedor> lista = new();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            if (fantasia == "")
            {
                cmd.CommandText = "select * from fornecedores order by fantasia";
            }
            else
            {
                cmd.CommandText = $"select * from fornecedores where nome Like '%{fantasia}%' order by fantasia";
            }
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(
                    new(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        dr.GetString(4),
                        dr.GetString(5),
                        dr.GetString(6)
                        )
                    );
            }
            return lista;
        }
        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_fornecedor_update";
            cmd.Parameters.AddWithValue("spid", Id);
            cmd.Parameters.AddWithValue("sprazao_social", RazaoSocial);
            cmd.Parameters.AddWithValue("spfantasia", Fantasia);
            cmd.Parameters.AddWithValue("spcnpj", Cnpj);
            cmd.Parameters.AddWithValue("spcontato", Contato);
            cmd.Parameters.AddWithValue("sptelefone", Telefone);
            cmd.Parameters.AddWithValue("spemail", Email);
            cmd.ExecuteNonQuery();
            ;
        }
    }
}
