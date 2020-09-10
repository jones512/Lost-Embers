using UnityEngine;

namespace AdventureKit.Utils
{
    public class Utils
    {
        public static string RemoveCloneSubstring(string nameWithClone)
        {
            string name = nameWithClone;
            if (name.Contains("(Clone)"))
            {
                // Quitamos la coletilla '(Clone)' del nombre de la instancia
                int cloneSubstringStartIdx = name.IndexOf('(');
                name = name.Substring(0, cloneSubstringStartIdx);
            }
            return name;
        }

        public static Vector3 GetPositiveAngle(float y)
        {
           if(y > 0)
            {

                return new Vector3(0, y, 0);
            }
            else
            {
                if (Mathf.Round(y) == -90)
                    return new Vector3(0, 270, 0);

                if (Mathf.Round(y) == -180)
                    return new Vector3(0, 360, 0);
            }

            return Vector3.zero;
        }
    }    
}


