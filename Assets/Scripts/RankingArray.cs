using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[Serializable]
public class RankingArray
{
    public Ranking[] content;

    override
    public string ToString()
    {
        if (content != null)
        {
            StringBuilder sb = new StringBuilder("[");
            foreach(Ranking entry in content)
            {
                sb.Append(entry.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }
        else
        {
            return "empty";
        }
    }
}
