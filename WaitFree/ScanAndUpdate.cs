namespace WaitFree
{
    class ScanAndUpdate
    {
        private Register[] registers;
        private readonly bool[,] q_hand_shakes;

        public ScanAndUpdate(Register[] regs)
        {
            this.registers = regs;
            this.q_hand_shakes = new bool[registers.Length, registers.Length];
        }

        public int[] Scan(int ind = 0)
        {
            var moved = new bool[registers.Length];

            while (true)
            {
                for (var j = 0; j < registers.Length; j++)
                {
                    q_hand_shakes[ind, j] = registers[j].GetBitmask()[ind];
                }

                var a = Collect();
                var b = Collect();

                var result = true;
                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k].GetBitmask()[ind] == b[k].GetBitmask()[ind] &&
                        b[k].GetBitmask()[ind] == q_hand_shakes[ind, k] &&
                        a[k].GetToggle() == b[k].GetToggle()) continue;
                    if (moved[k])
                    {
                        return b[k].GetView();
                    }
                    moved[k] = true;

                    result = false;
                    break;
                }

                if (!result) continue;

                var view = new int[registers.Length];
                for (var i = 0; i < registers.Length; i++)
                    view[i] = a[i].GetData();
                return view;
            }
        }

        public void Update(int i, int value)
        {
            var newBitmask = new bool[registers.Length];
            for (var j = 0; j < registers.Length; j++)
                newBitmask[j] = !q_hand_shakes[j, i];
            var view = Scan(i);

            registers[i].AtomicUpdate(value,
                newBitmask, !registers[i].GetToggle(), view);
        }

        private Register[] Collect()
        {
            return (Register[])registers.Clone();
        }

    }



}