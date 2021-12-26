using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

using System.Text.Json;


namespace dapp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "HTTP://localhost:7545";
            string address = "0x81e3eC0b87429025Db8a3cbFAaA6E9fE9bf09842";

            string fileName = "solidity/build/contracts/Vote.json";
            string jsonString = File.ReadAllText(fileName);

            string ABI; 



            using (JsonDocument json = JsonDocument.Parse(jsonString))
            {
                ABI = json.RootElement.GetProperty("abi").ToString();
            }


            Web3 web3 = new Web3(url);
            Contract voteContract = web3.Eth.GetContract(ABI, address);

            //Reads the vote count for Candidate 1 and Candidate 2
            Task<BigInteger> candidate1Function = voteContract.GetFunction("candidate1").CallAsync<BigInteger>();

            candidate1Function.Wait();
            int candidate1 = (int)candidate1Function.Result;
            Task<BigInteger> candidate2Function = voteContract.GetFunction("candidate2").CallAsync<BigInteger>();
            candidate2Function.Wait();
            int candidate2 = (int)candidate2Function.Result;
            Console.WriteLine("Candidate 1 votes: {0}", candidate1);
            Console.WriteLine("Candidate 2 votes: {0}", candidate2);
            Console.Write("Enter the address of your account: ");

            string accountAddress = Console.ReadLine();     //Prompts for the users vote.
            int vote = 0;
            Console.WriteLine("Press 1 to vote for candidate 1, Press 2 to vote for candidate 2: ");
            Int32.TryParse(Convert.ToChar(Console.Read()).ToString(), out vote);
            Console.WriteLine("You pressed {0}", vote);    //Executes the vote on the contract.
            try
            {
                HexBigInteger gas = new HexBigInteger(new BigInteger(400000));
                HexBigInteger value = new HexBigInteger(new BigInteger(0));
                Task<string> castVoteFunction = voteContract.GetFunction("castVote").SendTransactionAsync(accountAddress, gas, value, vote);
                castVoteFunction.Wait();
                Console.WriteLine("Vote Cast!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }


        }
    }
}
