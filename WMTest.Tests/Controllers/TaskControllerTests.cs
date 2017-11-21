using Microsoft.VisualStudio.TestTools.UnitTesting;
using WMTest.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMTest.Models;
using Moq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace WMTest.Controllers.Tests
{
    [TestClass()]
    public class TaskControllerTests
    {
        #region Moqs

        List<FirstTaskModel> FirstModels = new List<FirstTaskModel>()
        {
            new FirstTaskModel() { ID = 1, Name = "Name1"},
            new FirstTaskModel() { ID = 2, Name = "Name2"},
            new FirstTaskModel() { ID = 3, Name = "Name3"}
        };
        public Mock<IFirstTaskRepository<FirstTaskModel, int>> MoqFirstRepo()
        {

            Mock<IFirstTaskRepository<FirstTaskModel, int>> MyMoq = new Mock<IFirstTaskRepository<FirstTaskModel, int>>();
            MyMoq.Setup(M => M.Add(It.IsAny<FirstTaskModel>()));
            MyMoq.Setup(so => so.Add(It.IsAny<FirstTaskModel>()))
                .Callback<FirstTaskModel>(l => l.ID = 100500);
            MyMoq.Setup(M => M.Update(It.IsAny<FirstTaskModel>()));
            MyMoq.Setup(M => M.Delete(It.IsAny<FirstTaskModel>()));
            MyMoq.Setup(M => M.Get()).Returns(FirstModels);
            MyMoq.Setup(M => M.Get(It.IsAny<Func<FirstTaskModel, bool>>())).Returns(FirstModels.GetRange(0, 2));
            MyMoq.Setup(M => M.FirstOrDefault(It.IsAny<Func<FirstTaskModel, bool>>())).Returns(FirstModels[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i < 3))).Returns(FirstModels[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i >= 3))).Returns((FirstTaskModel)null);
            MyMoq.Setup(M => M.GetOrAdd(It.Is<FirstTaskModel>(m => FirstModels.Select(F => F.Name).ToList().IndexOf(m.Name) >= 0))).Returns(1);
            MyMoq.Setup(M => M.GetOrAdd(It.Is<FirstTaskModel>(m => FirstModels.Select(F => F.Name).ToList().IndexOf(m.Name) < 0))).Returns(3333);
            return MyMoq;
        }
        List<SecondTaskModel> SecondModels = new List<SecondTaskModel>()
        {
            new SecondTaskModel() { ID = 1, Value  = 0 },
            new SecondTaskModel() { ID = 2, Value = 50000 },
            new SecondTaskModel() { ID = 3, Value = 10000 }
        };
        public Mock<ISecondTaskRepository<SecondTaskModel, int>> MoqSecondRepo()
        {

            Mock<ISecondTaskRepository<SecondTaskModel, int>> MyMoq = new Mock<ISecondTaskRepository<SecondTaskModel, int>>();
            MyMoq.Setup(M => M.Add(It.IsAny<SecondTaskModel>()));
            MyMoq.Setup(so => so.Add(It.IsAny<SecondTaskModel>()))
                .Callback<SecondTaskModel>(l => l.ID = 100500);
            MyMoq.Setup(M => M.Update(It.IsAny<SecondTaskModel>()));
            MyMoq.Setup(M => M.Delete(It.IsAny<SecondTaskModel>()));
            MyMoq.Setup(M => M.Get()).Returns(SecondModels);
            MyMoq.Setup(M => M.Get(It.IsAny<Func<SecondTaskModel, bool>>())).Returns(SecondModels.GetRange(0, 2));
            MyMoq.Setup(M => M.FirstOrDefault(It.IsAny<Func<SecondTaskModel, bool>>())).Returns(SecondModels[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i < 3))).Returns(SecondModels[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i >= 3))).Returns((SecondTaskModel)null);
            MyMoq.Setup(M => M.AddOrUpdate(It.Is<SecondTaskModel>(m => SecondModels.Select(F => F.ID).ToList().IndexOf(m.ID) > 0))).Returns(1);
            MyMoq.Setup(M => M.AddOrUpdate(It.Is<SecondTaskModel>(m => SecondModels.Select(F => F.ID).ToList().IndexOf(m.ID) < 0))).Returns(3333);
            return MyMoq;
        }
        public void RefModify(SecondTaskModel stm)
        {
            stm.ID = 100500;
        }
        List<ThirdTaskModel> Wallets = new List<ThirdTaskModel>()
        {
            new ThirdTaskModel() { ID = 1, Balance  = 0 },
            new ThirdTaskModel() { ID = 2, Balance = 10000 },
            new ThirdTaskModel() { ID = 3, Balance = 10000 }
        };

        public Mock<IThirdTaskRepository<ThirdTaskModel, int>> MoqThirdRepo()
        {

            Mock<IThirdTaskRepository<ThirdTaskModel, int>> MyMoq = new Mock<IThirdTaskRepository<ThirdTaskModel, int>>();
            MyMoq.Setup(M => M.Add(It.IsAny<ThirdTaskModel>()));
            MyMoq.Setup(so => so.Add(It.IsAny<ThirdTaskModel>()))
                .Callback<ThirdTaskModel>(l => l.ID = 100500);
            MyMoq.Setup(M => M.Update(It.IsAny<ThirdTaskModel>()));
            MyMoq.Setup(M => M.Delete(It.IsAny<ThirdTaskModel>()));
            MyMoq.Setup(M => M.Get()).Returns(Wallets);
            MyMoq.Setup(M => M.Get(It.IsAny<Func<ThirdTaskModel, bool>>())).Returns(Wallets.GetRange(0, 2));
            MyMoq.Setup(M => M.FirstOrDefault(It.IsAny<Func<ThirdTaskModel, bool>>())).Returns(Wallets[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i < 3))).Returns(Wallets[1]);
            MyMoq.Setup(M => M.Find(It.Is<int>(i => i >= 3))).Returns((ThirdTaskModel)null);
            MyMoq.Setup(M => M.TrasferMoney
            (
                It.Is<int> (i => i <= 0 || i > 2), It.IsAny<int>(), It.IsAny<decimal>()
            )).Returns(MoneyTransferResult.InvalidSource);
            MyMoq.Setup(M => M.TrasferMoney
            (
                It.IsAny<int>(), It.Is<int>(i => i <= 0 || i > 2), It.IsAny<decimal>()
            )).Returns(MoneyTransferResult.InvalidDestination);
            MyMoq.Setup(M => M.TrasferMoney
            (
                It.IsAny<int>(), It.IsAny<int>(), It.Is<decimal>(d => d > 10000 && d != 123456789)
            )).Returns(MoneyTransferResult.NotEnoughMoney);
            MyMoq.Setup(M => M.TrasferMoney
            (
                It.IsAny<int>(), It.IsAny<int>(), It.Is<decimal>(d =>  d == 123456789) //moq db failure or something
            )).Throws(new NotImplementedException("Random exception to emulate db malfunction or something like that")); ;
            MyMoq.Setup(M => M.TrasferMoney
            (
                It.Is<int>(i => i > 0 && i <= 2), It.Is<int>(i => i > 0 && i <= 2), It.Is<decimal>(d => d > 0 && d <= 10000)
            )).Returns(MoneyTransferResult.OK);

            return MyMoq;
        }
        #endregion
        [TestMethod()]
        public void Ctor_Test()
        {
            // Alloc 
            // Alloc is act for ctor
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object, 
                MoqSecondRepo().Object, 
                MoqThirdRepo().Object
            );

            // Assert
                // No excetion
        }

        [TestMethod()]
        public void GetOrAdd_GET_Test()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            HttpConfiguration configuration = new HttpConfiguration();
            HttpRequestMessage HttpRequest = new HttpRequestMessage();
            ctrl.Request = HttpRequest;
            ctrl.Request.Properties["MS_HttpConfiguration"] = configuration;
            //Act
            HttpResponseMessage qwe = ctrl.GetOrAdd("Name1");
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains("1"));

        }
        [TestMethod()]
        public void GetOrAdd_ADD_Test()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            HttpConfiguration configuration = new HttpConfiguration();
            HttpRequestMessage HttpRequest = new HttpRequestMessage();
            ctrl.Request = HttpRequest;
            ctrl.Request.Properties["MS_HttpConfiguration"] = configuration;
            //Act
            HttpResponseMessage qwe = ctrl.GetOrAdd("Vasya!");
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains("3333"));
        }
        [TestMethod()]
        public void GetOrAdd_ADD_INVALID_Test()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            string Vasya = "Vasya!";
            for (int i = 0; i < 10; i++) Vasya += Vasya;
            HttpConfiguration configuration = new HttpConfiguration();
            HttpRequestMessage HttpRequest = new HttpRequestMessage();
            ctrl.Request = HttpRequest;
            ctrl.Request.Properties["MS_HttpConfiguration"] = configuration;
            //Act
            HttpResponseMessage qwe = ctrl.GetOrAdd(Vasya);
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.BadRequest);
            Assert.IsNull(qwe.Content);
        }
        [TestMethod()]
        public void AddOrUpdate_ADD_Test()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            SecondTaskModel stm = new SecondTaskModel()
            {
                ID = 0,
                Value = 25
            };
            string str = new JavaScriptSerializer().Serialize(stm);
            //Act
            HttpResponseMessage qwe = ctrl.AddOrUpdate(str);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains("3333"));
        }
        [TestMethod()]
        public void AddOrUpdate_UPDATE_Test()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            SecondTaskModel stm = new SecondTaskModel()
            {
                ID = 1,
                Value = 25
            };
            string str = new JavaScriptSerializer().Serialize(stm);
            //Act
            HttpResponseMessage qwe = ctrl.AddOrUpdate(str);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.ToString().Contains((stm.ID - 1).ToString()));
        }

        [TestMethod()]
        public void TransferMoneyTest_SourceNotFound()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            int id1 = 100500;
            int id2 = 2;
            decimal amount = 100;
            //Act
            var qwe = ctrl.TransferMoney(id1, id2, amount);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains(MoneyTransferResult.InvalidSource.ToString()));
        }
        [TestMethod()]
        public void TransferMoneyTest_DestinationNotFound()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            int id1 = 2;
            int id2 = 100500;
            decimal amount = 100;
            //Act
            var qwe = ctrl.TransferMoney(id1, id2, amount);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains(MoneyTransferResult.InvalidDestination.ToString()));
        }
        [TestMethod()]
        public void TransferMoneyTest_NotEnoughMoney()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            int id1 = 1;
            int id2 = 2;
            decimal amount = 10000000;
            //Act
            var qwe = ctrl.TransferMoney(id1, id2, amount);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains(MoneyTransferResult.NotEnoughMoney.ToString()));
        }
        [TestMethod()]
        public void TransferMoneyTest_OK()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            int id1 = 1;
            int id2 = 2;
            decimal amount = 100;
            //Act
            var qwe = ctrl.TransferMoney(id1, id2, amount);
            string wer = qwe.Content.ReadAsStringAsync().Result;
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.OK);
            Assert.IsTrue(wer.Contains(MoneyTransferResult.OK.ToString()));
        }
        [TestMethod()]
        public void TransferMoneyTest_InternalErrorEmulation()
        {
            //Alloc
            TaskController ctrl = new TaskController
            (
                MoqFirstRepo().Object,
                MoqSecondRepo().Object,
                MoqThirdRepo().Object
            );
            int id1 = 1;
            int id2 = 2;
            decimal amount = 123456789;
            //Act
            var qwe = ctrl.TransferMoney(id1, id2, amount);
            //Assert
            Assert.AreEqual(qwe.StatusCode, System.Net.HttpStatusCode.BadRequest);
            Assert.IsNull(qwe.Content);
        }
    }
}