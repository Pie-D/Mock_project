//using Common.Models;
//using Microsoft.AspNetCore.Mvc;
//using RestSharp;

//namespace WebUI.Controllers;

//public class ProductController(IRestClient client) : Controller {
//    // GET
//    public async Task<IActionResult> Index() {
//        var request = new RestRequest("api/product");
//        request.AddJsonBody(new CreateProductModel());
//        await client.PostAsync<bool>(request);
        
//        var model = await client.GetAsync<GetProductResultModel>("api/product");
//        return View(model);
//    }
//}
