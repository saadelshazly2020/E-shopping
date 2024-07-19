using Discount.Grpc.Protos;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        public override Task<CopounModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            //get discount from database
            return base.GetDiscount(request, context);
        }
        public override Task<CopounModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            return base.CreateDiscount(request, context);
        }

        public override Task<CopounModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
