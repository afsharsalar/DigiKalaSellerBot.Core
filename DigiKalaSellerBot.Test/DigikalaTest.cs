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
        public void ChangePriceTest()
        {

            //arrange
            var dkp = 272377;
            var dkpc = 1957080;
            var price = 3_800_000;
            var login=new LoginModel{Email = "", Password = "" };

            //act
            var result = _digikala.ChangePrice(dkpc, dkp, price, login);

            //assert
            Assert.AreEqual(true,result.status);

            


        }
    }
}
