using GYM_MN.Models;
using GYM_MN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYM_MN.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly GymMnContext _context;

        public PaymentsController(GymMnContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments()
        {
            var payments = await _context.Payments
                .Select(p => new PaymentDto
                {
                    PaymentId = p.PaymentId,
                    MemberId = p.MemberId,
                    MembershipTypeId = p.MembershipTypeId,  
                    PaymentDate = p.PaymentDate,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status
                })
                .ToListAsync();

            return payments;
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDto>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            var paymentDto = new PaymentDto
            {
                PaymentId = payment.PaymentId,
                MemberId = payment.MemberId,
                MembershipTypeId = payment.MembershipTypeId,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status
            };

            return paymentDto;
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(PaymentDto paymentDto)
        {
            // Tạo mới đối tượng Payment từ PaymentDto
            var payment = new Payment
            {
                MemberId = paymentDto.MemberId,
                MembershipTypeId= paymentDto.MembershipTypeId,
                PaymentDate = paymentDto.PaymentDate,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
        }

        // PUT: api/Payments
        [HttpPut]
        public async Task<IActionResult> PutPayment(PaymentDto paymentDto)
        {
            var id = paymentDto.PaymentId;
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin của đối tượng Payment từ PaymentDto
            payment.MemberId = paymentDto.MemberId;
            payment.MembershipTypeId = paymentDto.MembershipTypeId;
            payment.PaymentDate = paymentDto.PaymentDate;
            payment.Amount = paymentDto.Amount;
            payment.PaymentMethod = paymentDto.PaymentMethod;
            payment.Status = paymentDto.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/Payments/VnPay
        [HttpPost]
        public async Task<IActionResult> PostPaymentViaVnPay(PaymentDto paymentDto)
        {
            // Xử lý tạo thanh toán qua VNPAY ở đây
            // Gọi API VNPAY để tạo thanh toán, sau đó xử lý kết quả trả về từ VNPAY

            // Sau khi thanh toán thành công, tạo payment mới và lưu vào cơ sở dữ liệu
            var payment = new Payment
            {
                MemberId = paymentDto.MemberId,
                MembershipTypeId = paymentDto.MembershipTypeId,
                PaymentDate = paymentDto.PaymentDate,
                Amount = paymentDto.Amount,
                PaymentMethod = paymentDto.PaymentMethod,
                Status = paymentDto.Status
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
