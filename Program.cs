using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


class Program
{
    static void Main(string[] args)
    {
        GameManager.LoadKillCount();
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }
}

class GameManager
{
    private string playerName = "Spelare";
    private int playerHp = 100;
    private int playerMaxHp = 100;
    private int playerDamage;

    private int enemyHp = 100;
    private int enemyDamage;
    private string enemyName = "Enemy";

    private static int killCount;
    private static string killCountFilePath = "killcount.json";

    public void StartGame()
    {
        enemyName = LoadRandomEnemy();

        if(enemyName == "Ronaldo"){
            enemyHp = 1000;
        }
        System.Console.WriteLine($"kills: {killCount} \n");
        Console.WriteLine("SPACE för attack \n");
        Console.WriteLine("Klicka en knapp för att starta!");
        Console.ReadKey();
        Console.Clear();

        AskName();
        while(true){
            Console.Clear();
            RunGameLoop();
            Console.WriteLine("Vill du spela igen? (J/N)");
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            if(keyInfo.Key == ConsoleKey.J || keyInfo.Key == ConsoleKey.Spacebar){
                playerHp = playerMaxHp;
                enemyHp = 100;
            }
            else{
                break;
            }
        } 
    }

    private string LoadRandomEnemy(){
        string[] enemyNames;
        try
        {
            enemyNames = File.ReadAllLines(@"./enemy.txt");
            int randomIndex = new Random().Next(0, enemyNames.Length);
            return enemyNames[randomIndex];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\n");
            return "Garlock";
        }
    }

    private static void AddKillCount()
    {
        killCount++;
        SaveKillCount();
    }

    private static void SaveKillCount() {
        try{
            var data = new Dictionary<string, int> { { "Kills", killCount} };
            string json = System.Text.Json.JsonSerializer.Serialize(data);
            File.WriteAllText("killcount.json", json);
        }catch(Exception ex){
            Console.WriteLine("kunde ispara killcount: " + ex.Message);
        }
    }

    public static void LoadKillCount(){
        try
        {
            if (File.Exists(killCountFilePath))
            {
                string json = File.ReadAllText(killCountFilePath);
                
                var data = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                killCount = data?["Kills"] ?? 0;
            }
            else
            {
                killCount = 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading kill count: {ex.Message}");
            killCount = 0;
        }

    }

    private void AskName()
    {
        do
        {
            Console.Write("Skriv Ditt namn: ");
            playerName = Console.ReadLine() ?? string.Empty;
        } while (string.IsNullOrEmpty(playerName));
        
    }

    private void RunGameLoop()
    {
        bool playerTurn = true;
        Random random = new Random();

        

        while (playerHp > 0 && enemyHp > 0)
        {
            Console.WriteLine($"Du har {playerHp} hp kvar \n{enemyName} har {enemyHp} hp kvar");
            System.Console.WriteLine($"\n\nkills: {killCount}");

            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
            Console.Clear();

            switch (keyInfo.Key)
            {
                case ConsoleKey.Spacebar:
                    if(playerTurn){
                        playerDamage = random.Next(1, 10);
                        enemyHp -= playerDamage;
                        if(playerDamage > 5){
                            Console.WriteLine($"{playerName} smiskade {enemyName} för {playerDamage} hp och gjorde en critical hit!!");
                        }else{
                            Console.WriteLine($"{playerName} smiskade {enemyName} för {playerDamage} hp");
                        }
                        playerTurn = false;  
                    }
                    else{
                        enemyDamage = random.Next(1, 10);
                        playerHp -= enemyDamage;
                        if(enemyDamage > 5){
                            Console.WriteLine($"{enemyName} smiskade dig för {enemyDamage} hp och gjorde en critical hit!!");
                        }else{
                            Console.WriteLine($"{enemyName} smiskade dig för {enemyDamage} hp");
                        }
                        playerTurn = true;
                        
                    }
                    break;
                case ConsoleKey.Escape:
                    Console.WriteLine("Du har avslutat spelet");
                    return;
            }
            
        }
        winCheck();
    }

    void winCheck(){
        Console.Clear();

        if(playerHp < 0) playerHp = 0;
        if(enemyHp < 0) playerHp = 0;

        if(playerHp == enemyHp) {
            Console.WriteLine("Det blev oavgjort!"); 
            AddKillCount();
            return;
        }

        if(playerHp <= 0) {
            Console.WriteLine("Du förlorade!");
        } else {
            Console.WriteLine("Du vann!");
            AddKillCount();
        }
    }
}