using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
*Deserializes the questions and choices from firebase
* @author Mok Wei Min
**/
public class QuestionChoice : MonoBehaviour
{
    string qns;
    int qnsNumber;
    ArrayList optionChoice = new ArrayList();
    int answerInt;
    string answerStr;
    bool answeredCorrectly = false;
    bool cleared = false;
    int difficulty;

    /**
    *Empty constructor
    **/
    public QuestionChoice()
    {

    }
    /**
    *Sets cleared variable
    * @param c boolean for cleared
    **/
    public void setCleared(bool c)
    {
        cleared = c;
    }
    /**
   * @return cleared
   **/
    public bool getCleared()
    {
        return cleared;
    }
    /**
    *Sets difficulty variable
    * @param d for difficulty
    **/
    public void setDifficulty(int d)
    {
        difficulty = d;
    }
    /**
   * @return difficulty
   **/
    public int getDifficulty()
    {
        return difficulty;
    }
    /**
    *Sets if question is answered correct or wrong
    * @param stats for answeredCorrectly
    **/
    public void setAnsweredCorrectWrong(bool stats)
    {
        answeredCorrectly = stats;
    }
    /**
  * @return answeredCorrectly
  **/
    public bool getAnsweredCorrecrWrong()
    {
        return answeredCorrectly;
    }
    /**
  * @return qnsNumber the question number of this question
  **/
    public int getQnsNumber()
    {
        return qnsNumber;
    }
    /**
   *Sets question number of this question
   * @param qn for qnsNumber
   **/
    public void setQnsNumber(int qn)
    {
        qnsNumber = qn;
    }
    /**
 * @return qns the string value of the question
 **/
    public string getQns()
    {
        return qns;
    }
    /**
 * @return optionChoice the array of choices tied to the question
 **/
    public ArrayList getChoices()
    {
        return optionChoice;
    }
    /**
   *Sets question for this question
   * @param qns for question string
   **/
    public void setQns(string qns)
    {
        this.qns = qns;
    }
    /**
   *Adds choice into choice array
   * @param opt string of choice
   **/
    public void addOptionChoice(string opt)
    {
        optionChoice.Add(opt);
    }
    /**
  *Sets a whole option choice
  * @param al array of strings
  **/
    public void setOptionChoice(ArrayList al)
    {
        optionChoice = al;
    }
    /**
  *Sets the number for the answer
  * @param i the number for the choice that is correct
  **/
    public void setAnswer(int i)
    {
        answerInt = i;
    }
    /**
 *Sets the string for the answer
 * @param i the string that is correct
 **/
    public void setAnswer(string i)
    {
        answerStr = i;
        //answerInt = int.Parse(answerStr);
    }
    /**
* @return answer number that is correct
**/
    public int getAnswer()
    {
        return answerInt;
    }
}
