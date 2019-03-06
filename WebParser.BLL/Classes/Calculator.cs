﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser.App
{
    interface ICalculator
    {
        int Sum(int a, int b);
    }


    public class Calculator : ICalculator
    {
        public int Sum(int a, int b)
        {

            return a + b;
        }
    }
}
