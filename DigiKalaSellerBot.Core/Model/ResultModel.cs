namespace DigiKalaSellerBot.Core.Model
{
    public class ResultModel
    {
        public bool status { set; get; }

        public ResultDetailModel data { set; get; }

        public SellerStockInfoModel info { set; get; }

        public int NewPrice { set; get; }
    }

    public class ResultDetailModel
    {
        public string price_sale { set; get; }

        public int price_sale_latin { set; get; }


        public string seller_physical_stock { set; get; }


    }
}
