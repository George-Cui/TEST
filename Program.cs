using System;
using System.Collections.Generic;
//-Prerequisites-

// Before you continue, ensure you have met the following requirements:

// * You have installed Visual Studio or Visual Studio Code.
// * You have setup the c# envirment.
// * You have a basic understanding using debuging configuration.

// -About-

// This program contain these feature:

// * Able to be execute in debugger
// * Contain 10 maps in total.
// * Each level have different zombie object.
// * Player will face a zombie when making the wrong choice in game.
// * Player will die when they have 0 health.
// * Player have chance to escape from the zombie. Higher the cap between 
//      player and zombie, the Higher chance player will have for escape.
// * The answer for each map is random created each game, so are zombies.
// * Player have to get through all 10 maps to win the game.

// -Use-

// Follow the instruction in game and key in 1 or 2, unacceptable input will 
// create a zombie in level. If player already facing a zombie, console will 
// output the choices again and nothing happen internally. 

// If you want to play this game again, you will need to reopen it. Every 
// data from last game will be removed.
namespace A1
{
    ///<summary>
    ///Level is the base of player and zombie. It contain attackforce and health of each charactor
    ///</sumary>
    class Level{

        private int level;
        private int exp;
        private int attackforce;
        private int health;
        private static int MINIMU = 1;
        private static int HEALTHADD = 5;
        private static int ATTACKADD = 3;
        private static int HEALTHDEBUFF = 2;
        private static int ATTACKDE = 1;
        ///The class constructor.
        private Level(int i){
            ///TODO: create a new level base with defult value.
            this.level = i;
            this.exp = 0;
            this.attackforce = 5 + i*ATTACKADD;
            this.health = 17 + i*HEALTHADD;
        }
        public static Level NewLevel(int i){
            ///TODO: create a new level.
            return new Level(i);
            }
        
        private void Upgrade(){
            ///TODO: upgrade player's level and make player full health,reset exp.
            this.level += 1;
            this.attackforce += 3;
            this.health = 17 + level*5;
            this.exp = 0;
            }
        private void ExpPlus(){
            ///TODO: exp + 1, if full, upgrade.
            if (exp == 1){
                Upgrade();
                Console.WriteLine("Congratulations! you are now level up!\nYou now have full health!");
            }
            else{exp+=1;}
        }
        public int getLevel(){
            ///TODO: return the number of level.
            return level;
        }
        public int GetHealth(){
            ///TODO: return the number of health.
            return health;
        }

        public void debuff(){
            ///TODO: debuff each zombie by decreasing their health and attackforce so they wont be 2 op.
            Random r = new Random();
            this.health -= r.Next(MINIMU,level*HEALTHDEBUFF);
            this.attackforce -=r.Next(MINIMU,ATTACKDE);
        }
        public bool fight(Level zombie){
            ///TODO: player fight zombie, each one decrease their health by number of other's attackforce. 
            ///     exp up for player if player killed it.
            this.health -= zombie.attackforce;
            zombie.health -= this.attackforce;
            if (zombie.health <= 0){
                Console.WriteLine("You have killed the Zombie!");
                ExpPlus();
                return false;
            }
            else{
                Console.WriteLine("You have deal zombie " + attackforce + "damage. \nYou now have " + health + " remain.");
                return true;
            }
        }
    }
    public abstract class Menu{
    }
    class Combate : Menu{
        public static int FIGHT = 1;
        public static int ESCAPE = 2;
        public static string TOSTRING = "\nYou met a Zombie! Choose your decision!";
        private Zombie zombie = null;
        public static string COMBATE = "\n1) FIGHT\n2) ESCAPE";
        private Combate(Zombie zombie){
            this.zombie = zombie;
        }
        public static Combate NewCombat(){
            return new Combate(Zombie.MakeZombie()); 
        }
        public Zombie GetZombie(){
            return zombie;
        }
        public void PrintMenu(){
            Console.WriteLine(TOSTRING + COMBATE);
        }

    }
    class Quest : Menu{
        public static int YES = 1;
        public static int NO = 2;
        public static string TOSTRING = YES + ") YES \n" + NO + ") NO";
        private int rightAnswer = 0;
        private Quest(int rightAnswer){
            this.rightAnswer = rightAnswer;
        }
        public static Quest newQuest(int rightAnswer){
            return new Quest(rightAnswer);
        }
        public int getRightNum(){
            return rightAnswer;
        }        

    }

    class Zombie{
        private Level level;
        private Zombie (int i){
            this.level = Level.NewLevel(i);
        }
        public static Zombie MakeZombie(){
            Random r = new Random();
            int medium = Program.PLAYER.getLevelnum();
            int i = 1;
            Zombie z;
            if (medium > 3){
                z = new Zombie(r.Next(medium-2,medium)); 
            }
            else{z = new Zombie(i);}
            z.level.debuff();
            return z;
            }   
        public Level GetLevel(){
            return level;
        }
        public int GetLevelnum(){
            return level.getLevel();
        }          
    }
    
    class Player{
        private Level level;
        private Player(){
            this.level = Level.NewLevel(1);
        }
        
        public static Player newPlayer(){
            return new Player();
        }

        public Level getLevel(){
            return level;
        }
        public int getLevelnum(){
            return level.getLevel();
        }
        
        public void attack(Zombie zombie){
            this.level.fight(zombie.GetLevel());
        }
    }
   
    class BATTLEMAP{
        private Map father = null;
        private Combate menu;
        private Zombie zombie = null;
        private BATTLEMAP(Map map){
            this.father = map;
            Program.BATTLEORNO = true;
            this.menu = Combate.NewCombat();
            this.zombie = menu.GetZombie();
        }
        public static BATTLEMAP NewBattleMap(Map map){
            return new BATTLEMAP(map);
        }
        public Combate GetCombate(){
            return menu;
        }
        public Zombie GetZombie(){
            return zombie;
        }
    }
    class Map{
        private Menu menu;
        private string text;
        private Map previous = null;
        private Map next = null;
        private BATTLEMAP  battlemap= null;
        private Zombie ZOMBIE = null;
        private Map(String text){
            this.text = text;
            this.ZOMBIE = Zombie.MakeZombie();
            Random r = new Random();
            this.menu = Quest.newQuest(r.Next(Combate.FIGHT,Combate.ESCAPE));
        }
        private Map(String text, Map previous){
            this.text = text;
            this.previous = previous;
            this.previous.Add(this);
        }
        public static Map NewMap(String text){
            return new Map(text);
        }
        public void PrintMenu(){
            Console.WriteLine(Quest.TOSTRING);
        }
        public string getString(){
            return text;
        }
        public void AddZombie(){
            this.battlemap = BATTLEMAP.NewBattleMap(this);
        }
        public BATTLEMAP GetBATTLEMAP(){
            return battlemap;
        }
        public Map AddMap(string text){
            if (! hasNext()){
                return new Map(text, this);
            }
            return null;
        }
        private void Add(Map next){
            this.next = next;
        }

        public bool hasNext(){
            return this.next!= null;
        }
        public Map NextMap(){
            if (hasNext()){
                return this.next;
            }
            else {
                Program.GAMEEND = true;
                Console.WriteLine("congratulation! You have escaped from the zombie land! \nRestart the game to play again.");
                return null;
            }
            
        }
        public bool hasPrevious(){
            return this.previous!= null;
        }
    }


    class Battle{
        private static string FAILTOESCAPE = "You failed to escape, and been killed by the zombie.";
        public static string LOSTFIGHT = "You have been killed by zombie.";
        public static string WINFIGHT = "You have killed the zombie! Lets go to your next map!";
        public static string REGAME = "\nRestart the game to play again.";
        public static string ESCAPED = "You have escaped!";
        public static void newBattle (){
                int order = Convert.ToInt32(Console.ReadLine());
                bool pass = false;
                if (order == Combate.FIGHT){
                    Program.PLAYER.getLevel().fight(Program.BATTLEMAP.GetZombie().GetLevel());
                    if (Program.PLAYER.getLevel().GetHealth() <= 0){
                        Program.GAMEEND = true;
                        Console.WriteLine(LOSTFIGHT+REGAME);
                    }
                    else if (Program.BATTLEMAP.GetZombie().GetLevel().GetHealth()<=0){
                        pass = true;
                    }
                }
                else if (order == Combate.ESCAPE){
                    Random r = new Random();
                    if (r.Next(Program.BATTLEMAP.GetZombie().GetLevelnum()-Program.PLAYER.getLevelnum(),Program.BATTLEMAP.GetZombie().GetLevelnum())>1){
                        Console.WriteLine(FAILTOESCAPE+REGAME);
                        Program.GAMEEND = true;
                    }
                    else{
                        Console.WriteLine(ESCAPED);
                    }
                }
            if (pass == true){
                Program.CURRENTMAP = Program.CURRENTMAP.NextMap();
                Program.BATTLEORNO = false;
                Console.WriteLine(WINFIGHT);
            }
            
        }
    }
    class NormalAction{
        private Player player = Program.PLAYER;
        public static string RIGHT = "Nothing happen! Is a good sign! You are arrived to the next level.";
        public static string WRONG = "Oh no! A zombie is coming!";
        public static void newNormalAction (){

            Random r = new Random();
            int rightAnswer = r.Next(Quest.YES,Quest.NO);
            string orderstring = Console.ReadLine();
            int order = Convert.ToInt32(orderstring);
            if ( order == rightAnswer){

                Program.CURRENTMAP = Program.CURRENTMAP.NextMap();
                Console.WriteLine(RIGHT);

            }
            else{

                Program.BATTLEORNO = true;
                Program.CURRENTMAP.AddZombie();
                Program.BATTLEMAP = Program.CURRENTMAP.GetBATTLEMAP();
                

            }

        }
    }
    class Game{
        public static string WELCOME = "Welcome to the zombieland, you goal is to find a \nway to exit the land.\nGood luck on your game!";
        public static string END = "You Escaped!!\n Restart the game to play again.";
        public static int MAXMAPNUM = 9;
        
        private Game(List<String> maps){
            Program.PLAYER = Player.newPlayer();
            Program.ROOTMAP = Map.NewMap(maps[0]);
            Program.CURRENTMAP = Program.ROOTMAP;
            Program.BATTLEMAP = null;
            Program.BATTLEORNO = false;
            Program.GAMEEND = false;

            Map referMap = Program.ROOTMAP;
            Program.GAMEEND = false;
            for(int i = 0; i < MAXMAPNUM; i ++){   
                referMap = referMap.AddMap(maps[i+1]);
            }
        }
        public static Game NewGame(List<String> maps){
            return new Game(maps);
        }
        public void RUN(){
            Console.WriteLine(WELCOME);
            
            Random r = new Random();
            
            while (! Program.GAMEEND){
                
                if (Program.BATTLEORNO){
                    Program.BATTLEMAP.GetCombate().PrintMenu();
                    Battle.newBattle();
                }
                else{
                    Console.WriteLine(Program.CURRENTMAP.getString());
                    Program.CURRENTMAP.PrintMenu();
                    NormalAction.newNormalAction();
                }
            }
            Console.ReadLine();
        }
    }
    class Program
    {
        public static bool BATTLEORNO = false;
        public static bool GAMEEND = true;
        public static Player PLAYER = null;
        public static List<string> MAPLIST= null;
        public static Map ROOTMAP= null;
        public static BATTLEMAP BATTLEMAP = null;
        public static Map CURRENTMAP = null;
        public static Game GAME = null;
        static void Main(string[] args){
            MAPLIST = new List<string>{"(Act 1)You are in the main bio lab. Open door now or later?","(Act 2)You realize you are alone, check the camera First?","(Act 3)Your friend Leo is lying on the floor, help him?",
            "(Act 4)You are now in entry of Fire Exit stair. You know there is a elevator near soon, do you want to use stairs?","(Act 5)You are hungery and you saw a banana, eat it?",
            "(Act 6)You are in parking of the building, do you want to drive your own car?", "(Act 7)You made your way out of the building, but car stoped on the way, check the car?",
            "(Act 8)The car is run out of fuel, research around?", "(Act 9)You found nothing happens. You are so along, you need to get to the bay to escape from the land, move now?",
            "(Act 10)You found a boat, get in now to escape?"};
            GAME = Game.NewGame(MAPLIST);
            GAME.RUN();
        }
    }
}
//asdadsddadsa
    
