using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        
        GameManager gameManager = new GameManager();
        gameManager.StartGame();
    }
}

class GameManager
{
    private string playerName = "Spelare";
    private int playerHp = 0;
    private int playerMaxHp = 100;
    private int playerDamage;

    private int enemyHp = 0;
    private int enemyDamage;
    private string enemyName = "Enemy";

    public void StartGame()
    {
        
        string[] enemyNames;
        try
        {
            enemyNames = File.ReadAllLines(@"./enemy.txt");
            int randomIndex = new Random().Next(0, enemyNames.Length);
            enemyName = enemyNames[randomIndex];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\n");
            enemyName = "Garlock";
        }
        
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
            if(keyInfo.Key == ConsoleKey.J){
                playerHp = playerMaxHp;
                enemyHp = 100;
            }
            else{
                break;
            }
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
            return;
        }

        if(playerHp <= 0) {
            Console.WriteLine("Du förlorade!");
        } else {
            Console.WriteLine("Du vann!");
        }
    }
}