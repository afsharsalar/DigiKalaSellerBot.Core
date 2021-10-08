using System;
using System.Collections.Generic;
using System.Text;

namespace DigiKalaSellerBot.Core.Model
{
    public class ProductSearchModel
    {
        public string Status { set; get; }

        public ProductDetailSearchModel data { set; get; }
    }

    public class ProductDetailSearchModel
    {
        public List<ProductItemDetail> Items { set; get; }
    }

    public class ProductItemDetail
    {
        public int id { get; set; }
        public string image_src { get; set; }
        public string main_category_title { get; set; }
        public int product_id { get; set; }
        public string product_url { get; set; }
        public int product_variant_id { get; set; }
        public object supplier_code { get; set; }
        public string product_moderation_status { get; set; }
        public string product_variant_title { get; set; }
        public string active { get; set; }
        public string lead_time { get; set; }
        public int lead_time_latin { get; set; }
        public string price_list { get; set; }
        public string price_list_latin { get; set; }
        public string price_sale { get; set; }
        public string promo_price { get; set; }
        public int price_sale_latin { get; set; }
        public string marketplace_seller_stock { get; set; }
        public int marketplace_seller_stock_latin { get; set; }
        public string warehouse_stock { get; set; }
        public int warehouse_stock_latin { get; set; }
        public string reservation { get; set; }
        public int reservation_latin { get; set; }
        public string left_consumer { get; set; }
        public int left_consumer_latin { get; set; }
        public string maximum_per_order { get; set; }
        public int maximum_per_order_latin { get; set; }
        public string allowed_count { get; set; }
        public bool isActive { get; set; }
        public bool ovl_selling_active { get; set; }
        public Cpo_Campaign_Status cpo_campaign_status { get; set; }
        public int max_lead_time { get; set; }
        public string price_type { get; set; }
        public int buy_box_price { get; set; }
        public string buy_box_price_formatted { get; set; }
        public bool is_buy_box_winner { get; set; }
        public bool is_seller_buy_box_winner { get; set; }
        public bool is_in_buy_box_challenge { get; set; }
        public string min_selling_price_limit { get; set; }
        public Product_Selling_Chanel product_selling_chanel { get; set; }
        public Variant_Selling_Chanel variant_selling_chanel { get; set; }
        public bool is_incredible_promotion { get; set; }
        public bool is_in_promotion { get; set; }
        public int shipping_nature_id { get; set; }
        public int default_selling_chanel_code { get; set; }
        public object rating { get; set; }
        public bool is_promotion_management_visible_for_seller { get; set; }
        public bool is_archived { get; set; }
        public int fulfilment_and_delivery_cost { get; set; }
        public int seller_reservation { get; set; }
        public int digikala_reservation { get; set; }
        public int seller_shipping_lead_time { get; set; }
        public Shipping_Options shipping_options { get; set; }


    }


    

    public class Cpo_Campaign_Status
    {
        public bool can_add_variant_to_campaign { get; set; }
        public string error { get; set; }
        public bool is_variant_added_to_campaign { get; set; }
        public bool is_campaign_paused_by_seller { get; set; }
    }

    public class Product_Selling_Chanel
    {
        public bool active_digikala { get; set; }
        public bool active_digistyle { get; set; }
    }

    public class Variant_Selling_Chanel
    {
        public bool active_digikala { get; set; }
        public bool active_digistyle { get; set; }
    }

    public class Shipping_Options
    {
        public bool is_fbs_ability_enable { get; set; }
        public bool is_fbd_active { get; set; }
        public bool is_fbs_active { get; set; }
        public bool is_needed_fbs_setting { get; set; }
        public bool is_sbs_module_active { get; set; }
    }

}
