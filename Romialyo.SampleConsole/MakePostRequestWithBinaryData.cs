using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Romialyo.Web;

namespace Romialyo.SampleConsole
{
    class MakePostRequestWithBinaryData
    {

        public static void Run()
        {
            Console.WriteLine("Press Ctrl+C to stop");
            while (true)
            {
                Console.WriteLine("Press ENTER to send email");
                Console.ReadLine();
                HttpWebRequest request = HttpWebRequest.Create("http://localhost:2143/Mailing/SendMail") as HttpWebRequest;
                string result = request.Post(
                    WebRequestDTOBuilder.New()
                    .WithParameter("fromAddress", "clients@Romialyo.com")
                    .WithParameter("fromName", "Romialyo Airlines (Pruebas desde consola externa)")
                    .WithParameter("toAddresses", "rodolfo.grave@blueit.es")
                    .WithParameter("subject", "Pruebas de servicio de envío de correos (Desde consola externa)")
                    .WithParameter("content", "Prueba de envío de correo electrónico mediante REST API.")
                    .WithFile("attachments", "RodolfoTest.txt", new MemoryStream(Encoding.UTF8.GetBytes("Hi! Just testing.")))
                    .WithFile("attachments", "2009-06.pdf", File.OpenRead("2009-06.pdf")));

                Console.WriteLine("Result='" + result + "'");
            }
        }
    }
}
