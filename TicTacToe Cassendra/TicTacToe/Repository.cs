using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Repository : IRepository
    {
        public bool Add(User user)
        {
            try
            {
                user.ID = Guid.NewGuid();
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                var session = cluster.Connect("TicTacToe");
                var command= session.Prepare("INSERT INTO \"GameUser\" (id,accesstokken,email,firstname,lastname,status,username) VALUES (?,?,?,?,?,?,?)");
               var execution = command.Bind(user.ID, Guid.NewGuid(), user.firstName,user.lastName,user.Email,"false", user.userName);   
                session.Execute(execution);
                return true;
          
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string getUserId(string tokken)
        {
            try
            {

                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                var session = cluster.Connect("TicTacToe");
                Row command = session.Execute("select id from \"GameUser\" where accesstokken= "+tokken+"").First();
                string token = command["id"].ToString();
                return token;
          }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string getTokken(string id)
        {
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                var session = cluster.Connect("TicTacToe");
                Row command = session.Execute("select \"accesstokken\" from \"GameUser\" where id= "+id+"").First();
               
                string token = command["accesstokken"].ToString();
                return token;
               
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public int KeyCheck(string tokken)
        {
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                var session = cluster.Connect("TicTacToe");
                Row command = session.Execute("select count(*) from \"GameUser\" where accesstokken= "+tokken+" allow filtering").First();
                int token = Convert.ToInt32(command.Count());
                if (token > 0)
                    return 1;
                else if (tokken == "")
                    return 2;
                else
                    return 0; 
               
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Case");
            }

        }
        public bool AddLog(Log log)
        {
            try
            {
                Cluster cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                ISession session = cluster.Connect("TicTacToe");
                var command = session.Prepare("INSERT INTO \"LoggerDB\"(\"Id\",\"Request\",\"Response\",\"Exception\",\"Time\") VALUES(?,?,?,?,?)");
                var execution = command.Bind(Guid.NewGuid(), log.Request,log.Response,log.Exception,log.Time);
                session.Execute(execution);
                return true;
           
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
