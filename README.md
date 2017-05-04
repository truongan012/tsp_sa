# tsp_sa
Traveling Salesman Problem with Simulated Annealing Algorithm

Syntax: [FileName] [Options]

FileName  :File name path, file format as TSPLIB95, egde weigh is EUC_2D

Options:

    -p    : Percentage of intial solution cost (intial temperature)
    -f    : Final temperature that will stop program
    -r    : Temperature reduction factor

For Example: eli51.tsp -p 10 -f 0.001 -r 0.99
