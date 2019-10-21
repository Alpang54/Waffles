using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionChoice : MonoBehaviour
{
    public string qns;
    public int qnsNumber;
    public ArrayList optionChoice = new ArrayList();
    public int answerInt;
    public string answerStr;
    public bool answeredCorrectly = false;
    public QuestionChoice()
    {

    }
    public void setAnsweredCorrectWrong(bool stats)
    {
        answeredCorrectly = stats;
    }
    public bool getAnsweredCorrecrWrong()
    {
        return answeredCorrectly;
    }
    public int getQnsNumber()
    {
        return qnsNumber;
    }
    public void setQnsNumber(int qn)
    {
        qnsNumber = qn;
    }
    public string getQns()
    {
        return qns;
    }
    public ArrayList getChoices()
    {
        return optionChoice;
    }
    public void setQns(string qns)
    {
        this.qns = qns;
    }
    public void addOptionChoice(string opt)
    {
        optionChoice.Add(opt);
    }
    public void setOptionChoice(ArrayList al)
    {
        optionChoice = al;
    }
    public void setAnswer(int i)
    {
        answerInt = i;
    }
    public void setAnswer(string i)
    {
        answerStr = i;
        //answerInt = int.Parse(answerStr);
    }
    public int getAnswer()
    {
        return answerInt;
    }
}
