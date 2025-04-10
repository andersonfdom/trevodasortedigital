package com.example.apitrevodasortedigital;

import javax.crypto.Cipher;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import java.nio.charset.StandardCharsets;
import java.security.Key;
import java.util.Properties;
import java.util.Base64;

public class Utils {

    private final String headerEmail = "https://apostas.trevodasortedigital.com.br/assets/img/logo.jpg";
    private byte[] key = {12, 2, 56, 117, 12, 67, 33, 23, 12, 2, 56, 117, 12, 67, 33, 23};
    private byte[] initVector = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16};

    public String criptografar(String entryText) throws Exception {
        Key secretKey = new SecretKeySpec(key, "AES");
        IvParameterSpec ivParameterSpec = new IvParameterSpec(initVector);

        Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
        cipher.init(Cipher.ENCRYPT_MODE, secretKey, ivParameterSpec);

        byte[] encrypted = cipher.doFinal(entryText.getBytes(StandardCharsets.UTF_8));
        return Base64.getEncoder().encodeToString(encrypted);
    }

    public String descriptografar(String entryText) throws Exception {
        Key secretKey = new SecretKeySpec(key, "AES");
        IvParameterSpec ivParameterSpec = new IvParameterSpec(initVector);

        Cipher cipher = Cipher.getInstance("AES/CBC/PKCS5Padding");
        cipher.init(Cipher.DECRYPT_MODE, secretKey, ivParameterSpec);

        byte[] decoded = Base64.getDecoder().decode(entryText);
        byte[] decrypted = cipher.doFinal(decoded);
        return new String(decrypted, StandardCharsets.UTF_8);
    }

    public String getItemLabelValueEmail(String label, String value) {
        return "<b style='font-size:18px'>" + label + ": </b><span style='font-size:15px;font-weight:normal'>" + value + "</span><br>";
    }

    public String getBodyEmail(String subject, String itensBody) {
        return "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                "<img src='" + headerEmail + "'>" +
                "<h2>" + subject + "<h2><br>" +
                "<div style='text-align:left;word-wrap: break-word'>" + itensBody + "</div><br /></div>";
    }

    public boolean emailEnviado(String emailDestino, String subject, String body) {
        try {
            String host = "mail.trevodasortedigital.com.br";
            String from = "contato@trevodasortedigital.com.br";
            String password = "TrevoDaSorteDigital@2025";

            Properties properties = System.getProperties();
            properties.setProperty("mail.smtp.host", host);
            properties.setProperty("mail.smtp.port", "587");
            properties.setProperty("mail.smtp.ssl.enable", "true");
            properties.setProperty("mail.smtp.auth", "true");

            Session session = Session.getInstance(properties, new Authenticator() {
                protected PasswordAuthentication getPasswordAuthentication() {
                    return new PasswordAuthentication(from, password);
                }
            });

            MimeMessage message = new MimeMessage(session);
            message.setFrom(new InternetAddress(from));
            message.addRecipient(Message.RecipientType.TO, new InternetAddress(emailDestino));
            message.setReplyTo(InternetAddress.parse(from));
            message.setSubject(subject);
            message.setContent(body, "text/html; charset=utf-8");

            Transport.send(message);
            return true;
        } catch (Exception ex) {
            ex.printStackTrace();
            return false;
        }
    }

    /*
    public String montarCorpoEmailRecSenha(String serial) {
        String corpoEmail = "";

        corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                "<img src='" + headerEmail + "' width=300 heigth=300>" + "<br /></div><br />";
        corpoEmail += "<h2>Olá</h2>";
        corpoEmail += "<p>Recebemos sua solicitação de recuperação de senha.</p>";
        corpoEmail += "<p>Para criar uma nova,<a href='https://apostas.trevodasortedigital.com.br/Home/RecuperacaoSenha?code=" + serial + "'>Clique aqui</a></p>";
        corpoEmail += "<p><b>Importante:</b>o link é válido por 24 horas.</p>";
        corpoEmail += "<p>Após este período, você deverá solicitar um novo, tá bem?!</p>";
        corpoEmail += "<p>Caso não tenha sido você, por gentileza, desconsidere este e-mail.</p>";
        corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
        corpoEmail += "<p>Equipe Sorte Digital</p>";

        return corpoEmail;
    }

    public String montarCorpoEmailGerarSenhaAcesso(Cliente cliente) throws Exception {
        String corpoEmail = "";

        corpoEmail += "<div style='text-align:center'><div style='max-width: 600px; margin: 0 auto;'>" +
                "<img src='" + headerEmail + "' width=300 heigth=300>" + "<br /></div><br />";
        corpoEmail += "<h2>Prezado(a) Cliente " + cliente.getNome() + "</h2>";
        corpoEmail += "<p>Obrigado por ter adquirido conosco o Sistema Gerador de Apostas Sorte Digital.</p>";
        corpoEmail += "<br />";
        corpoEmail += "<p>Segue abaixo os dados de acesso:</p>";
        corpoEmail += "<p>Login: " + cliente.getEmail() + "</p>";
        corpoEmail += "<p>Senha: " + descriptografar(cliente.getSenha()) + "</p>";
        corpoEmail += "<p>Para acessar o Sistema,<a href='https://apostas.trevodasortedigital.com.br'>Clique aqui</a></p>";
        corpoEmail += "<br />";
        corpoEmail += "<p>Se precisar de ajuda, conte com a gente!*</p><br />";
        corpoEmail += "<p>Equipe Sorte Digital</p>";

        return corpoEmail;
    }
    */
}
