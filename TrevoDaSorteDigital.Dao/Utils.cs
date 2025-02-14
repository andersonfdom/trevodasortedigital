using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrevoDaSorteDigital.Dao.Models;

namespace TrevoDaSorteDigital.Dao
{
    public abstract class Utils
    {
        protected byte[] Key { get; set; }
        protected byte[] IniVetor { get; set; }
        protected Aes Algorithm { get; set; }

        protected string Criptografar(string entryText)
        {
            Key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            IniVetor = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            Algorithm = Aes.Create();

            byte[] symEncryptedData;

            var dataToProtectAsArray = Encoding.UTF8.GetBytes(entryText);
            using (var encryptor = Algorithm.CreateEncryptor(Key, IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(dataToProtectAsArray, 0, dataToProtectAsArray.Length);
                cryptoStream.FlushFinalBlock();
                symEncryptedData = memoryStream.ToArray();
            }
            Algorithm.Dispose();
            return Convert.ToBase64String(symEncryptedData);
        }

        protected string Descriptografar(string entryText)
        {
            Key = new byte[] { 12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23 };
            IniVetor = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            Algorithm = Aes.Create();

            var symEncryptedData = Convert.FromBase64String(entryText);
            byte[] symUnencryptedData;
            using (var decryptor = Algorithm.CreateDecryptor(Key, IniVetor))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(symEncryptedData, 0, symEncryptedData.Length);
                cryptoStream.FlushFinalBlock();
                symUnencryptedData = memoryStream.ToArray();
            }
            Algorithm.Dispose();
            return System.Text.Encoding.Default.GetString(symUnencryptedData);
        }

        protected bool EmailEnviado(string destinatario, string emailDestino, string subject, string body,bool enviarCopia)
        {
            try
            {
                // Configuração do remetente
                string emailFrom = "contato@trevodasortedigital.com.br";
                string emailPassword = "TrevoDaSorteDigital@2024";

                // Configuração do servidor SMTP
                string smtpHost = "winbr.servidordado.cloud";
                int smtpPort = 465;

                // Criação da mensagem
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Trevo da Sorte Digital", emailFrom));
                message.To.Add(new MailboxAddress(destinatario, emailDestino));

                if (enviarCopia == true)
                {
                    message.Cc.Add(new MailboxAddress("Administrativo Sorte Digital", "anderson.ferdomingos@gmail.com"));
                    message.Cc.Add(new MailboxAddress("Administrativo Sorte Digital", "lipe.bellotj@gmail.com"));
                }

                message.Subject = subject;

                // Corpo do e-mail em HTML
                string htmlBody = body;

                // Define o corpo como HTML
                message.Body = new TextPart("html")
                {
                    Text = htmlBody
                };

                // Envio do e-mail
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Ignorar erros de validação de certificado (Use com cuidado!)
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                    // Conecta ao servidor com segurança
                    client.Connect(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);

                    // Autentica com o servidor
                    client.Authenticate(emailFrom, emailPassword);

                    // Envia o e-mail
                    client.Send(message);

                    // Desconecta
                    client.Disconnect(true);
                    Console.WriteLine("E-mail enviado com sucesso!");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
                return false;
            }
        }

        protected string RemoverCaracteresEspeciais(string cpf)
        {
            // Remove todos os caracteres que não são números
            return Regex.Replace(cpf, "[^0-9]", "");
        }

        protected string GerarToken(int qtde)
        {
            return Guid.NewGuid().ToString("N").Substring(0, qtde);
        }

        protected bool LoginValidoAdmin(ApiTrevoDasorteDigitalContext ctx, string tokenAcesso)
        {
            return ctx.Usuarios.FirstOrDefault(c => c.Tokenacesso == tokenAcesso) != null ? true : false;
        }

        protected bool LoginValidoAfiliado(ApiTrevoDasorteDigitalContext ctx, string tokenAcesso)
        {
            return ctx.Afiliados.FirstOrDefault(c => c.Tokenacesso == tokenAcesso) != null ? true : false;
        }

        protected bool LoginValidoCliente(ApiTrevoDasorteDigitalContext ctx, string tokenAcesso)
        {
            return ctx.Clientes.FirstOrDefault(c => c.Tokenacesso == tokenAcesso) != null ? true : false;
        }

        protected string FormatarDataHora(string input)
        {
            // Especifica explicitamente o formato para garantir que a data seja interpretada corretamente.
            DateTime dateTime = DateTime.ParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            // Converte para o formato desejado "12/02/2025 15:40" usando o InvariantCulture
            string output = dateTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return output;
        }
    }
}
