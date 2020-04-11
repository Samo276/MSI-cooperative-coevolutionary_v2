using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_projekt_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int ograniczenie_ilosci_pokolen = 20;
            int ilosc_wektorow = 100;
            double c = 0.1;

            //______________________________________________________________________________________________________________________________
            
            int[] wzorzec = Gen_215();
            Console.WriteLine("\nWzorzec: ");
            for (int i = 0; i < 215; i++)
                Console.Write(wzorzec[i]);
            
            Console.WriteLine("\n");

            //______________________________________________________________________________________________________________________________

            double licznik_60 = 0;
            double licznik_155 = 0;
            double licznik_215 = 0;
                
            int[] pr = Gen_215();
           for (int i = 0; i < 155; i++)
               if (wzorzec[i] == 0 ) licznik_155+=1.0;
           for (int i = 155; i < 215; i++)
               if (wzorzec[i] == 0 ) licznik_60+=1.0;

           for (int i = 0; i < 215; i++)
               if (wzorzec[i] == pr[i]) licznik_215+=1.0;

          //gen 1
           double fitness_full = (c * licznik_155 * licznik_60) + ((1.0 - c) * licznik_215);
           Console.WriteLine("fitnes optymalny: "+fitness_full);
          // Console.ReadLine();
           //zbior wszytskich x-tysięcy wektorow
           


            //______________________________________________________________________________________________________________________________
            //int[,] tab60 = new int[ilosc_wektorow,60]; // ich jest 1000 <-----
            //int[,] tab155 = new int[ilosc_wektorow,155];
            //---------------------------------------
            double[] fitness_tab = new double[ilosc_wektorow];
            /*int fitness_index = 0;
            double fitness_new = 0;
            double fitness_full = 0;*/
            //----------------------------------------
            /*int[] basic60 = Gen_60();
            int[] basic155 = new int[155];*/


            //______________________________________________________________________________________________________________________________
            //generowanie rodzicow i tworzenie tabel dla potomnych
            int[,] pop1 = new int[ilosc_wektorow, 215];
            int[,] pop1_children = new int[ilosc_wektorow, 215];
            int[,] pop1_new = new int[ilosc_wektorow, 215];
            
            
            int[,] pop2 = new int[ilosc_wektorow, 215];
            int[,] pop2_children = new int[ilosc_wektorow, 215];
            int[,] pop2_new = new int[ilosc_wektorow, 215];

            int[,] pop_final = new int[ilosc_wektorow, 215];

            pop1 = Gen_215(ilosc_wektorow);
            pop2 = Gen_215(ilosc_wektorow);

            int[] fitness_pop = new int[ilosc_wektorow];
            int[] fitness_pop_children = new int[ilosc_wektorow];
            

            //______________________________________________________________________________________________________________________________
            for (int i = 0; i < ograniczenie_ilosci_pokolen; i++)
            {

                Funkcja(pop1,pop1_new,pop1_children, ilosc_wektorow,wzorzec,c);
                

                Funkcja(pop2,pop2_new,pop2_children, ilosc_wektorow,wzorzec,c);
                pop_final = Cross_Final_Pop(pop1, pop2, ilosc_wektorow);
                Print(pop_final, wzorzec, ilosc_wektorow, c, i);

            }

            

            //______________________________________________________________________________________________________________________________

            Console.WriteLine("\nOK - skończone\n");
            Console.ReadKey();
                       
        }
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        static int[,] Cross_Final_Pop(int[,] pop1, int[,]pop2, int il_wektorow)
        {
            Random rng = new Random();

            int wylosowany_osobnik_155 = 0;
            int wylosowany_osobnik_60 = 0;

            int[,] pop_final = new int[il_wektorow, 215];
            
            for (int j = 0; j < il_wektorow; j++)
            {
                wylosowany_osobnik_155 = rng.Next(0, il_wektorow);
                for (int i = 0; i < 155; i++)
                    pop1[wylosowany_osobnik_155, i] = pop_final[j, i];
            }

            for (int j = 0; j < il_wektorow; j++)
            {
                wylosowany_osobnik_60 = rng.Next(0, il_wektorow);
                for (int i = 155; i < 215; i++)
                    pop2[wylosowany_osobnik_60, i] = pop_final[j, i];
            }
            return pop_final;
        }
        
        static void Funkcja(int[,]pop,int[,] pop_new, int[,] pop_children, int il_wektorow, int[]wzorzec, double c)
        {
            pop_children = Crossowanie(pop, il_wektorow);
            
            double[] fitness_pop = new double[il_wektorow];
            double[] fitness_pop_children = new double[il_wektorow];
            
            double licznik_215 = 0.0;
            double licznik_155 = 0.0;
            double licznik_60 = 0.0;
            
            for (int i = 0; i < 155; i++)
                if (wzorzec[i] == 0) licznik_155 += 1.0;
            for (int i = 155; i < 215; i++)
                if (wzorzec[i] == 0) licznik_60 += 1.0;

            for (int il = 0; il < il_wektorow; il++)
            {
                for (int i = 0; i < 215; i++)
                    if (wzorzec[i] == pop[il, i]) licznik_215 += 1.0;
                fitness_pop[il] = (c * licznik_155 * licznik_60) + ((1.0 - c) * licznik_215);

                for (int i = 0; i < 215; i++)
                    if (wzorzec[i] == pop_children[il, i]) licznik_215 += 1.0;
                fitness_pop_children[il] = (c * licznik_155 * licznik_60) + ((1.0 - c) * licznik_215);
            }

            QuickSort(fitness_pop, pop,0,il_wektorow);
            QuickSort(fitness_pop_children, pop_children,0,il_wektorow);

            for (int i = 0; i < 0.7 * il_wektorow; i++)
                for (int j = 0; j < 215; j++)
                    pop_new[i, j] = pop_children[i, j];

            for (int i = (int)(0.7 * il_wektorow); i < il_wektorow ; i++)
                for (int j = 0; j < 215; j++)
                    pop_new[i, j] = pop[i, j];

            pop = pop_new;
        }
        
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        //______________________________________________________________________________________________________________________________
        
        
        public static void QuickSort(double[] fitness, int[,]pop , int left, int right)
        {
            var i = left;
            var j = right;
            var pivot = fitness[(left + right) / 2];
            while (i < j)
            {
                while (fitness[i] < pivot) i++;
                while (fitness[j] > pivot) j--;
                if (i <= j)
                {
                    // swap
                    var tmp = fitness[i];
                    
                    int[] tmp2 = new int[215];
                    for (int k = 0; k < 215; k++)
                        tmp2[k] = pop[i, k];

                    fitness[i] = fitness[j];  
                    for (int k = 0; k < 215; k++)
                        pop[i, k] = pop[j, k];
                    i++;

                    fitness[j] = tmp;
                    for (int k = 0; k < 215; k++)
                        pop[j, k]=tmp2[k];
                    j--; 
                }
            }
            if (left < j) QuickSort(fitness,pop, left, j);
            if (i < right) QuickSort(fitness,pop, i, right);
        }
        
        static void Print(int[,]f_pop,int[]wzorzec,int il_wektorow, double c,int licznik)
        {
            double[] fitness_pop = new double[il_wektorow];
            double licznik_215 = 0.0;
            double licznik_155 = 0.0;
            double licznik_60 = 0.0;

            for (int i = 0; i < 155; i++)
                if (wzorzec[i] == 0) licznik_155 += 1.0;
            for (int i = 155; i < 215; i++)
                if (wzorzec[i] == 0) licznik_60 += 1.0;

            for (int il = 0; il < il_wektorow; il++)
            {
                for (int i = 0; i < 215; i++)
                    if (wzorzec[i] == f_pop[il, i]) licznik_215 += 1.0;
                fitness_pop[il] = (c * licznik_155 * licznik_60) + ((1.0 - c) * licznik_215);
            }
            
            QuickSort(fitness_pop, f_pop,0,215);
            
            /*double max = fitness_pop[0];
            int index = 0;
            for (int i = 1; i < il_wektorow-1; i++)
                if (max < fitness_pop[i])
                {
                    max = fitness_pop[i];
                    index = i;
                }*/
            

            Console.WriteLine();
            Console.WriteLine("Pokolenie: "+ licznik++);
            Console.WriteLine("Fitnes Wektora : {0}", fitness_pop[0]);
            Console.WriteLine();
        }
        //______________________________________________________________________________________________________________________________
        static int[,] Crossowanie(int[,]pop1, int ilosc_wektorow)
        {
            int wylosowany_osobnik_155 = 0;
            int wylosowany_osobnik_60 = 0;

            int[,] pop1_dzieci = new int[ilosc_wektorow, 215];

            Random rng = new Random();
            
            for (int j = 0; j < ilosc_wektorow; j++)
            {
                wylosowany_osobnik_155 = rng.Next(0, ilosc_wektorow);
                wylosowany_osobnik_60 = rng.Next(0, ilosc_wektorow);
                for (int i = 0; i < 155; i++)
                    pop1_dzieci[j, i] = pop1[wylosowany_osobnik_155, i];

                for (int i = 155; i < 215; i++)
                    pop1_dzieci[j, i] = pop1[wylosowany_osobnik_60, i];

            }
            
            /*for (int j = 0; j < ilosc_wektorow; j++)
            {
                wylosowany_osobnik_60 = rng.Next(0, ilosc_wektorow);
                for (int i = 155; i < 215; i++)
                    pop1[wylosowany_osobnik_60, i] = pop1_dzieci[j, i];
                
            }*/
            Console.WriteLine("drukowanie pop1");
            for (int k = 0; k < 2; k++)
            {
                for (int s = 0; s < 215; s++)
                {
                    Console.Write(pop1[k, s]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("_______________________________________________________");
            

            return pop1_dzieci;
        }
        //______________________________________________________________________________________________________________________________
        static int[] Gen_215()
        {
            Random rng = new Random();
            int[] tab = new int[215];
            for (int i = 0; i < 215; i++)
                tab[i] = rng.Next(0, 2);
            return tab;
        }
        static int[,] Gen_215(int liczba_wektorow)
        {
            Random rng = new Random();
            int[,] tab = new int[liczba_wektorow,215];
            for (int i = 0; i < liczba_wektorow; i++)
                for (int j = 0; j < 215; j++)
                    tab[i,j] = rng.Next(0, 2);
            return tab;
        }

        /*static int[] Gen_155()
        {
            Random rng = new Random();
            int[] tab = new int[155];
            for (int i = 0; i < 155; i++)
                tab[i] = rng.Next(0, 2);
            return tab;
        }
        static int[,] Gen_155(int numer_wektora)
        {
            Random rng = new Random();
            int[,] tabx2 = new int[numer_wektora,155];
            
            for (int j = 0; j < numer_wektora; j++)
                for (int i = 0; i < 155; i++)
                    tabx2[j,i] = rng.Next(0, 2);
            
            return tabx2;
        }
        static int[] Gen_60()
        {
            Random rng = new Random();
            int[] tab = new int[60];
            for (int i = 0; i < 60; i++)
                tab[i] = rng.Next(0, 2);
            return tab;
        }
        static int[,] Gen_60(int numer_wektora)
        {
            Random rng = new Random();
            int[,] tab = new int[numer_wektora,60];
            for(int j = 0; j<numer_wektora;j++)
                for (int i = 0; i < 60; i++)
                    tab[j,i] = rng.Next(0, 2);
            
            return tab;
        }*/
    }
}
