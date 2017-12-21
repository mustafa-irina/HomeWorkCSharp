using System;
using System.Threading.Tasks;

namespace WaitFree
{

    internal class Register
    {
        static Object updLock = new Object();

        private int id;
        private int data;
        private bool[] bitmask;
        private bool toggle;
        private int[] view;

        public Register(int data, int id, int n)
        {
            this.id = id;
            this.data = data;
            bitmask = new bool[n];
            view = new int[n];
        }

        public bool[] GetBitmask()
        {
            return bitmask;
        }

        public bool GetToggle()
        {
            return toggle;
        }

        public int GetData()
        {
            return data;
        }

        public int[] GetView()
        {
            return view;
        }

        public void AtomicUpdate(int newData, bool[] newBitmask, bool newToggle, int[] snapshot)
        {

            lock (updLock)
            {
                this.bitmask = newBitmask;
                this.toggle = newToggle;
                this.view = snapshot;

                this.data = newData;
                Console.WriteLine("Task {0} updated register {2} ; data = {1}\n", Task.CurrentId, data, id);
            }
        }
    }
}