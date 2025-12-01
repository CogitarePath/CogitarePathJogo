using System.Threading;
using System.Timers;
using UnityEngine;

public class TimertoDificult : MonoBehaviour
{
    public int MultiplySpawn;
    public int Minutes;
    public float Seconds;
    public int Hours;
    private void OnTriggerStay(Collider other)
    {
        Seconds += Time.deltaTime;
        if( Seconds >= 60)
        {
            Minutes++;
            Seconds = 0;
        }

        if (Minutes >= 60)
        {
            Hours++;
            Minutes = 0;
        }

        if (Hours >= 1)
        {
            MultiplySpawn += 1;
            Minutes = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Seconds = 0;
        Minutes = 0;
        MultiplySpawn = 1;
    }
}
