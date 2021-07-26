using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DigiKalaSellerBot.Core.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;

namespace DigiKalaSellerBot.Core.Helper
{
    public  class Digikala
    {
        #region Fields
        private RestClient _restClient;
        private List<RestResponseCookie> _cookies;
        private RestRequest _restRequest;
        private IRestResponse _response;

        #endregion

        #region Ctor
        public Digikala()
        {
            _restRequest = new RestRequest(Method.POST);
            _response = new RestResponse();
            _restClient = new RestClient();
            _cookies = new List<RestResponseCookie>();
        }
        #endregion

        #region Login
        public  void Login(LoginModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "مشخصات ورود به پنل سلر دیجی کالا را وارد نمایید");

            if (string.IsNullOrEmpty(model.Email))
                throw new ArgumentNullException(nameof(model.Email), "ایمیل ورود به پنل سلر دیجی کالا را وارد نمایید");
            if (string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException(nameof(model.Password), "کلمه عبور ورود به پنل سلر دیجی کالا را وارد نمایید");


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            _restClient = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            _restRequest = new RestRequest(Method.GET);
            _restClient.FollowRedirects = false;
            _restRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
            foreach (RestResponseCookie cookie in _restClient.Execute(_restRequest).Cookies)
            {
                _cookies.Add(cookie);
            }
            _restClient = new RestClient("https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Host", "seller.digikala.com");
            _restRequest.AddHeader("Referer", "https://seller.digikala.com/account/login/?_back=https://seller.digikala.com/");
            _restRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
            string[] textArray1 = new string[] { "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[email]\"\r\n\r\n", model.Email, "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"login[password]\"\r\n\r\n", model.Password, "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--" };
            _restRequest.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", string.Concat(textArray1), ParameterType.RequestBody);
            _restClient.FollowRedirects = false;
            Thread.Sleep(100);
            foreach (RestResponseCookie cookie2 in _cookies)
            {
                _restRequest.AddCookie(cookie2.Name, cookie2.Value);
            }
            _response = _restClient.Execute(_restRequest);
            if (_response.StatusCode == HttpStatusCode.Found)
            {
                foreach (RestResponseCookie cookie3 in _response.Cookies)
                {
                    _cookies.Add(cookie3);
                }
            }
        }
        #endregion

        #region GetProductInfo
        /// <summary>
        /// دریافت اطلاعات محصول
        /// </summary>
        /// <param name="dkp">کد محصول دیجی کالا</param>
        /// <returns></returns>
        public ProductModel GetProductInfo(int dkp)
        {
            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var url = $"https://www.digikala.com/product/dkp-{dkp}";
                var content = client.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(content);
                var title = doc.DocumentNode.SelectSingleNode("//h1[@class='c-product__title']");
                var variantsNodeFromScript = doc.DocumentNode.SelectNodes("//script")[9];
                if (!variantsNodeFromScript.InnerText.Contains("variants"))
                    throw new Exception("script not found");
                var checkstartIndex = variantsNodeFromScript.InnerText.IndexOf("variants", variantsNodeFromScript.InnerText.IndexOf("variants") + 1);
                if (checkstartIndex < 0)
                {
                    return new ProductModel
                    {
                        Data = new List<VariantModel>(),
                        Title = title.InnerText,
                        Dkp = dkp
                    };
                }
                if (!variantsNodeFromScript.InnerText.Contains("var variants"))
                {
                    throw new Exception("Not found");
                }
                var startIndex = variantsNodeFromScript.InnerText.IndexOf("{");
                var lastIndex = variantsNodeFromScript.InnerText.IndexOf("}};") + 2;
                var variantsScript = variantsNodeFromScript.InnerText.Substring(startIndex, lastIndex - startIndex);

                string encoded = DecodeEncodedNonAsciiCharacters(variantsScript);


                var variantNode = doc.DocumentNode.SelectNodes("//div[contains(@class,'c-table-suppliers__row js-supplier')]");
                var variants = new List<int>();
                if (variantNode == null)
                    throw new Exception("Not found");
                foreach (var item in variantNode)
                {
                    var id = Convert.ToInt32(item.Attributes["data-variant"].Value);
                    variants.Add(id);
                    encoded = encoded.Replace($"\"{id}\":", "");
                }
                encoded = "[" + encoded.Substring(1, encoded.Length - 2) + "]";

                encoded = encoded.Replace("\"color\":[],", "").Replace("\"size\":[],", "");
                var data = JsonConvert.DeserializeObject<List<VariantModel>>(encoded);
                //return data;
                return new ProductModel
                {
                    Data = data,
                    Title = title.InnerText.Trim(),
                    Dkp = dkp
                };
            }
        }

        #endregion

        #region ChangePrice
        /// <summary>
        /// کاهش قیمت محصول
        /// </summary>
        /// <param name="dkpc">کد تنوع محصول موردنظر</param>
        /// <param name="dkp">کد محصول دیجی کالا</param>
        /// <param name="newPrice">قیمت جدید به ریال</param>
        /// <param name="login">مشخصات ورود به پنل</param>
        /// <returns></returns>
        public ResultModel ChangePrice(int dkpc, int dkp, int newPrice)
        {   
            var product = GetProductInfo(dkp);
            if(product==null)
                throw new ArgumentNullException(nameof(dkp),"محصول مورد نظر یافت نشد");

            if(product.Data.All(q => q.price_list.variant_id != dkpc))
                throw new ArgumentNullException(nameof(dkpc), "تنوع مورد نظر یافت نشد");           
            

            var info = GetStockInfo(dkpc, dkp);

            _restClient = new RestClient("https://seller.digikala.com/content/edit/product/variant/save/");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
            foreach (RestResponseCookie cookie4 in _cookies)
            {
                _restRequest.AddCookie(cookie4.Name, cookie4.Value);
            }


            _restRequest.AddParameter("product_variant[product_id]", dkp);
            _restRequest.AddParameter("product_variant[product_variant_id]", dkpc);
            _restRequest.AddParameter("product_variant[shipping_type_digikala]", 1);
            _restRequest.AddParameter("product_variant[active]", "on");
            _restRequest.AddParameter("product_variant[lead_time]", info.lead_time);
            _restRequest.AddParameter("product_variant[order_limit]", info.maximum_per_order);
            _restRequest.AddParameter("product_variant[marketplace_seller_stock]", info.marketplace_seller_stock);
            _restRequest.AddParameter("product_variant[marketplace_seller_old_stock]", info.oldSellerStock);
            _restRequest.AddParameter("product_variant[supplier_code]", "01");
            _restRequest.AddParameter("product_variant[price]", newPrice);
            if (info.Length > 0)
                _restRequest.AddParameter("product_variant[package_length]", info.Length);
            if (info.Width > 0)
                _restRequest.AddParameter("product_variant[package_width]", info.Width);
            if (info.Height > 0)
                _restRequest.AddParameter("product_variant[package_height]", info.Height);
            if (info.Weight > 0)
                _restRequest.AddParameter("product_variant[package_weight]", info.Weight);

            _response = _restClient.Execute(_restRequest);
            var result = JsonConvert.DeserializeObject<ResultModel>(_response.Content);
            return result;
        }
        #endregion

        #region GetStockInfo

        public SellerStockInfoModel GetStockInfo(int dkpc,int dkp)
        {
            

            _restClient = new RestClient("https://seller.digikala.com/content/create/product/variant/search/");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
            foreach (RestResponseCookie cookie4 in _cookies)
            {
                _restRequest.AddCookie(cookie4.Name, cookie4.Value);
            }
            _restRequest.AddParameter("items", 50);
            _restRequest.AddParameter("page", 1);
            _restRequest.AddParameter("params[ref]", "conf");
            _restRequest.AddParameter("search[product_id]", dkp);



            _response = _restClient.Execute(_restRequest);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_response.Content);

            var tr = doc.DocumentNode.SelectSingleNode($"//tr[contains(@id,'productVariantEditRow_{dkpc}')]");
            if (tr != null)
            {
                var thisRow = new HtmlDocument();
                thisRow.LoadHtml(tr.InnerHtml);

                var length = thisRow.DocumentNode.SelectSingleNode("//input[contains(@name,'product_variant[package_length]')]");
                var packageLength = length != null ? length.Attributes["value"].Value : "0";

                var width = thisRow.DocumentNode
                    .SelectSingleNode("//input[contains(@name,'product_variant[package_width]')]");
                var packageWidth = width != null ? width.Attributes["value"].Value : "0";

                var height = thisRow.DocumentNode
                    .SelectSingleNode("//input[contains(@name,'product_variant[package_height]')]");
                var packageHeight = height != null ? height.Attributes["value"].Value : "0";

                var weight = thisRow.DocumentNode
                    .SelectSingleNode("//input[contains(@name,'product_variant[package_weight]')]");
                var packageWeight = weight != null ? weight.Attributes["value"].Value : "0";


                var sellerStock = thisRow.DocumentNode
                    .SelectSingleNode("//input[contains(@name,'product_variant[marketplace_seller_stock]')]").Attributes["value"].Value;

                var sellerOldStock = thisRow.DocumentNode
                    .SelectSingleNode("//input[contains(@name,'product_variant[marketplace_seller_old_stock]')]").Attributes["value"].Value;

                var leadTime = thisRow.DocumentNode
                    .SelectSingleNode("//select[@name='product_variant[lead_time]']/option[@selected='selected']").Attributes["value"].Value;

                var limit = thisRow.DocumentNode.SelectSingleNode("//input[@name='product_variant[order_limit]']").Attributes["value"].Value;


                var info = new SellerStockInfoModel
                {
                    Height = Convert.ToInt32(packageHeight),
                    Length = Convert.ToInt32(packageLength),
                    Weight = Convert.ToInt32(packageWeight),
                    Width = Convert.ToInt32(packageWidth),
                    marketplace_seller_stock = Convert.ToInt32(sellerStock),
                    oldSellerStock = Convert.ToInt32(sellerOldStock),
                    maximum_per_order = Convert.ToInt32(limit),
                    lead_time = Convert.ToInt32(leadTime)
                };
                return info;
            }
            
            return null;
        }

        #endregion

        #region GetSellerProducts

        public List<ProductItemDetail> GetSellerProducts(int pageSize=20,bool? buyBoxWinner=null)
        {
            _restClient = new RestClient("https://seller.digikala.com/ajax/variants/search/");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
            foreach (RestResponseCookie cookie4 in _cookies)
            {
                _restRequest.AddCookie(cookie4.Name, cookie4.Value);
            }

            _restRequest.AddParameter("items", pageSize);
            if(buyBoxWinner.HasValue)
                _restRequest.AddParameter("search[is_buy_box_winner]", buyBoxWinner.Value?1: 0);

            _restRequest.AddParameter("search[type]", "all");
            _restRequest.AddParameter("search[active]", 1);
            _restRequest.AddParameter("search[moderation_status]", "approved");

            _response = _restClient.Execute(_restRequest);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_response.Content);
            if (string.IsNullOrEmpty(_response.Content))
                return null;
            

            var data = JsonConvert.DeserializeObject<ProductSearchModel>(_response.Content);
            return data.data.Items;         


        }

        #endregion

        #region DecodeEncodedNonAsciiCharacters

        private string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }

        #endregion
    }


}
