using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository discountRepository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        this.discountRepository = discountRepository;
    }


    [HttpGet("{productName}",Name = "GetDiscount")]
    [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>>  GetDiscount(string productName)
    {
        var coupon = await discountRepository.GetDisCount(productName);
        return Ok(coupon);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
    {
       await discountRepository.CreateDiscount(coupon);
       return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
       
    }

    [HttpPut]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateDiscount([FromBody] Coupon coupon)
    {
       return Ok( await discountRepository.UpdateDiscount(coupon)); 
    }

    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteDiscount(string productName)
    {
        return Ok(await discountRepository.DeleteDiscount(productName));
    }
}

