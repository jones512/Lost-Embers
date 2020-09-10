using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onki.util
{

	public class Expression
    {
		protected string originalExpression, rpnExpression;
		
		// Diccionario de variables
		public Dictionary<string, float> Variables = new Dictionary<string, float>();
		
		// Tokens compilados
		Stack<string> compiled;
	
		// Construye una expresión
		public Expression(string exp)
        {
			rpnExpression = LibRpn.InfixToRpn(exp, LibRpn.RpnOptions.None);
			
			//Debug.Log ("Compilada: "+rpnExpression);
		}
		
		public float Evaluate()
        {
			float result;
			compiled = new Stack<string>();
			
			string[] fields = rpnExpression.Split(' ');
			
			if (fields.Length==1)
				float.TryParse(fields[0], out result);
			
			foreach(string f in fields)
				compiled.Push(f);
				
			result = GetResult();
				
			return result;			
		}
		
		public float GetResult()
        {
		
			string operation;
			float op1, op2;
			float result;
			
			// Lee el operando
			operation = compiled.Pop();
			
			// Si es 0, es un valor concreto
			if (operation=="0") 
				return 0f; 
			
			// Si se puede parsear es que es un valor, se parsea y se devuelve
			float.TryParse(operation, out result);
            if (result!=0)
				return result;
			
			// sino es posible que sea una variable
			foreach(KeyValuePair<string,float> var in Variables)
            {
				if (var.Key == operation)
					return var.Value;
			}
			
			//Debug.Log ("Operacion "+operation);
			
			// Lee los operandos
			op2 = GetResult ();
			op1 = GetResult ();
					
			switch ( operation )
            {
				case "+":
					//Debug.Log (op1+" + "+op2);
					return op1+op2;
				//break;	
				case "-":
					//Debug.Log (op1+" - "+op2);
					return op1-op2;
				//break;
				case "*":
					//Debug.Log (op1+" * "+op2);
					return op1*op2;
				//break;
				case "/":
					//Debug.Log (op1+" / "+op2);
					return op1/op2;
				//break;
				default:
					Debug.LogError("Operación no reconocida: "+operation);
				break;
			}
			
			return 0f;
		}
	}
}