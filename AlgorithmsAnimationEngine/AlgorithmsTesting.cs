using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsAnimationEngine {
    public static class AlgorithmsTesting {
        private static Random rand = new Random();

        public static void Swap<T>(T[] array, string arrayName, int i, int j, AlgorithmsAnimation animation) {
            animation.PushFunctionStack($"ArraySwap({arrayName}, {i}, {j})");
            animation.SwapArrayValues(arrayName, i, j);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
            animation.PopFunctionStack();
        }

        public static int Partition(int[] array, string arrayName, int start, int end, int pivotId, AlgorithmsAnimation animation) {
            animation.PushFunctionStack($"Partition");
            animation.DeclareVariable("start");
            animation.DeclareVariable("end");
            animation.DeclareVariable("pivotId");
            animation.SetVariable("start", start.ToString());
            animation.SetVariable("end", end.ToString());
            animation.SetVariable("pivotId", pivotId.ToString());
            int x = array[pivotId];

            animation.DeclareVariable("pivot");
            animation.SetVariable("pivot", x.ToString());


            Swap(array, arrayName, pivotId, end, animation);
            int i = start;
            animation.DeclareVariable("i");
            animation.SetVariable("i", i.ToString());
            animation.DeclareVariable("j");
            for (int j = start; j <= end; j++) {
                animation.SetVariable("j", j.ToString());
                if (array[j] < x) {
                    Swap(array, arrayName, i++, j, animation);
                    animation.SetVariable("i", i.ToString());
                }
            }
            Swap(array, arrayName, i, end, animation);
            animation.PopFunctionStack();
            return i;
        }

        public static int LasVegasRandomizedSelect(int[] array, string arrayName, int start, int end, int i, AlgorithmsAnimation animation) {
            animation.PushFunctionStack("Select");
            animation.DeclareVariable("start");
            animation.DeclareVariable("end");
            animation.DeclareVariable("i");
            animation.SetVariable("start", start.ToString());
            animation.SetVariable("end", end.ToString());
            animation.SetVariable("i", i.ToString());

            if (start == end) {
                animation.PopFunctionStack();
                return array[start];
            }

            int pivot = Partition(array, arrayName, start, end, rand.Next(start, end + 1), animation);
            animation.DeclareVariable("pivot");
            animation.SetVariable("pivot", pivot.ToString());
            int k = pivot - start + 1;
            animation.DeclareVariable("k");
            animation.SetVariable("k", k.ToString());

            int res;

            if (k == i)
                res = array[pivot];
            else if (i < k)
                res = LasVegasRandomizedSelect(array, arrayName, start, pivot - 1, i, animation);
            else
                res = LasVegasRandomizedSelect(array, arrayName, pivot + 1, end, i - k, animation);

            animation.PopFunctionStack();
            return res;
        }

    }
}
