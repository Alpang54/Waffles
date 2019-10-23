using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.UI;
using System;

public class MailSender : MonoBehaviour
{
    [SerializeField]
    InputField ifs;
    [SerializeField]
    GameObject reportPopUp;
    [SerializeField]
    GameObject sentPopUp;
    [SerializeField]
    Text popUpText;
    bool validateEmail = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendMail()
    {
        string toEmail = ifs.text;
        System.Text.RegularExpressions.Regex mailValidator = new System.Text.RegularExpressions.Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        validateEmail = mailValidator.IsMatch(ifs.text);

        if (validateEmail)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("cz3003wa@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Waffles Statistic Report";
                mail.Body = "This is the statistics for the Waffles app";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("cz3003wa@gmail.com", "@qwerty123");
                smtpClient.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback = (x, y, z, w) => true;
                smtpClient.SendMailAsync(mail);
                reportPopUp.SetActive(false);
                sentPopUp.SetActive(true);
                popUpText.text = "Report has been sent to " + toEmail;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        else
        {
            ifs.text = "";
            ifs.placeholder.GetComponent<Text>().text = "Please input valid email";
        }
    }
}


