using BuildingBlocks.Exceptions;
using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CopounModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            Coupon? discount;
            try
            {
                discount = await dbContext.Coupons.OrderByDescending(x=>x.Id).FirstOrDefaultAsync(x => x.ProductName == request.ProductName)!;

                if (discount == null)
                    return new CopounModel { ProductName = "No Discount", Amount = 0, Description = "product doesn`t have discount!" };
            }
            catch (Exception ex)
            {

                throw new InternalServerException(ex.Message);
            }
            
            var model = discount.Adapt<CopounModel>();

            return model;

        }
        public override async Task<CopounModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request?.Coupon?.Adapt<Coupon>();

            var addedCoupon = await dbContext.Coupons.AddAsync(coupon!);
            try
            {
                await dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            return addedCoupon.Entity.Adapt<CopounModel>()!;
        }

        public async override Task<CopounModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {

            var coupon = request?.Coupon?.Adapt<Coupon>();
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Data!"));
            var addedCoupon = dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            return coupon.Adapt<CopounModel>();
        }

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = dbContext.Coupons.FirstOrDefault(x => x.ProductName == request.ProductName);
            if (coupon == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request Data!"));
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            return new DeleteDiscountResponse { IsSuccess = true };
        }
        public override async Task<GetAllDiscountsResponse> GetAllDiscounts(GetAllDiscountsRequest request, ServerCallContext context)
        {
            var coupons = await dbContext.Coupons.ToListAsync();

            var response = new GetAllDiscountsResponse
            {
                Coupons = { coupons.Adapt<List<CopounModel>>() }
            };

            return response;

        }

    }
}
