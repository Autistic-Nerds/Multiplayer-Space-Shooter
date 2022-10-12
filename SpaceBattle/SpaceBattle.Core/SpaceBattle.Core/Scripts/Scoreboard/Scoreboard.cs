using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;
using System;

namespace SpaceBattle
{
    public class Scoreboard
    {
        HttpClient client = new HttpClient();

        //GET CALL
        string apiURL = "https://localhost:5001/api/score";
        UriBuilder builder = new UriBuilder();


    }
}