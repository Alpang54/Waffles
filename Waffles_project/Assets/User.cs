using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class User
{

    private string email;
    private string password;

    public User(string email, string password)
    {
       this.email = email;
       this.password = password;
        
    }

    public string getEmail()
    {

        return email;
    }

    public string getPassword()
    {
        return password;
    }


}
