using System.Collections;
using System.Collections.Generic;

public class Sort
{
    public static void SelectionSort(int[] args)
    {
        int[] arr = args;
        int n = args.Length;
        int temp, smallest;
        for (int i = 0; i < n - 1; i++)
        {
            smallest = i;
            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[smallest])
                {
                    smallest = j;
                }
            }
            temp = arr[smallest];
            arr[smallest] = arr[i];
            arr[i] = temp;
        }
    }
}
