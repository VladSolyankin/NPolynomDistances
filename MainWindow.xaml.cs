using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NPolynomDistances
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rules.Text = System.IO.File.ReadAllText("C:\\Users\\79117\\source\\repos\\NPolynomDistances\\rulesText.txt");
        }

        private void MainBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e)
        {
            if (polynomInput.Text.Length == 0 || xInput.Text.Length == 0)
            {
                ErrorWindow errorWindow = new ErrorWindow();
                errorWindow.errorTextBlock.Text = "Поля не могут быть пустыми!";
                errorWindow.Show();
            }

            else {
                ResultWindow resultWindow = new ResultWindow();
                
                resultWindow.resultXTextBlock.Text = GetPolynomeValue().ToString();

                ShowPolynomSolveDistances(resultWindow);

                resultWindow.Show();

                this.Hide();
            }
        }

        private void polynomInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,-]+").IsMatch(e.Text);
        }

        private void xInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9,-]+").IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void polynomInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] coefs = polynomInput.Text.Trim().Split(' ');
            string formula = "";
            for (int i = 0; i < coefs.Length; i++) {
                if (coefs[i].Contains("-") == false && coefs[i] != "0")
                    formula += "+" + coefs[i] + "x^" + (coefs.Length - i - 1).ToString();
                else if (coefs[i] == "0")
                    formula += "";
                else
                    formula += coefs[i] + "x^" + (coefs.Length - i - 1).ToString();
            }
            polynomFormula.Formula = formula;
        }

        public double GetPolynomeValue()
        {
            string[] coefs = polynomInput.Text.Split(' ');
            double xValue = Convert.ToDouble(xInput.Text);
            double res = 0;

            for (int i = 0; i < coefs.Length; i++)
                res += Convert.ToDouble(coefs[i]) * Math.Pow(xValue, coefs.Length - i - 1);

            return res;
        }

        public void ShowPolynomSolveDistances(ResultWindow resultWindow)
        {
            string[] coefs = polynomInput.Text.Split(' ');
            int counter = 0;
            for (double i = -20, j = -19.9; j <= 21; i += 0.1, j += 0.1)
            {
                double rangeSumI = 0;
                double rangeSumJ = 0;

                for (int k = 0; k < coefs.Length; k++)
                {
                    rangeSumI += Convert.ToDouble(coefs[k]) * Math.Pow(i, coefs.Length - k - 1);
                    rangeSumJ += Convert.ToDouble(coefs[k]) * Math.Pow(j, coefs.Length - k - 1);
                }

                if (rangeSumI > 0 && rangeSumJ < 0 || rangeSumI < 0 && rangeSumJ > 0)
                    resultWindow.rangesTextBlock.Text += $"{++counter}. [{Math.Round(i, 2)}; {Math.Round(i + 0.1, 2)}]\n";
            }
        }
    }
}
