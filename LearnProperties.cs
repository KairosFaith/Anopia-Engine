using UnityEngine;
public class LearnProperties : MonoBehaviour
{
    private float _hp;//actual variable, made private and inaccessible to outside
    void Start()//learn get set variable terminology
    {
        float getHP = _hp;//get a value from the variable
        print(getHP);
        float setHP = 2;
        _hp = setHP;//set the variable to another value
    }
    float GetHP()
    {
        return _hp;// get means value output
    }
    void SetHP(float value)
    {
        _hp = value;// set means value input
    }
    public float HP//public property accessible from other scripts
    {
        get
        {
            return _hp;//same as above
        }
        set
        {
            _hp = value;//same as above, the value keyword must be there
        }
    }//you can think of get set as methods to input and output the value
    void UseProperty()
    {
        float getHP = HP;
        print(getHP);
        float setHP = 2;
        HP = setHP;
    }//you use the property the same way you use a variable
}