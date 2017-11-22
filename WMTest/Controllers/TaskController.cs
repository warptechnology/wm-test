using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using WMTest.Filters;
using WMTest.Models;
using WMTest.Utils;

namespace WMTest.Controllers
{
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {
        IFirstTaskRepository<FirstTaskModel, int> RepoFirst { set; get; }
        ISecondTaskRepository<SecondTaskModel, int> RepoSecond { set; get; }
        IThirdTaskRepository<ThirdTaskModel, int> RepoThird { set; get; }

        public TaskController(
            IFirstTaskRepository<FirstTaskModel, int> R1,
            ISecondTaskRepository<SecondTaskModel, int> R2,
            IThirdTaskRepository<ThirdTaskModel, int> R3
            )
        {
            RepoFirst = R1;
            RepoSecond = R2;
            RepoThird = R3;
        }
        //[Route("GetOrAdd"), HttpGet]
        [HttpGet]
        public HttpResponseMessage GetOrAdd(string value)
        {

            HttpResponseMessage Mes = new HttpResponseMessage();
            try
            {
                FirstTaskModel Model = new FirstTaskModel() { ID = 1, Name = value };

                Validate<FirstTaskModel>(Model);
                if (ModelState.IsValid)
                {
                    var qqwe = RepoFirst.GetOrAdd(Model).ToString();
                    Mes.Content = new StringContent(qqwe);
                    Mes.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    Mes.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                Mes.StatusCode = HttpStatusCode.InternalServerError;
            }
            return Mes;
        }
        [Route("AddOrUpdate"), HttpPost]
        //[HttpPost]
        public HttpResponseMessage AddOrUpdate([NakedBody] string request)
        {
            HttpResponseMessage Mes = new HttpResponseMessage();
            Mes.StatusCode = HttpStatusCode.BadRequest;
            if (null == request) return Mes;

            //SecondTaskModel value = JsonConvert.DeserializeObject<SecondTaskModel>(request);
            SecondTaskModel value = request.TryParseJson<SecondTaskModel>();
            if (null == value)
                return Mes;
            try
            {
                Mes.Content = new StringContent(RepoSecond.AddOrUpdate(value).ToString());
                Mes.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception Ex)
            {
                Mes.StatusCode = HttpStatusCode.InternalServerError;
            }
            return Mes;
        }


        [Route("TransferMoney"), HttpGet]
        //[HttpGet]
        public HttpResponseMessage TransferMoney(int id1, int id2, decimal amount)
        {
            HttpResponseMessage Mes = new HttpResponseMessage();
            Mes.StatusCode = HttpStatusCode.BadRequest;

            if (id1 <= 0 || id2 <= 0 || amount <= 0 || id1 == id2)
                return Mes;

            try
            {
                var obj = RepoThird.TrasferMoney(id1, id2, amount);
                Mes.Content = new StringContent(obj.ToString(), Encoding.UTF8);
                Mes.StatusCode = HttpStatusCode.OK;
            }
            catch
            {
                Mes.StatusCode = HttpStatusCode.InternalServerError;
            }
            return Mes;
        }

        [Route("Table"), HttpGet]
        public HttpResponseMessage Table(int value)
        {
            HttpResponseMessage Mes = new HttpResponseMessage();
            Mes.StatusCode = HttpStatusCode.BadRequest;
            List<Tuple<string, string>> Table = new List<Tuple<string, string>>();

            switch (value)
            {
                case 1:
                    List<FirstTaskModel> ftm = RepoFirst.Get().OrderBy(m => m.ID).ToList();
                    Table.AddRange(ftm.Select(m => new Tuple<string, string>(m.ID.ToString(), m.Name)));
                    break;
                case 2:
                    List<SecondTaskModel> stm = RepoSecond.Get().OrderBy(m => m.ID).ToList();
                    Table.AddRange(stm.Select(m => new Tuple<string, string>(m.ID.ToString(), m.Value.ToString())));
                    break;
                case 3:
                    List<ThirdTaskModel> ttm = RepoThird.Get().OrderBy(m => m.ID).ToList();
                    Table.AddRange(ttm.Select(m => new Tuple<string, string>(m.ID.ToString(), m.Balance.ToString())));
                    break;
                default:
                    return Mes;
            }
            string resp = new JavaScriptSerializer().Serialize(Table);
            Mes.Content = new StringContent(resp);
            Mes.StatusCode = HttpStatusCode.OK;
            return Mes;
        }



    }

}
