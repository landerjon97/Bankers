/*
Author: Jonathan Lander
Class: CIS 355
Date: 10/20/19
About: Example of the bankers algorithm
*/
using System;
using System.Collections.Generic;
using System.IO;

namespace Jon_Lander_Bankers
{
    class Bankers
    {
        /*        
        static int[] instances;
        static int[,] processes;
        */
        static void Main(string[] args)
        {
            //Get relative to the project folder
            string path = Directory.GetCurrentDirectory();
            //Get the datafile and parse the file.
            FileParser fileParser = new FileParser(path + "/data.txt");
            //Initialize the the arrays by grabbing from the file parser class
            int[] instances = fileParser.getInstances();
            int[,] processes = fileParser.getProcesses();
            int[,] maxProcesses = fileParser.getMaxProcesses();
            //Print out the info that was just passed to the variables
            fileParser.printInfo();
            /******************************* BONUS ********************************/
            //Multiple Inputs
            Boolean done = false;
            while(!done){
                //Catch any issues that could have occured from user input
                try{
                    //get the process that needs updated
                    int processToUpdate = getRequestedProcess();
                    //grab the n number of each resource for the given process
                    int[] req = getRequest(processes.GetLength(1));
                    //update the instance
                    for(int i = 0; i < processes.GetLength(1); i++){
                        processes[processToUpdate, i] += req[i];
                    }
                }
                catch{
                    Console.WriteLine("You did not have the correct input, skipping this part.");
                    done = true;
                }
                if(!done){
                    //ask to do it again
                    Console.WriteLine("Enter another process request before running? y/n");
                    //if it is anything else other than the following just make it as no.
                    String yesNo = Console.ReadLine();
                    if(yesNo != "y" && yesNo != "Y" && yesNo != "yes" && yesNo != "Yes" && yesNo != "YES"){
                        done = true;
                    }
                    else{
                        done = false;
                    }
                }
            }
            /******************************* BONUS ********************************/
            //Print updated processes
            printInfo(processes,maxProcesses);
            //safetyCheck and tell if it succeeded.
            SafetyCheck safetyCheck = new SafetyCheck(instances, processes, maxProcesses);
            //print Final results.
            Console.WriteLine("Safe Sequence:");
            safetyCheck.printResults();
        }
        //getRequest (takes the process length)
        //it returns the array of the updated process.
        public static int[] getRequest(int process){
            //seperate the input by spaces
            char[] sepearator = {' '};
            Console.Write("Enter Request for the resource: ");
            String line = Console.ReadLine();
            //convert the string to an array
            String[] lines = line.Split(sepearator, process, StringSplitOptions.None);
            //set the return value to the length of lines
            int[] request = new int[lines.Length];
            //assign values
            for(int i =0; i < request.Length; i++){
                request[i] = Int32.Parse(lines[i]);
            }
            //return updated array
            return request;
        }
        //getRequestedProcess returns that process that will be updated in the future
        //Returns the process number
        public static int getRequestedProcess(){
            Console.Write("Enter Process for request: ");
            String request = Console.ReadLine();
            //parse from string to integer
            int requested = Int32.Parse(request);
            return requested;
        }
        //provides information of the current instances of each array in a friendly way
        //passes the state of processes and maxProcesses
        public static void printInfo(int[,] processes, int[,] maxProcesses){
            //Simply loops through each array printing the current state
            for(int i =0; i < processes.GetLength(0);i++){
                Console.Write("Process " + i + ": ");
                for (int j = 0; j< processes.GetLength(1);j++){
                    Console.Write(processes[i,j] + " ");
                }
                Console.Write("\n");
            }
            for(int i =0; i < maxProcesses.GetLength(0);i++){
                Console.Write("Max Process " + i + ": ");
                for (int j = 0; j< maxProcesses.GetLength(1);j++){
                    Console.Write(maxProcesses[i,j] + " ");
                }
                Console.Write("\n");
            }
        }
    }
    //FileParse class is responsible for reading the file and assgining all the values to the right arrays. (Should not need any error correction)
    class FileParser{
        //declare the values that will be initialized in the constructor
        int[] instances;
        int[,] processes;
        int[,] maxProcesses;
        string path;
        //Constructor takes the path of the file and begins to assign its values.
        public FileParser(string path){
            //assign to local variable
            this.path = path;
            //grabs the file content and stores each line in a string array
            String[] text  = getFileContents(path);
            //assigns the instances to instances varaible
            instances = getInstances(getResourcesTypes(text), text);
            //assigns the processes to the processes array
            processes = new int[getProcesses(text), instances.Length];
            //also grabs the maxprocesses from the file.
            maxProcesses = new int[getProcesses(text), instances.Length];
            //Console.WriteLine(processes.GetLength(0));
            //Console.WriteLine(processes.GetLength(1));
            processes = allocateProcesses(processes,text);
            maxProcesses = getMaxProcesses(maxProcesses,text);
        }
         //for Debugging, loads all lines into an array
        public string[] getFileContents(String path){
            String[] text = System.IO.File.ReadAllLines(path);
            return text;
        }
        //for Debuggin, prints the contents of the file.
        public  void printFile(String[] text){
            for(int i =0; i< text.Length; i++){
                Console.WriteLine(text[i]);
            }
        }
        //gets all the processses
        private int getProcesses(String[] text){
            return Int32.Parse(text[0]);
        }
        //get the number of resources
        private  int getResourcesTypes(String[] text){
            return Int32.Parse(text[1]);
        }
        
        private int[] getInstances(int resources, String[] text){
            char[] sepearator = {' '};
            String[] resourcesArr = text[2].Split(sepearator,resources,StringSplitOptions.None);
            int[] intResources = new int[resourcesArr.Length];
            for(int i = 0; i < resourcesArr.Length; i++){
                intResources[i] = Int32.Parse(resourcesArr[i]);
            }
            return intResources;
        }
        private int[,] allocateProcesses(int[,] processes, String[] text){
            char[] sepearator = {' '};
            for(int i = 0; i < processes.GetLength(0); i++){
                 String[] processValues = text[i+3].Split(sepearator,processes.GetLength(1),StringSplitOptions.None);
                for(int j =0; j < processes.GetLength(1);j++){
                    processes[i,j] = Int32.Parse(processValues[j]);
                }
            }
            return processes;
        }
        //get max 
        private int[,] getMaxProcesses(int[,] processes, String[] text){
            char[] sepearator = {' '};
            for(int i = 0; i < processes.GetLength(0); i++){
                 String[] processValues = text[i+3+processes.GetLength(0)].Split(sepearator,processes.GetLength(1),StringSplitOptions.None);
                for(int j =0; j < processes.GetLength(1);j++){
                    processes[i,j] = Int32.Parse(processValues[j]);
                }
            }
            return processes;
        }
        //print the current state of this class (same as the main function one)
        public void printInfo(){
            for(int i =0; i < processes.GetLength(0);i++){
                Console.Write("Process " + i + ": ");
                for (int j = 0; j< processes.GetLength(1);j++){
                    Console.Write(processes[i,j] + " ");
                }
                Console.Write("\n");
            }
            for(int i =0; i < maxProcesses.GetLength(0);i++){
                Console.Write("Max Process " + i + ": ");
                for (int j = 0; j< maxProcesses.GetLength(1);j++){
                    Console.Write(maxProcesses[i,j] + " ");
                }
                Console.Write("\n");
            }
        }
        //simple get statements for the main class
        public int[] getInstances(){
            return instances;
        }
        public int[,] getProcesses(){
            return processes;
        }
        public int[,] getMaxProcesses(){
            return maxProcesses;
        }
    }

    //SafetyCheck is responsible of seeing if it is possible to finish all the proccesses without deadlocks.
    class SafetyCheck{
        //delcare variables that will be used throughout.
        int[] instances;
        int[,] processes;
        int[,] maxProcesses;
        int[] currentInstance;
        //for printing the string
        List<String> order = new List<string>();
        //constructor takes all the arrays to do its calculations
        public SafetyCheck(int[] instances, int[,] processes, int[,] maxProcesses)
        {
            //assigns parameters to 
            this.instances = instances;
            this.processes = processes;
            this.maxProcesses = maxProcesses;
            //current state of the insstances
            currentInstance = instances;
            //set the current instances
            setCurrentInstance();
            //print the instances out
            /*
            for(int i = 0; i < currentInstance.Length; i++){
                Console.WriteLine(currentInstance[i]);
            }*/
            //perform the saftey check
            safteyCheck();
        }
        //print out the results
        public void printResults(){
            //print out the results
            if(order.Count < processes.GetLength(0)){
                Console.WriteLine("A deadlock has occured. Could not process all processes.");
            }
            else{
                for(int i =0; i< order.Count ;i++){
                    if (i == 0){
                        Console.Write("<");
                    }
                    if (i ==order.Count-1){
                        Console.Write(order[i] + ">");
                    }else{
                        Console.Write(order[i] + ", ");
                    }
                }
                Console.Write("\n");
                Console.ReadKey();
            }
        }
        //sets the currentInstances Variable
        private void setCurrentInstance(){
            for(int i = 0; i < processes.GetLength(0); i++){
                for(int j = 0; j < processes.GetLength(1); j++){
                    currentInstance[j] -= processes[i,j];
                }
            }
        }
        //Performs the basic safteyCheck.
        private void safteyCheck(){
            //tells if index needs reset (precents a 3rd loop)
            Boolean resetIndex = false;
            //go through all processses
            for(int i = 0; i < processes.GetLength(0); i++){
                //if index needs reset the reset it
                if(resetIndex){
                    i=0;
                }
                //tell if the proccess can be satified.
                Boolean failed = false;
                //count to check if the process is all zeros or not
                int count=0;
                //go through each instance of each process
                for (int j = 0; j < processes.GetLength(1);j++){
                    //if its zero the add to count
                    if(processes[i,j]==0){
                        count++;
                    }
                    //see if each instances of the process is posisble or not. (if it fails at all then stop checking all together.)
                    if(maxProcesses[i,j] <= currentInstance[j]+processes[i,j] && !failed){
                        failed = false;
                    }
                    else{
                        failed = true;
                    }
                }
                //check the results
                //if didnt fail and the process isn't all zeros
                if(!failed && count < 3){
                    //add the processes resources to the currentInstance
                    for(int j = 0; j < processes.GetLength(1); j++){
                        currentInstance[j] += processes[i,j];
                        //set all process instances to zero
                        processes[i,j] = 0;
                    }
                    //add to the order list
                    order.Add("P" + i);
                    //reset the index
                    resetIndex = true;
                }
                else{
                    //dont reset the index
                    resetIndex = false;
                }
            }
        }
        //Recursive version.
        private void safteyCheckRecursive(int[,] process, int[] currentInstances){
            for(int i = 0; i < processes.GetLength(0); i++){
                int count = 0;
                for(int j = 0; j < processes.GetLength(1);j++){
                    if(currentInstances[j] < maxProcesses[i,j]){
                        count = 0;
                    }
                    else{
                        count++;
                    }
                }
                if(count == currentInstances.Length){
                    Console.Write("P" + i + ", ");
                    for(int j = 0; j < process.GetLength(1); j++){
                        currentInstances[j] += process[i,j];
                    }

                    int[,] temp = new int[process.GetLength(0) - 1, process.GetLength(1)];
                    int index = i;
                    for(int j = 0; j < process.GetLength(0);  j++){
                        if(j!=index){
                            for(int k = 0; k < process.GetLength(1); k++){
                                temp[j,k] = process[j,k];
                            }
                            index = -1;
                            j--;
                        }
                    }
                    safteyCheckRecursive( temp, currentInstance);
                }
            }
        }
        
    }
}










/*
Implement the Banker's Algorithm in C# or C++ using Visual Studio.
This programming assignment will be weighted as twice as other assignments.
The algorithms that you need are
Resource-Request Algorithm on slide 8-27 and 
Safety Algorithm on slide 8-24.
Submission
Due on 11/1 (Fri)
Zip the whole project folder and submit the zip file to the D2L dropbox Banker's Algorithm.
You should name the zip file and your project folder FirstName_LastName_Bankers.
Don’t forget to include the input file data.txt in your project folder.
The input file "data.txt" should include the following data on slide 8-25:
5         (Number of processes)
3         (Number of resource types)
10 5 7 (Number of instances for R0, R1, R2))
0 1 0   (Allocation for P0)
2 0 0   (Allocation for P1)
3 0 2   (Allocation for P2)
2 1 1   (Allocation for P3)
0 0 2   (Allocation for P4)
7 5 3   (Max for P0)
3 2 2   (Max for P1)
9 0 2   (Max for P2)
2 2 2   (Max for P3)
4 3 3   (Max for P4)
Note that you need to calculate Available and Need in your program
Additional inputs from the keyboard
Which process is requesting the resources? -> Example: 1 for P1
What is the request for the resources? -> Example: 1 0 2 for (1, 0, 2)
Output to the screen
Show all the detail steps to make the decision (Yes or No).
When the decision is yes, you must show a safe sequence.
Test your program with the following inputs
Can a request for (1,0,2) by P1 be granted? -> Yes. Safe sequence: <1, 3, 4, 0, 2> or <1, 3, 0, 2, 4> or <3, 1, 2, 0, 4> or ...
Can a request for (3,3,0) by P4 be granted? -> No
Can a request for (0,2,0) by P0 be granted? -> Yes. Safe sequence: <3, 1, 0, 2, 4> or <3, 1, 2, 0, 4> or ...
Extra Credit
GUI: 20%
Multiple input: 5%
Start this program ASAP. Don’t wait till last minute.


 */