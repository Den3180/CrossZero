using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace CrossZero
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int arraySize = 3;
        Canvas[,] canvasArr;
        public MainWindow()
        {
            InitializeComponent();
            FindCanvas();           
        }
        public static IEnumerable<Canvas> CanvasCollection(DependencyObject obj) 
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj,i);
                    if (child != null && child is Canvas)
                    {
                        yield return (Canvas)child;
                    }
                    foreach (var item in CanvasCollection(child))
                    {
                        yield return item;
                    }
                }
            }
        }
        public void FindCanvas()
        {
            canvasArr = new Canvas[arraySize,arraySize];
            int indexI = 0;
            int indexJ = 0;
            MyGrid.IsEnabled = false;
            foreach (Canvas can in CanvasCollection(MyGrid))
            {
                canvasArr[indexI, indexJ] = can;
                indexJ++;
                if (indexJ % arraySize == 0)
                {
                    indexI++;
                    indexJ = 0;
                }
            }
        }

        public void DrawCross(Canvas canvas)
        {
            if (canvas != null && canvas.Children.Count == 0)
            {
                Line line1 = new Line
                {
                    X1 = 0,
                    Y1 = 0,
                    X2 = canvas.ActualWidth,
                    Y2 = canvas.ActualHeight,
                    Stroke = Brushes.Red,
                    StrokeThickness = 10,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                canvas.Children.Add(line1);

                Line line2 = new Line
                {
                    X1 = canvas.ActualWidth,
                    Y1 = 0,
                    X2 = 0,
                    Y2 = canvas.ActualHeight,
                    Stroke = Brushes.Red,
                    StrokeThickness = 10,
                    StrokeStartLineCap = PenLineCap.Round,
                    StrokeEndLineCap = PenLineCap.Round
                };
                canvas.Children.Add(line2);
            }
        }
        public void DrawCircle(Canvas canvas)
        {
            if (canvas!=null&&canvas.Children.Count==0)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = canvas.ActualWidth,
                    Height = canvas.ActualHeight,
                    Stroke = Brushes.Blue,
                    StrokeThickness = 10
                };
                canvas.Children.Add(ellipse);
            }
        }
        public bool CheckTheVictory(string symbol)
        {
            for (int i = 0; i < arraySize; i++)
            {
                int checkWin = 0;
                for (int j = 0; j < arraySize; j++)
                {                  
                if ((string)canvasArr[0, j].Tag == symbol && (string)canvasArr[1, j].Tag == symbol && (string)canvasArr[arraySize - 1,j].Tag == symbol)
                {
                    return true;
                }
                    if( canvasArr[i,j]!=null&&(string)canvasArr[i,j].Tag==symbol)
                    {
                        checkWin++;
                    }
                }
                if (checkWin == arraySize)
                {
                 return true;
                }
            }          
            if ((string)canvasArr[0,0].Tag==symbol&& (string)canvasArr[1,1].Tag==symbol&& (string)canvasArr[arraySize-1, arraySize-1].Tag==symbol)
            {
                return true;
            }
            if ((string)canvasArr[0, arraySize-1].Tag == symbol && (string)canvasArr[1, 1].Tag == symbol && (string)canvasArr[arraySize - 1, 0].Tag == symbol)
            {
                return true;
            }
            return false;
        }

        public void MoveComputer(string symbol_0, string symbol_x)
        {
            Canvas canvas= CheckAll(symbol_0);
            if (canvas==null)
            {
            canvas = CheckAll(symbol_x);
            }
            if(canvas!=null)
            {
                DrawCircle(canvas);
            }
            else
            {
               canvas = ChooseCell();
               DrawCircle(canvas);
            }
            if (canvas != null) { canvas.Tag = "0"; }
            if (CheckTheVictory(symbol_0))
            {
                MessageBox.Show("Computer win!");
                GameYesOrNo();
            }
        }
        
        public Canvas ChooseCell()
        {
            Random rand = new Random();
            int index_i = rand.Next(3);
            int index_j = rand.Next(3);
            int countElement = 0;
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    countElement++;
                    if (canvasArr[i, j].Tag!=null&&SmallCheck(i,j)!=null)
                    {
                        return SmallCheck(i,j); 
                    }                  
                }
            }
            return canvasArr[index_i,index_j];
        }

        public Canvas SmallCheck(int index_i, int index_j)
        {
            for(int i=index_i-1;i<=index_i+1;i++)
            {
                for (int j = index_j-1; j <= index_j + 1; j++)
                {
                    if (i >= 0 && i < arraySize && j >= 0 && j < arraySize && canvasArr[i, j].Children.Count == 0)
                    {
                        return canvasArr[i, j];
                    }
                }
            }
            return null;
        }

        public Canvas CheckAll(string symbol)
        {
            for(int i=0;i<arraySize;i++)
            {
            int countSymbol_X = 0;
            int countSymbol_Y = 0;           
                for (int j = 0; j < arraySize; j++)
                {
                    if ((string)canvasArr[i, j].Tag == symbol)
                    {
                        countSymbol_X++;
                        if(countSymbol_X==2)
                        {
                            for (int k = 0; k < arraySize; k++)
                            {
                                if (canvasArr[i, k].Tag ==null)
                                {
                                    return canvasArr[i, k];
                                }
                            }
                        }
                    }
                    if((string)canvasArr[j,i].Tag==symbol)
                    {
                        countSymbol_Y++;
                        if(countSymbol_Y==2)
                        {
                            for (int k = 0; k < arraySize; k++)
                            {
                                if (canvasArr[k,i].Tag == null)
                                {
                                    return canvasArr[k,i];
                                }
                            }
                        }
                    }
                    
                }
            }                    
                return CheckDiagonal(symbol);                     
        }
        public Canvas CheckDiagonal(string symbol)
        {
            Canvas canvas = null;
            int countSymbol = 0;
            for(int i=0,j=0;i<arraySize&&j<arraySize;i++,j++)
            {
                if (canvasArr[i, j].Tag == null)
                {
                    canvas = canvasArr[i, j];
                }
                if((string)canvasArr[i,j].Tag==symbol)
                {
                    countSymbol++;
                }
                if(countSymbol==2&&canvas!=null)
                {
                return canvas;
                }               
            }
            countSymbol = 0;
            canvas = null;
            for (int i = 0, j = arraySize - 1; i < arraySize && j >= 0; i++, j--)
            {
                if (canvasArr[i, j].Tag == null)
                {
                    canvas = canvasArr[i, j];
                }
                if ((string)canvasArr[i, j].Tag == symbol)
                {
                    countSymbol++;
                }
                if (countSymbol == 2&&canvas!=null)
                {
                    return canvas;
                }
            }
            return null;
        }

        public void GameYesOrNo()
        {
         MessageBoxResult result=MessageBox.Show("Играть еще?","", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                this.Close();
            }
            else
            {
                for (int i = 0; i < arraySize; i++)
                {
                    for(int j=0;j<arraySize;j++)
                    {
                        canvasArr[i, j].Children.Clear();
                        canvasArr[i, j].Tag= null;
                    }
                }
                Start.IsEnabled = true;
                MyGrid.IsEnabled = false;
                this.UpdateLayout();
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Canvas canvas = ((Border)sender).Child as Canvas;
            if (canvas!=null) 
            {
                DrawCross(canvas);
                canvas.Tag ="x";
                
                if (CheckTheVictory("x"))
                {
                    MessageBox.Show("You win!");                    
                    GameYesOrNo();
                }
            }              
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Child is Canvas)
            {
            Thread.Sleep(500);
            MoveComputer("0", "x");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyGrid.IsEnabled = true;
            Thread.Sleep(100);
            MoveComputer("0", "x");
            ((Button)sender).IsEnabled = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
