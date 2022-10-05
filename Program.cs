using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursOS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
           // Application.EnableVisualStyles(); не понял за что это отвечает..
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }        

        public static int[] ScanBlock(string array)
        {
            int[] blocks = new int[(int)Sizes.page];
            if (array != "non")
            {
                string[] block = array.Split(' ');

                for (int i = 0; i < block.Length; i++)

                {
                    blocks[i] = Convert.ToInt32(block[i]);
                }
                if (block.Length < (int)Sizes.page)
                {
                    for (int i = block.Length; i < blocks.Length; i++)
                    {
                        blocks[i] = (int)Сell.empty;
                    }
                }
            }
            else
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i] = (int)Сell.empty;
                }
            }
            return blocks;

        }

        public static int[] ScanTabl(string array)

        {
            
            string[] tabl = array.Split(' ');

            int[] tabls = new int[tabl.Length];


            for (int i = 0; i < tabl.Length; i++)

            {

                tabls[i] = Convert.ToInt32(tabl[i]);

            }
            return tabls;

        }

        public static bool StringCheckBlock(string block)
        {
            bool check = true;
            if (block != "non")
            {
                string[] blocks = block.Split(' ');
                int[] blocksi = new int[blocks.Length];

                for (int i = 0; i < blocks.Length; i++)

                {

                    blocksi[i] = Convert.ToInt32(blocks[i]);

                }
                int tmp = blocksi[0];
                for (int j = 0; j < blocksi.Length - 1; j++)
                {
                    for (int i = j + 1; i < blocksi.Length; i++)
                    {
                        if (tmp == (int)Сell.empty)
                            break;
                        if (tmp == blocksi[i])
                        {
                            MessageBox.Show("Нет смысла делать страницы одинаковыми");
                            check = false;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        break;
                    }
                    tmp = blocksi[j + 1];
                }


                if (blocksi.Length > (int)Sizes.maxPageNum)
                {
                    MessageBox.Show("Страниц не может быть больше 5");
                    check = false;
                }

                for (int i = 0; i < blocksi.Length; i++)
                {
                    if (blocksi[i] > (int)Sizes.maxNum)
                    {
                        MessageBox.Show("Вводите число от 0 до 16");
                        check = false;
                        break;
                    }
                }
            }
            return check;
        }

        public static bool StringCheckTabls(string tabl)
        {
            bool check = true;
            string[] tablsing = tabl.Split(' ');
            int[] tablsi = new int[tablsing.Length];

            for (int i = 0; i < tablsing.Length; i++)

            {
                tablsi[i] = Convert.ToInt32(tablsing[i]);
            }
            for (int i = 0; i < tablsi.Length; i++)
            {
                if (tablsi[i] > (int)Sizes.maxNum)
                {
                    MessageBox.Show("Вводите число от 0 до 16");
                    check = false;
                    break;
                }
            }
            if (tablsi.Length > (int)Sizes.maxTablNum)
            {
                MessageBox.Show("Вводите не больше 40 страниц");
                check = false;
            }
            return check;
        }

        public static bool StringCheckBits(string bit)
        {
            bool check = true;
            string[] binsing = bit.Split(' ');
            int[] bits = new int[binsing.Length];

            for (int i = 0; i < binsing.Length; i++)

            {
                bits[i] = Convert.ToInt32(binsing[i]);
            }
            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i] > 1 || bit[i] < 0)
                {
                    MessageBox.Show("Вводите число от 0 до 1");
                    check = false;
                    break;
                }
            }
            return check;
        }


        public static int[] StringCheckBit(string array)
        {
            string[] bit = array.Split(' ');

            int[] bits = new int[bit.Length];


            for (int i = 0; i < bit.Length; i++)

            {

                bits[i] = Convert.ToInt32(bit[i]);

            }
            return bits;
        }


        public static int[][] nums = new int[5][];
        public static List<int> saves = new List<int>();

        public static List<int> Algorithm(int[]bitblok, int[]bit, int[] blocks, int[] tabl)
        {

            
            for (int p = 0; p < blocks.Length; p++)
                nums[p] = new int[2];
            for (int p = 0; p < blocks.Length; p++)
                nums[p][0] = blocks[p];
            for (int p = 0; p < blocks.Length; p++)
                nums[p][1] = bitblok[p];





            List<int> numbers = new List<int>();            
            int blank = 0; // если приходит пустая ячейка, то заполняется не сдвигом а подменой. Подсчёт таких ячеек.
            for (int p = 0; p < blocks.Length; p++)
            {
                
                if (nums[p][0] == (int)Сell.empty)
                {
                    break;
                }
                else
                {
                    blank++;
                }
            }


            bool first = true;
            int time = 0;
            for (int i = 0; i < tabl.Length; i++)
            {
                bool flag = true; // Флаг. true - страница найдена в страничном блоке. false - не найдена.
                
                for (int j = 0; j < blocks.Length; j++)
                {
                    if (nums[j][0] == tabl[i])
                    {
                       
                        numbers.Add((int)Сell.interrupt); // нет прерывания
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                for (int j = 0; j < blocks.Length; j++)
                {
                    if (nums[j][0] == tabl[i])
                    {
                        nums[j][1] += 1;
                    }
                }
                if (first)
                {
                    first = false;

                }

                
                if (flag == false)
                {                                      
                    if (blank < blocks.Length && nums[blank][0] == (int)Сell.empty)
                    {
                        nums[blank][0] = tabl[i];
                        blank++;
                    }
                    else
                    {
                        
                        for (int q = 0;; q++)
                        {
                            if (time > 4)
                                time = 0;
                            if (nums[time][1] == 0)
                            {
                                nums[time][0] = tabl[i];
                                nums[time][1] = bit[i];
                                time += 1;
                                break;
                            }
                            else
                            {
                                nums[time][1] -= 1;
                            }
                            time++;
                        }



                        /////////////////////////////
                        /*
                        int kol = 0;
                        for (int q = 0; q < blocks.Length; q++)
                        {
                            if (nums[q][1] > 0)
                            {
                                kol++;
                            }
                        }
                        if (nums[0][1] == 0 || kol == blocks.Length)
                        {
                            int k = 0;
                            while (k < blocks.Length - 1) // сдвиг массива
                            {
                                nums[k][0] = nums[k + 1][0];
                                nums[k][1] = nums[k + 1][1];
                                k++;
                            }
                            nums[blocks.Length - 1][0] = tabl[i]; // вставка элемента в конец
                             nums[blocks.Length - 1][1] = 0;
                        }
                        else
                        {
                            int r = 0; int k = 0;
                            while (nums[0][1] > 0)
                            {
                                int tmp = nums[0][0];
                                k = 0;
                                while (k < blocks.Length - 1) // сдвиг массива
                                {
                                    nums[k][0] = nums[k + 1][0];
                                    nums[k][1] = nums[k + 1][1];
                                    k++;
                                }
                                nums[blocks.Length - 1][0] = tmp;
                                nums[blocks.Length - 1][1] -= 1;
                                r++;
                            }

                            //nums[blocks.Length - 1][0] = tabl[i]; // вставка элемента в конец
                            k = 0;
                            while (k < blocks.Length - 1) // сдвиг массива
                            {
                                nums[k][0] = nums[k + 1][0];
                                nums[k][1] = nums[k + 1][1];
                                k++;
                            }
                            nums[blocks.Length - 1][0] = tabl[i]; // вставка элемента в конец
                            nums[blocks.Length - 1][1] = bit[i];
                        }
                        */
                        /////////////////////////////
                    }
                }
                for (int k = 0; k < blocks.Length; k++)
                {
                    numbers.Add(nums[k][0]);
                }
                if (!first)
                {
                    for (int j = 0; j < blocks.Length; j++)
                    {
                        saves.Add(nums[j][1]);
                    }
                }
            }
            return numbers;
        }
    }
}
