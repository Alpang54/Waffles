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
/** To send email from waffle account to user's email input
 * @author Mok Wei Min
 */
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
    bool done = false;
    [SerializeField]
    GameObject confirmButton;
    [SerializeField]
    GameObject fetching;
    string toEmail;
    bool sent = false;
    bool done2 = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    /**
    *Calls the couroutine to fetch custom stage statistics, followed by main stage statistics
    *Operates on button press
    **/
    public void GetStats()
    {
        StartCoroutine(FetchStats());

    }
    /**
    *Converts time stamp that is from the firebase server into a valid date time for appending in the text documents
    * @param unixTimeStamp
    **/
    public DateTime ConvertUnixTimeStamp(long unixTimeStamp)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        epoch = epoch.AddMilliseconds(unixTimeStamp);// your case results to 4/5/2013 8:48:34 AM
        TimeZoneInfo tzi = TimeZoneInfo.Local;

        DateTime dt = System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(epoch, tzi.Id);
        return dt;
    }
    /**
    *Fetches from the firebase the main stage statistics
    *Appends data to a mainStats.txt
    **/
    IEnumerator FetchMainStats()
    {
        string data = "";

        done2 = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        data += "Main Stages Statistics\n";
        data += "===============================================\n";

        FirebaseDatabase.DefaultInstance.GetReference("Data/" + "Main/").GetValueAsync().ContinueWith(task =>
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
                    data += "World/Stage: " + stageNumber.Key + "\n";

                    noOfCustom = (int)stageNumber.ChildrenCount;  //noOfQuestions
                    data += "Number of players: " + noOfCustom + "\n";
                    Debug.LogFormat("Count={0}", stageNumber.ChildrenCount); //Node at question number

                    foreach (var firebaseToken in stageNumber.Children)
                    {
                        countDB = 0;
                        data += "----------------------------------------\n";

                        Debug.LogFormat("Firebase token={0}", firebaseToken.Key); //values inside question 1,2
                        foreach (var stats in firebaseToken.Children)
                        {
                            if (stats.Key == "fbUsername")
                                data += "Facebook Username: " + stats.Value.ToString() + "\n";
                            Debug.LogFormat("FB={0}", stats.Value.ToString()); //values inside question 1,2

                            if (stats.Key == "totalQns")
                                data += "Number of attempted qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "noRight")
                                data += "Number of correct qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "attemptedTimestamp")
                            {
                                long milliseconds;
                                long.TryParse(stats.Value.ToString(), out milliseconds);
                                Debug.Log(ConvertUnixTimeStamp(milliseconds));
                                data += "Timestamp attempted: " + ConvertUnixTimeStamp(milliseconds) + "\n";
                            }
                           
                            if (stats.Key == "noWrong")
                                data += "Number of wrong qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "timeTakenPer")
                                data += "Average time taken for each qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "totalTimeTaken")
                                data += "Total time taken for stage: " + stats.Value.ToString() + "\n";
                            countDB++;
                        }

                    }
                    data += "=======================================\n";

                }
                done2 = true;
            }
        });

        yield return new WaitUntil(() => done2 == true);
        if (done2)
        {
            Debug.Log(data);

            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/mainStats.txt");
            sw.WriteLine(data);
            sw.Close();

        }

    }
    /**
    *Fetches from the firebase the custom stage statistics
    *Appends data into a customStats.txt
    **/
    IEnumerator FetchStats()
    {
        string data = "";

        done = false;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cz3003-waffles.firebaseio.com/");
        data += "Custom Stages Statistics\n";
        data += "===============================================\n";

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
                    Debug.LogFormat("Count={0}", stageNumber.ChildrenCount); //Node at question number

                    foreach (var firebaseToken in stageNumber.Children)
                    {
                        countDB = 0;
                        data += "----------------------------------------\n";

                        Debug.LogFormat("Firebase token={0}", firebaseToken.Key); //values inside question 1,2
                        foreach (var stats in firebaseToken.Children)
                        {
                            if (stats.Key == "fbUsername")
                                data += "Facebook Username: " + stats.Value.ToString() + "\n";
                            Debug.LogFormat("FB={0}", stats.Value.ToString()); //values inside question 1,2

                            if (stats.Key=="qnsCount")
                                data += "Number of attempted qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "noRight")
                                data += "Number of correct qns: " + stats.Value.ToString() + "\n";
                            if(stats.Key=="attemptedTimestamp")
                                {
                                    long milliseconds;
                                    long.TryParse(stats.Value.ToString(), out milliseconds);
                                    Debug.Log(ConvertUnixTimeStamp(milliseconds));
                                    data += "Timestamp attempted: " + ConvertUnixTimeStamp(milliseconds) + "\n";
                                }
                            if (stats.Key == "qnsRight")
                                data += "Correct qns: " + stats.Value.ToString() + "\n";
                            //Debug.LogFormat("Right={0}", stats.Value.ToString()); //values inside question 1,2
                            if (stats.Key == "noWrong")
                                data += "Number of wrong qns: " + stats.Value.ToString() + "\n";
                            //Debug.LogFormat("Wrong={0}", stats.Value.ToString()); //values inside question 1,2
                            if (stats.Key == "qnsWrong")
                                data += "Wrong qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "timeTakenPer")
                                data += "Average time taken for each qns: " + stats.Value.ToString() + "\n";
                            if (stats.Key == "totalTimeTaken")
                                data += "Total time taken for stage: " + stats.Value.ToString() + "\n";
                            countDB++;
                        }

                    }
                    data += "=======================================\n";

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
            StartCoroutine(FetchMainStats());

        }

    }
    /**
   *Validating of email using regex
   *@param email User's input from input field
   * @return true if it is a valid email string
   **/
    public bool ValidateEmail(string email)
    {
        System.Text.RegularExpressions.Regex mailValidator = new System.Text.RegularExpressions.Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
        if (mailValidator.IsMatch(email))
            return true;
        else
            return false;
    }
    /**
   *Smtp connection, sends email from cz3003wa@gmail.com to user's input email if it is a valid email string
   *Attaches the 2 txt files (customTxt & mainTxt)
   **/
    public void SendMail()
    {
        toEmail = ifs.text;

        if (ValidateEmail(toEmail))
        {
            if(done2)
            try
            {
                string attachmentPath = Application.persistentDataPath + "/customStats.txt";
                  
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
                attachmentPath = Application.persistentDataPath + "/mainStats.txt";
                System.Net.Mail.Attachment attachment2 = new System.Net.Mail.Attachment(attachmentPath);
                var mail = new MailMessage();
                mail.From = new MailAddress("cz3003wa@gmail.com");
                mail.To.Add(toEmail);
                mail.Attachments.Add(attachment);
                mail.Attachments.Add(attachment2);
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
                sent = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            else
            {
                ifs.text = "";
                ifs.placeholder.GetComponent<Text>().text = "Fetching report, please try later";
            }
        }
        else
        {
            ifs.text = "";
            ifs.placeholder.GetComponent<Text>().text = "Please input valid email";
        }
    }
}


