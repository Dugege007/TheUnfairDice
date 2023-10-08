using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Global : Architecture<Global>
{
    public static BindableProperty<float> HP = new(3);
    public static BindableProperty<float> MaxHP = new(3);

    protected override void Init()
    {
        
    }
}
