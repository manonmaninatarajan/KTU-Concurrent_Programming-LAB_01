#include <iostream>
#include <fstream>
#include <vector>
#include <string>
#include <sstream>
#include <iomanip>
#include <omp.h>
#include <algorithm> 

class mydata {
private:
    std::string Text;
    int Number;
    double Value;

public:
    mydata() : Text(""), Number(0), Value(0.0) {}
    mydata(std::string text, int number, double value)
        : Text(text), Number(number), Value(value) {}
    std::string getText() const { return Text; }//get set method
    void setText(const std::string& text) { Text = text; }

    int getNumber() const { return Number; }
    void setNumber(int number) { Number = number; }

    double getValue() const { return Value; }
    void setValue(double value) { Value = value; }
    std::string ToString() const //override method
    {
        std::ostringstream oss;
        oss << "| " << std::left << std::setw(20) << Text
            << " | " << std::setw(8) << Number
            << " | " << std::setw(8) << Value << " |";
        return oss.str();
    }
    int ComputeResult() const//compute result
    {
        return Number * static_cast<int>(Value);
    }
    bool FitsCriterion() const 
    {
        return Number % 2 == 0; //Criteria to check if it goes to result file or not;if even number then goes to result fike
    }
 
};
void Read(const std::string& filePath, std::vector<mydata>& listdata) //Method to read data from the data file.
{
    std::ifstream file(filePath);
    std::string line;
    while (std::getline(file, line)) {
        std::istringstream iss(line);
        std::string text;
        int number;
        double value;
        char delimiter;

        if (std::getline(iss, text, ';') &&
            iss >> number >> delimiter &&
            iss >> value) {
            listdata.emplace_back(text, number, value);
        }
    }
}
void Print(const std::string& filePath, const std::vector<mydata>& listdata, int sum_int, double sum_float) //Method used to write all data to the result fiel
{
    std::ofstream file(filePath);
    std::string line;
    std::string l(64, '-');
    line = "| Text                 | Number   | Value    | Computed result |";
    file << l << '\n' << line << '\n' << l << '\n';
    std::cout << l << '\n' << line << '\n' << l << '\n';
    for (const auto& data : listdata) {
        std::string result = data.ToString() + std::to_string(data.ComputeResult()) + " |";
        file << result << '\n';
        std::cout << result << '\n';
    }
    file << l << '\n';
    std::cout << l << '\n';
    file << "Sum of integers: " << sum_int << '\n';
    file << "Sum of floats: " << sum_float << '\n';
    std::cout << "Sum of integers: " << sum_int << '\n';
    std::cout << "Sum of floats: " << sum_float << '\n';

}
int main() {
    std::string cfd = "C:\\Users\\manon\\Downloads\\ConsoleApplication1\\mano_data1.txt"; // data file
    std::string cfr = "Result_data.txt"; // result file
    const int numThreads = 6; // Number of threads to spawn
    int sumofint = 0;//initiating for computing sums of it
    double sumofdouble = 0.0;//tnitiating for computing sums of float
    std::vector<mydata> listdata; 
    Read(cfd, listdata);   
    std::vector<mydata> resultData;

 

#pragma omp parallel num_threads(numThreads)
    {
        int thread_id = omp_get_thread_num(); // Get the thread ID
        int num_threads = omp_get_num_threads(); // Get total number of threads
       //thread distribution 
        int start = (listdata.size() * thread_id) / num_threads; // Start index for this thread       
        int end = (listdata.size() * (thread_id + 1)) / num_threads; // End index for this thread
  
        for (int i = start; i < end; ++i) {
            mydata& item = listdata[i];   
            if (item.FitsCriterion()) 
            {
#pragma omp critical
                {
                    resultData.push_back(item); 
                    std::cout << "Thread: " << thread_id << " added " << item.getText() << " to the resultData." << '\n';
                    sumofint += item.getNumber();
                    sumofdouble += item.getValue();
                }               
            }
        }
    }
    // Sort result data
    std::sort(resultData.begin(), resultData.end(), [](const mydata a, const mydata b) {
        return a.getNumber() < b.getNumber();
        });

    Print(cfr, resultData, sumofint, sumofdouble);

    return 0;
}
