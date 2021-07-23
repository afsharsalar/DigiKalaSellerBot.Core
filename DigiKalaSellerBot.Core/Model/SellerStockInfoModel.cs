using System;
using System.Collections.Generic;
using System.Text;

namespace DigiKalaSellerBot.Core.Model
{
    public class SellerStockInfoModel
    {
        public int Length { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int Weight { set; get; }

        public int lead_time { set; get; }
        public int marketplace_seller_stock { set; get; }
        public int maximum_per_order { set; get; }
        public int oldSellerStock { set; get; }


        public int digi { set; get; }
        public int reserve { set; get; }


        public int rrpPrice { set; get; }
    }
}
