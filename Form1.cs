using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace KursOS
{

    enum Сell
    {
        empty = -1,
        interrupt = -2
    }

    enum Sizes
    {
        page = 5,
        maxPageNum = 5,
        maxNum = 16,
        maxTablNum = 40
        
    }

    public partial class Form1 : Form
    {
        List<int> numbers = new List<int>(); // лист для заолнения таблицы
        int prer = 0; // количество прерываний
        bool first = true; // первый ли проход
        public Form1()
        {
            InitializeComponent();
            var myToolTip = new ToolTip(); // подсказки
            myToolTip.SetToolTip(textBox1, "Введите до 5 чисел через пробел (от 0 до 16).\nЭту строку можно оставить пустой ");
            myToolTip.SetToolTip(textBox2, "Введите до 40 чисел через пробел (от 0 до 16).");
            myToolTip.SetToolTip(button2, "Очистить ввод и таблицу");
        }

        private void button1_Click(object sender, EventArgs e) // кнопка Старт
        {
            if (!first) // очистка таблицы после предыдущего раза
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                label4.Text = "";
                numbers.Clear();
                Program.saves.Clear();
            }
            first = false;
            bool check = true; // проверка на валидность
            // блок проверки ввода
            { 
                try
                {
                    string block = Convert.ToString(textBox1.Text);
                    string tabl = Convert.ToString(textBox2.Text);
                    string bit = Convert.ToString(textBox3.Text);
                    string bitblok = Convert.ToString(textBox4.Text);

                    if (textBox2.Text == "")
                    {
                        check = false;
                    }
                    if (textBox3.Text == "")
                    {
                        check = false;
                    }
                    if (textBox1.Text == "")
                    {
                        block = "non";
                    }
                    check = Program.StringCheckBlock(block);
                    if (block.Length != bitblok.Length)
                    {
                        MessageBox.Show("Введите бит модификации для каждой страницы!");
                        check = false;
                    }
                    if (tabl.Length != bit.Length)
                    {
                        MessageBox.Show("Введите бит модификации для каждой страницы!");
                        check = false;
                    }
                    if (check)
                    {
                        check = Program.StringCheckBits(bit);
                    }
                    if (check)
                        check = Program.StringCheckTabls(tabl);
                    if (check)
                    {
                        check = Program.StringCheckBits(bitblok);
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Вводите числа!");
                    check = false;
                }
            }

            if (check == true)
            {
                string bitblok = Convert.ToString(textBox4.Text);
                string bit = Convert.ToString(textBox3.Text);
                string block = Convert.ToString(textBox1.Text);
                if (textBox1.Text == "")
                {
                    block = "non";
                }
                // работа алгоритма
                numbers = Program.Algorithm(Program.StringCheckBit(Convert.ToString(bitblok)),Program.StringCheckBit(Convert.ToString(bit)),Program.ScanBlock(Convert.ToString(block)), Program.ScanTabl(Convert.ToString(textBox2.Text.ToString())));
                int[] tabls = Program.ScanTabl(textBox2.Text.ToString());



                //подготовка (создание колонок и строк
                {

                    for (int i = 0; i < tabls.Length; i++) // вывод колонок
                    {
                        dataGridView1.Columns.Add(i.ToString(), Convert.ToString(tabls[i]));
                    }

                    for (int i = 0; i < tabls.Length; i++)  // задаю ширину
                        dataGridView1.Columns[i].Width = 25; // установка ширины ячейки
                    for (int i = 1; i < 6; i++) // начальная строка
                    {
                        dataGridView1.Rows.Add(" "); // заполнение 1 столбца
                    }
                }

                //заполнение таблицы
                {
                    int k = 0; // счётчик прохода по numbers
                    int q = 0; // счётчик прохода по saves
                    prer = 0;
                    for (int i = 0; i < tabls.Length; i++)
                    {
                        for (int j = 0; j < (int)Sizes.page; j++)
                        {
                            
                            if (numbers[k] == (int)Сell.interrupt)
                            {
                                //for (int i2 = 0; i2 < dataGridView1.Rows.Count; i2++)
                                {
                                    // dataGridView1[i, 0].Style.BackColor = Color.LightSeaGreen;
                                    dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.LightGreen;

                                    prer++;
                                }
                                k++;
                            }
                            if (Program.saves[q+j] == 1)
                            {
                                dataGridView1[i, j].Style.BackColor = Color.Pink;
                            }
                            if (Program.saves[q + j] == 2)
                            {
                                dataGridView1[i, j].Style.BackColor = Color.Aqua;
                            }



                            if (numbers[k] == (int)Сell.empty)
                            {
                                dataGridView1.Rows[j].Cells[i].Value = " ";
                                k++;
                            }
                            else
                            {
                                dataGridView1.Rows[j].Cells[i].Value = numbers[k];
                                k++;
                            }
                           
                        }
                        q += 5;
                    }
                 //   prer /= 5;
                    prer = tabls.Length - prer;
                    label4.Text = prer.ToString(); // вывод прерываний
                }
            }

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dataGridView1.Columns) 
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable; // запрет на сортировку по столбцам
            }
        }

        private void разработчикToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка Сброс
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            label4.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            numbers.Clear();
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void исходныеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void конечныйРезультатToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileDialog1.FileName;
                    // читаем файл в строку
                    string block = File.ReadLines(filename).First();
                    string tabl = File.ReadLines(filename).Skip(1).First();
                    string bitblock = File.ReadLines(filename).Skip(2).First();
                    string bittabl = File.ReadLines(filename).Skip(3).First();
                    textBox1.Text = block;
                    textBox2.Text = tabl;
                    textBox3.Text = bitblock;
                    textBox4.Text = bittabl;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка файла!");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл



                    File.WriteAllText(filename, textBox1.Text + '\n' + textBox2.Text + '\n' + textBox3.Text + '\n' + textBox4.Text);

                    MessageBox.Show("Файл сохранен");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения файла!");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл
                    string block = Convert.ToString(textBox1.Text);

                    string tabl = Convert.ToString(textBox2.Text);
                    string bitblock = Convert.ToString(textBox3.Text);
                    string bittabl = Convert.ToString(textBox4.Text);
                    StreamWriter f = new StreamWriter(filename);
                    f.WriteLine("Исходные данные: " + block);
                    f.WriteLine("Биты исходных данных: " + bitblock);
                    f.WriteLine("Подгружаемые стриницы: " + tabl);
                    f.WriteLine("Биты подгружаемых страниц: " + bittabl);
                    f.WriteLine("Результат работы алгоритма вторая попытка");
                    f.WriteLine("Количество прерываний: " + prer);

                    //сохраняем таблицу в файл
                    for (int i = 0; i < numbers.Count - (int)Sizes.page;)
                    {
                        for (int j = 0; j < (int)Sizes.page; j++)
                        {
                            if (numbers[i] == (int)Сell.interrupt)
                            {
                                f.Write("Нет прерывания ");
                                f.Write(numbers[++i] + " ");
                                i++;
                            }
                            else if (numbers[i] == (int)Сell.empty)
                            {
                                f.Write("  ");
                                i++;
                            }
                            else
                            {
                                f.Write(numbers[i++] + " ");
                            }

                        }
                        f.WriteLine("");
                    }
                    f.Close();
                    MessageBox.Show("Файл сохранен");
                }


            }

            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения файла!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @"Help.chm", HelpNavigator.TableOfContents);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программный комплекс разработан студенткой\nСПбГТИ(ТУ) факультета ИТиУ 475 группы:\nПекер Валерией ", "Разработчик");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

