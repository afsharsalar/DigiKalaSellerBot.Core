using System.Linq;
using DigiKalaSellerBot.Core.Helper;
using DigiKalaSellerBot.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigiKalaSellerBot.Test
{
    [TestClass]
    public class DigikalaTest
    {

        private readonly Digikala _digikala;

        public DigikalaTest()
        {
            _digikala=new Digikala();
        }

        [TestMethod]
        public void GetProductInfoTest()
        {
            //arrange
            
            var dkp = 272377;
            var title = "فرش ماشینی ساوین طرح مهر کد FSM19 زمینه کرم";

            //act
            var data = _digikala.GetProductInfo(dkp);

            //assert
            Assert.AreEqual(title,data.Title);
        }


        [TestMethod]
        public void GetProductStockInfo()
        {
            //arrange

            var dkp = 272377;
            var dkpc = 1957080;
            var login = new LoginModel { Email = "", Password = "" };

            //act

            _digikala.Login(login);
            var data = _digikala.GetStockInfo(dkpc,dkp);

            //assert
            Assert.AreEqual(4000, data.Weight);
            Assert.AreEqual(150, data.Length);
        }


        [TestMethod]
        public void GetNotBuyBoxList()
        {
            //arrange            
            var login = new LoginModel { Email = "", Password = "" };

            //act

            _digikala.Login(login);
            var data = _digikala.GetSellerProducts(buyBoxWinner:false);

            //assert
            Assert.AreEqual(true, data.Any(p=>p.product_variant_id== 18830572));
            
        }


        [TestMethod]
        public void ChangePriceTest()
        {

            //arrange
            var dkp = 272377;
            var dkpc = 1957080;
            var price = 3_800_000;
            var login = new LoginModel { Email = "", Password = "" };

            //act
            _digikala.Login(login);
            var result = _digikala.ChangePrice(dkpc, dkp, price);

            //assert
            Assert.AreEqual(true,result.status);

            


        }

        [TestMethod]
        public void CommentTest()
        {
            var data = _digikala.GetComments(3868296);
            Assert.AreEqual(true,data.Any());
        }

        [TestMethod]
        public void PromotionTest()
        {
            //arrange
            var dkp = 4452500;
            var dkpc = 19806737;
            var price = 1_540_000;
            var login = new LoginModel { Email = "", Password = "" };

            //act
            _digikala.Login(login);
            var result = _digikala.ChangePromotionPrice(dkpc, dkp, price);

            //assert
            Assert.AreEqual(true, result.status);
        }



        [TestMethod]
        public void CustomerTest()
        {
            //arrange
            
            var login = new LoginModel { Email = "", Password = "" };

            //act
            _digikala.Login(login);
            var result = _digikala.GetCustomers();

            //assert
            Assert.AreEqual(true, result.Any());
        }
    }
}
