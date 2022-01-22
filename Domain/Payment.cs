using Data.Models.Chalets;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
//
namespace Domain
{
    public class Payment
    {
        public static bool IsPaid(HttpContext httpContext, Guid ReservationId)
        {
            var paymentRepository = (IRepository<PaymentTransaction>)httpContext.RequestServices.GetService(typeof(IRepository<PaymentTransaction>));
            var reservationRepository = (IRepository<Reservation>)httpContext.RequestServices.GetService(typeof(IRepository<Reservation>));
            var reservation = reservationRepository.Table.Include(c => c.Unit).FirstOrDefault(c => c.Id == ReservationId);
            var trans = paymentRepository.Table.Where(c => c.ReservationId == ReservationId).Sum(c => c.Amount);
            if (trans>=reservation.TotalPrice)
            {
                return true;
            }
            return false;
        }
        public static string PayByCard(HttpContext httpContext,double amount,string ccv,string exDate,string cardNo, Guid reservationId,Guid UserId)
        {
            var paymentRepository = (IRepository<PaymentTransaction>)httpContext.RequestServices.GetService(typeof(IRepository<PaymentTransaction>));
            PaymentTransaction paymentTransaction = new PaymentTransaction
            {
                UserId=UserId,
                ReservationId=reservationId,
                Amount=amount,
                PaymentDateTime=DateTime.Now,
                PaymentType=(int)Enums.PaymentMethod.CreditCard,
                
            };
            paymentRepository.Add(paymentTransaction);
            return null;
        }
        public static string PayCash(HttpContext httpContext, double amount, Guid reservationId, Guid UserId)
        {
            var paymentRepository = (IRepository<PaymentTransaction>)httpContext.RequestServices.GetService(typeof(IRepository<PaymentTransaction>));
            PaymentTransaction paymentTransaction = new PaymentTransaction
            {
                UserId = UserId,
                ReservationId = reservationId,
                Amount = amount,
                PaymentDateTime = DateTime.Now,
                PaymentType = (int)Enums.PaymentMethod.Cash,
                RefNo= Guid.NewGuid().ToString("N").Substring(0, 5)
        };
            paymentRepository.Add(paymentTransaction);
            SaveInvoice(httpContext, reservationId, paymentTransaction.Id);
            return null;
        }

        public static void SaveInvoice(HttpContext httpContext, Guid ReservationId,Guid PaymentId)
        {
            var reservationRepository = (IRepository<Reservation>)httpContext.RequestServices.GetService(typeof(IRepository<Reservation>));
            var reservation = reservationRepository.Table.Include(c=>c.Invoices).ThenInclude(c=>c.User).Include(c => c.Unit).ThenInclude(c=>c.Chalet).Include(c=>c.User).FirstOrDefault(c => c.Id == ReservationId);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoiceTmp.html");
            string html = "";
            var invoice = reservation.Invoices.FirstOrDefault(c => c.Id == PaymentId);
            var TotalPrice = reservation.TotalDays * reservation.DayPrice;
            var UnPaidAmount = TotalPrice - reservation.Invoices.Sum(c => c.Amount);
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    html += reader.ReadLine();
                }
            }
            var LogoUrl = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "website", "images", "logo.png");

            html = html.Replace("#LogoUrl#", LogoUrl);

            html = html.Replace("#INVOICEName#", Resource.Invoice);
            html = html.Replace("#CLIENTLabel#", Resource.Name);
            html = html.Replace("#ADDRESSLabel#", Resource.Address);
            html = html.Replace("#EMAILLabel#", Resource.Email);
            html = html.Replace("#MobileLabel#", Resource.PhoneNumber);
            html = html.Replace("#WhatsappLabel#", Resource.WhatsAppNumber);
            html = html.Replace("#CLIENT#", reservation.User.FirstName+" "+ reservation.User.LastName);
            html = html.Replace("#ADDRESS#", reservation.User.Region);
            html = html.Replace("#EMAIL#", reservation.User.Email);
            html = html.Replace("#Mobile#", reservation.User.PhoneNumber);
            html = html.Replace("#WhatsappNumber#", reservation.User.WhatsAppNumber);
            html = html.Replace("#CompanyName#", Resource.CompanyName);
            html = html.Replace("#CompanyPhoneNumber#", Setting.Get(httpContext, "CompanyPhoneNumber"));
            html = html.Replace("#CompanyEmail#", Setting.Get(httpContext, "CompanyEmail"));
            html = html.Replace("#ProprtyNameLabel#", Resource.PropertyName);
            html = html.Replace("#ProprtyNumberLabel#", Resource.PropertyNumber);
            html = html.Replace("#ArrivedDateLabel#", Resource.ArriveDate);
            html = html.Replace("#LeaveDateLabel#", Resource.LeaveDate);
            html = html.Replace("#TotalDaysLabel#", Resource.TotalDays);
            html = html.Replace("#DayPriceLabel#", Resource.DayPrice);
            html = html.Replace("#PaidLabel#", Resource.PaidAmount);
            html = html.Replace("#UnPaidLabel#", Resource.UnPaidAmount);
            html = html.Replace("#TOTALLabel#", Resource.Total);
            html = html.Replace("#TotalPaidLabel#", Resource.TotalPaid);
            html = html.Replace("#PaidAmount#", reservation.Invoices.FirstOrDefault(c=>c.Id==PaymentId).Amount+"");
            html = html.Replace("#TotalPaidAmount#", reservation.Invoices.Sum(c => c.Amount)+"");
            html = html.Replace("#UnPaidAmount#", UnPaidAmount+"");
            var notice = Setting.Get(httpContext, "NOTICE");
            if (notice!= "N/A")
            {
                html = html.Replace("#NOTICELabel#", Resource.Notice);
                html = html.Replace("#NOTICE#", notice);
            }
            else
            {
                html = html.Replace("#NOTICELabel#", "");
                html = html.Replace("#NOTICE#", "");
            }
            var paidBy = "";
            if (invoice.User.UserType==(int)Enums.UserType.Admin)
            {
                paidBy = " تم الدفع بواسطة توريستا ";
                paidBy += invoice.User.FirstName + " " + invoice.User.LastName;
            }
            else
            {
                paidBy = " تم الدفع بواسطة مالك العقار ";
                paidBy += invoice.User.FirstName + " " + invoice.User.LastName;
            }
            html = html.Replace("#PaidTo#", paidBy);
            html = html.Replace("#ProprtyName#", reservation.Unit.Name);
            html = html.Replace("#ProprtyNumber#", reservation.Unit.Number+"");
            html = html.Replace("#ArrivedDate#", reservation.DateFrom.ToString("dd-MM-yyyy") +" "+ reservation.Unit.Chalet.EnterTime.ToString("hh:mm tt"));
            html = html.Replace("#LeaveDate#", reservation.DateFrom.ToString("dd-MM-yyyy") + " " + reservation.Unit.Chalet.ExitTime.ToString("hh:mm tt"));
            html = html.Replace("#TotalDays#", reservation.TotalDays+"");
            html = html.Replace("#DayPrice#", reservation.DayPrice+"");
            html = html.Replace("#TOTALAmount#", TotalPrice + "");
            html = html.Replace("#QRCODE#", getQRCode(reservation.Invoices.FirstOrDefault(c=>c.Id==PaymentId).RefNo));

            var doc = HtmlToPDF.HtmlToPdf(html);
            var subPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "invoices", ReservationId + "");
            if (!System.IO.Directory.Exists(subPath))
            {
                System.IO.Directory.CreateDirectory(subPath);
            }
            var pdfpath = Path.Combine(subPath, PaymentId + ".pdf");
            System.IO.File.WriteAllBytes(pdfpath, doc);
            //SendWhatsApp(httpContext,reservation.User.WhatsAppNumber, pdfpath);
            EmailService.SendMail(reservation.User.Email,new List<string>(),"This Test Email","Reservation Invoice", pdfpath);
        }


        private static string getQRCode(string QRCodeText)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            ImageConverter converter = new ImageConverter();
            var BitmapArray = (byte[])converter.ConvertTo(QrBitmap, typeof(byte[]));
            return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
        }

        private static void SendWhatsApp(HttpContext httpContext, string whatsAppNumber, string pdfpath)
        {
            throw new NotImplementedException();
        }
    }
}
