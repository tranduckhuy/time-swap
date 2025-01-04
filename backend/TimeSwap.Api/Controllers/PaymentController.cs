using MediatR;
using Microsoft.AspNetCore.Mvc;
using TimeSwap.Api.Mapping;
using TimeSwap.Api.Models;
using TimeSwap.Application.Configurations.Payments.Responses;
using TimeSwap.Application.Mappings;
using TimeSwap.Application.Payments.Commands;
using TimeSwap.Shared;
using TimeSwap.Shared.Constants;

namespace TimeSwap.Api.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : BaseController<PaymentController>
    {
        public PaymentController(
            IMediator mediator,
            ILogger<PaymentController> logger
        ) : base(mediator, logger) { }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (request == null || string.IsNullOrEmpty(ipAddress))
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = (int)Shared.Constants.StatusCode.ModelInvalid,
                    Message = ResponseMessages.GetMessage(Shared.Constants.StatusCode.ModelInvalid),
                    Errors = ["The request body does not contain required fields or the ipAddress is not found in the claims"]
                });
            }

            var command = AppMapper<ModelMapping>.Mapper.Map<CreatePaymentCommand>(request);
            command.IpAddress = ipAddress;
            return await ExecuteAsync<CreatePaymentCommand, string>(command);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromQuery] VnPayOneTimePaymentCreateLinkResponse response)
        {
            var command = new VnpayReturnCommand
            {
                vnp_TmnCode = response.vnp_TmnCode,
                vnp_Amount = response.vnp_Amount,
                vnp_BankCode = response.vnp_BankCode,
                vnp_BankTranNo = response.vnp_BankTranNo,
                vnp_CardType = response.vnp_CardType,
                vnp_PayDate = response.vnp_PayDate,
                vnp_OrderInfo = response.vnp_OrderInfo,
                vnp_TransactionNo = response.vnp_TransactionNo,
                vnp_ResponseCode = response.vnp_ResponseCode,
                vnp_TransactionStatus = response.vnp_TransactionStatus,
                vnp_TxnRef = response.vnp_TxnRef,
                vnp_SecureHash = response.vnp_SecureHash
            };

            return await ExecuteAsync<VnpayReturnCommand, string>(command);
        }

    }
}
