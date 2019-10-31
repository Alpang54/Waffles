using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
namespace Tests
{
    public class WeiMinTest : MonoBehaviour
    {
        [Test]
        public void TestSettingQuestion()
        {
            List<QuestionChoice> qnList = new List<QuestionChoice>();
            QuestionChoice q = new QuestionChoice();
            q.setQns("What is this test");
            q.addOptionChoice("Answer");
            q.addOptionChoice("Not Answer");
            q.addOptionChoice("Not Answer");
            q.addOptionChoice("Not Answer");
            q.setAnswer(1);
            qnList.Add(q);
            CustomGame cg = new CustomGame();
            cg.qnList = new List<QuestionChoice>();
            cg.qnList = qnList;
            Assert.AreEqual("What is this test", cg.qnList[0].getQns());
            Assert.AreEqual("Answer", cg.qnList[0].getChoices()[0].ToString());

        }
        [Test]
        public void TestQuestionAnswer()
        {
            List<QuestionChoice> qnList = new List<QuestionChoice>();
            QuestionChoice q = new QuestionChoice();
            q.setQns("What is this test");
            q.addOptionChoice("Answer");
            q.addOptionChoice("Not Answer");
            q.addOptionChoice("Not Answer");
            q.addOptionChoice("Not Answer");
            q.setAnswer(1);
            qnList.Add(q);
            CustomGame cg = new CustomGame();
            cg.qnList = new List<QuestionChoice>();
            cg.qnList = qnList;
            Assert.AreEqual(true, cg.CheckAns(1));
            Assert.AreEqual(false, cg.CheckAns(2));
            Assert.AreEqual(false, cg.CheckAns(3));
            Assert.AreEqual(false, cg.CheckAns(4));
        }
        [Test]
        public void TestEmailFormatValidation()
        {
            MailSender ms = new MailSender();
            Assert.AreEqual(true, ms.ValidateEmail("xxx@gmail.com"));
            Assert.AreEqual(false, ms.ValidateEmail("xxx@"));
            Assert.AreEqual(false, ms.ValidateEmail("xxx"));
            Assert.AreEqual(false, ms.ValidateEmail("xxx.com"));

        }
        [Test]
        public void TestConvertTimeStampToGMT8()
        {
            MailSender ms = new MailSender();
            DateTime dt = ms.ConvertUnixTimeStamp(1571920529448);
            Assert.AreEqual("10/24/2019 8:35:29 PM", dt.ToString());

        }
        [Test]
        public void TestSearchStageName()
        {
            List<string> al = new List<string>();
            al.Add("Stage Name 1");
            al.Add("Stage Name 2");
            al.Add("Stage Name 3");
            al.Add("Hello");
            al.Add("Bye");
            InitContent ic = new InitContent();

            int count = 0;
            foreach(string temp in al)
            {
                if (ic.Search(temp, "Stage"))
                {
                    count++;
                }
            }

            Assert.AreEqual(3, count);
        }
    }
}

