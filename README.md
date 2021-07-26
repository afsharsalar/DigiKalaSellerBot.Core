#DigiKalaSellerBot.Core
=======
<div dir="rtl">

<p>
  <a href="https://github.com/afsharsalar/DigiKalaSellerBot.Core">
     <img alt="GitHub Actions status" src="https://github.com/VahidN/DNTPersianUtils.Core/workflows/.NET%20Core%20Build/badge.svg">
  </a>
</p>

DigiKalaSellerBot.Core کتابخانه ای است متشکل از متدهایی برای سلرهای دیجی کالا که بتوانند با استفاده از سیاست های رقابتی خود با سایر فروشنده های رقیب رقابت نمایند.


</div>

[![Nuget](https://img.shields.io/nuget/v/DigiKalaSellerBot.Core)](https://github.com/afsharsalar/DigiKalaSellerBot.Core)
```
PM> Install-Package DigiKalaSellerBot.Core 
```
<div dir="rtl">
  <h2>نحوه کار 
</h2>
  
</div>

برای دریافت اطلاعات محصول
```csharp
var service = new DigiKala();
var dkp=272383;
var info=service.GetProductInfo(dkp);

```


برای کاهش قیمت تنوع مورد نظر
```csharp
var service = new DigiKala();
var dkp=272383;
var dkpc=1957080;
var price=3_800_000;
var login=new LoginModel{Email = "info@example.ir", Password = "1234" };
service.Login(login);
var result = service.ChangePrice(dkpc, dkp, price);
```



دریافت لیست محصولات سلر
```csharp
var service = new DigiKala();
var login=new LoginModel{Email = "info@example.ir", Password = "1234" };
service.Login(login);
var result = service.GetSellerProducts();
```



دریافت لیست محصولاتی که بای باکس نیست
```csharp
var service = new DigiKala();
var login=new LoginModel{Email = "info@example.ir", Password = "1234" };
service.Login(login);
var result = service.GetSellerProducts(buyBoxWinner:false);
```

