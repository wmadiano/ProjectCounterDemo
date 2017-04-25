using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace ProjectCounter.Controllers
{
    public class HomeController : Controller
    {

        String connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();

        [HttpGet]
        [ActionName("Index")]
        public ActionResult Index_Get()
        {
            ViewBag.Count = getCurrentCount();
            return View();
        }

        //Increments the counter if exists.
        //Inserts new counter if not exists.
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_Post()
        {
            Int16 curCount;

            curCount = Convert.ToInt16(ExecuteScalar("SELECT Count(1) FROM [Counter]"));

            if (curCount != 0)
            {
                ExecuteNonQuery("UPDATE [Counter] SET [Count] = [Count] + 1 WHERE [count] < 10");
            }
            else
            {
                ExecuteNonQuery("INSERT INTO [Counter]([Count])VALUES  (1);");
            }
            ViewBag.Count = getCurrentCount();
            return View();
        }


        //Sets the current counter to zero if not exists.
        //Get the actual counter value if exists.
        private int getCurrentCount()
        {
            Int16 curCount = 0;

            curCount = Convert.ToInt16(ExecuteScalar("SELECT Count(1) FROM [Counter]"));

            if (curCount != 0)
            {
                curCount = Convert.ToInt16(ExecuteScalar("SELECT [count] FROM [Counter]"));
            }

            return curCount;
        }


        #region Wrapper
        // Tools
        public void ExecuteNonQuery(string sqlCommandText)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand(sqlCommandText, con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                com.ExecuteNonQuery();
            }

        }

        public Object ExecuteScalar(string sqlCommandText)
        {
            Object ret;

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                System.Data.SqlClient.SqlCommand com = new System.Data.SqlClient.SqlCommand(sqlCommandText, con);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                ret = Convert.ToString(com.ExecuteScalar());
            }


            return ret;

        }
    }
    #endregion
}
