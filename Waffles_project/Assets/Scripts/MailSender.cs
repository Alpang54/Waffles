using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.IO;

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
    string data = "";
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchStats());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator FetchStats()
    {
        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        data += "Custom Stages Statistics\n";
        FirebaseDatabase.DefaultInstance.GetReference("Data/" + "Custom/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                int countDB = 0;
                int noOfCustom;
                DataSnapshot snapshot = task.Result;
                foreach (var stageNumber in snapshot.Children)
                {
                    Debug.LogFormat("Key={0}", stageNumber.Key); //Node at question number
                    data += "Stage Name: " + stageNumber.Key + "\n";

                    noOfCustom = (int)stageNumber.ChildrenCount;  //noOfQuestions
                    data += "Number of players: " + noOfCustom + "\n";

                    foreach (var firebaseToken in stageNumber.Children)
                    {
                        countDB = 0;
                        Debug.LogFormat("Firebase token={0}", firebaseToken.Key); //values inside question 1,2
                        foreach (var stats in firebaseToken.Children)
                        {
                            if (stats.Key == "fbUserName")
                                data += "Facebook Username: " + stats.Value.ToString() + "\n";
                            //Debug.LogFormat("FB={0}", stats.Value.ToString()); //values inside question 1,2
                            if (stats.Key == "noRight")
                                data += "Number of correct qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "qnsRight")
                                data += "Correct qns: " + stats.Value.ToString() + "\n";
                            //Debug.LogFormat("Right={0}", stats.Value.ToString()); //values inside question 1,2
                            if (stats.Key == "noWrong")
                                data += "Number of wrong qns: " + stats.Value.ToString() + "\n";
                            //Debug.LogFormat("Wrong={0}", stats.Value.ToString()); //values inside question 1,2
                            if (stats.Key == "qnsWrong")
                                data += "Wrong qns: " + stats.Value.ToString() + "\n";
                            countDB++;
                        }

                    }
                    data += "================================\n";

                }
                done = true;
            }
        });

        yield return new WaitUntil(() => done == true);
        if (done)
        {
            Debug.Log(data);

            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/customStats.txt");
            sw.WriteLine(data);
            sw.Close();
        }

    }
    public void SendMail()
    {
        string toEmail = ifs.text;
        System.Text.RegularExpressions.Regex mailValidator = new System.Text.RegularExpressions.Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        validateEmail = mailValidator.IsMatch(ifs.text);

        if (validateEmail && done)
        {
            try
            {
                string attachmentPath = Application.persistentDataPath + "/customStats.txt";
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
                var mail = new MailMessage();
                mail.From = new MailAddress("cz3003wa@gmail.com");
                mail.To.Add(toEmail);
                mail.Attachments.Add(attachment);
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


