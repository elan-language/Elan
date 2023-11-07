using System.Collections.Generic;
using System.Collections.Immutable;
using static Globals;
using static StandardLibrary.SystemCalls;
using static StandardLibrary.Constants;

public static partial class Globals
{
    public const string welcome = @$"
    ++++++++++++++++++++++++++++++++++++++
    Welcome to the WORDS WITH AQA game
    ++++++++++++++++++++++++++++++++++++++
    
    
    ";
    public const string tileChoiceMenu = @$"
    Do you want to
         replace the tiles you used(1) OR
         get three extra tiles(2) OR
         replace the tiles you used and get three extra tiles(3) OR
         get no new tiles(4) ? ";
    public const string menu3 = @$"
    Either
        enter the word you would like to play OR
        press 1 to display the letter values OR
        press 4 to view the tile queue OR
        press 7 to view your tiles again OR
        press 0 to fill hand and stop the game";
    public static readonly StandardLibrary.ElanList<string> tileMenuChoices = new StandardLibrary.ElanList<string>(@$"1", @$"2", @$"3", @$"4");
    public static readonly StandardLibrary.ElanDictionary<char, int> tileDictionary = new StandardLibrary.ElanDictionary<char, int>(('A', 1), ('B', 2), ('C', 2), ('D', 2), ('E', 1), ('F', 3), ('G', 2), ('H', 3), ('I', 1), ('J', 5), ('K', 3), ('L', 2), ('M', 2), ('N', 1), ('O', 1), ('P', 2), ('Q', 5), ('R', 1), ('S', 1), ('T', 1), ('U', 2), ('V', 3), ('W', 3), ('X', 5), ('Y', 3), ('Z', 5));
    public const int startHandSize = 15;
    public const int maxHandSize = 20;
    public const int maxTilesPlayed = 50;
    public const string letters = @$"****ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string mainMenu = @$"
    ========= 
    MAIN MENU
    =========
    
    1. Play game with random start hand,
    2. Play game with training start hand
    9. Quit";

    public record class QueueOfTiles
    {
        public static QueueOfTiles DefaultInstance { get; } = new QueueOfTiles._DefaultQueueOfTiles();
        private QueueOfTiles() { }
        public QueueOfTiles(int maxSize)
        {
            this.maxSize = maxSize;
            contents = new StandardLibrary.ElanArray<string>(maxSize);
            rear = -1;
        }
        protected virtual StandardLibrary.ElanArray<string> contents { get; set; } = StandardLibrary.ElanArray<string>.DefaultInstance;
        protected virtual int rear { get; set; } = default;
        protected virtual int maxSize { get; set; } = default;
        public virtual Boolean isNotEmpty()
        {

            return rear != -1;
        }
        public virtual string show()
        {
            var result = @$"";
            foreach (var letter in contents)
            {
                result = result + letter;
            }
            return result + newline;
        }
        public virtual string asString()
        {

            return show();
        }
        public virtual void initialise()
        {
            rear = -1;
            for (var count = 0; count <= maxSize - 1; count = count + 1)
            {
                add();
            }
        }
        public virtual void withdrawNextLetter(ref string letter)
        {
            if (isNotEmpty())
            {
                letter = contents[0];
                for (var count = 1; count <= rear; count = count + 1)
                {
                    contents[count - 1] = contents[count];
                }
                contents[rear] = @$"";
                rear = rear - 1;
            }
        }
        public virtual void add()
        {
            if (rear < maxSize - 1)
            {
                rear = rear + 1;
                var n = random(0, 30);
                contents[rear] = StandardLibrary.Functions.asString(letters[n]);
            }
        }
        private record class _DefaultQueueOfTiles : QueueOfTiles
        {
            public _DefaultQueueOfTiles() { }
            protected override StandardLibrary.ElanArray<string> contents => StandardLibrary.ElanArray<string>.DefaultInstance;
            protected override int rear => default;
            protected override int maxSize => default;
            public override void initialise() { }
            public override void withdrawNextLetter(ref string letter) { }
            public override void add() { }
            public override string asString() { return "default QueueOfTiles"; }
        }
    }
    public record class Player
    {
        public static Player DefaultInstance { get; } = new Player._DefaultPlayer();
        private Player() { }
        public Player(string name)
        {
            this.name = name;
            score = 50;
        }
        public virtual string name { get; set; } = "";
        public virtual int score { get; set; } = default;
        public virtual int tilesPlayed { get; set; } = default;
        public virtual string tilesInHand { get; set; } = "";
        public virtual string asString()
        {

            return name;
        }
        public virtual void setTilesInHand(ref string tiles)
        {
            tilesInHand = tiles;
        }
        public virtual void addToLetters(ref char letter)
        {
            tilesInHand = tilesInHand + letter;
        }
        public virtual void addToScore(ref int points)
        {
            score = score + points;
        }
        public virtual void removeLetter(ref char letter)
        {
            var i = StandardLibrary.Functions.indexOf(tilesInHand, letter);
            tilesInHand = tilesInHand[..(i)] + tilesInHand[(i + 1)..];
        }
        public virtual void wordAsWouldBePlayed(ref string word, ref string asPlayed)
        {
            asPlayed = @$"";
            tilesPlayed = tilesPlayed + StandardLibrary.Functions.length(word);
            foreach (var letter in word)
            {
                var c = letter;
                if (!StandardLibrary.Functions.contains(tilesInHand, letter))
                {
                    c = '*';
                }
                asPlayed = asPlayed + c;
            }
        }
        public virtual void removeLetters(ref string wordAsPlayed)
        {
            foreach (var letter in wordAsPlayed)
            {
                var x = letter;
                removeLetter(ref x);
            }
        }
        public virtual void deductAnyPenalty()
        {
            foreach (var tile in tilesInHand)
            {
                score = score - tileDictionary[tile];
            }
        }
        private record class _DefaultPlayer : Player
        {
            public _DefaultPlayer() { }
            public override string name => "";
            public override int score => default;
            public override int tilesPlayed => default;
            public override string tilesInHand => "";
            public override void setTilesInHand(ref string tiles) { }
            public override void addToLetters(ref char letter) { }
            public override void addToScore(ref int points) { }
            public override void removeLetter(ref char letter) { }
            public override void wordAsWouldBePlayed(ref string word, ref string asPlayed) { }
            public override void removeLetters(ref string wordAsPlayed) { }
            public override void deductAnyPenalty() { }
            public override string asString() { return "default Player"; }
        }
    }
    public record class Game
    {
        public static Game DefaultInstance { get; } = new Game._DefaultGame();
        private Game() { }
        public Game(Player player1, Player player2, StandardLibrary.ElanList<string> allowedWords)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.allowedWords = allowedWords;
            tileQueue = new QueueOfTiles(100);
        }
        public virtual StandardLibrary.ElanList<string> allowedWords { get; set; } = StandardLibrary.ElanList<string>.DefaultInstance;
        public virtual Player player1 { get; set; } = Player.DefaultInstance;
        public virtual Player player2 { get; set; } = Player.DefaultInstance;
        public virtual QueueOfTiles tileQueue { get; set; } = QueueOfTiles.DefaultInstance;
        public virtual string checkWordIsValid(string word)
        {

            return StandardLibrary.Functions.contains(allowedWords, word);
        }
        public virtual Boolean isPlayable()
        {

            return player1.tilesPlayed <= maxTilesPlayed && player2.tilesPlayed <= maxTilesPlayed && StandardLibrary.Functions.length(player1.tilesInHand) < maxHandSize && StandardLibrary.Functions.length(player2.tilesInHand) < maxHandSize;
        }
        public virtual Boolean checkWordIsInTiles(string word, Player player)
        {
            var inTiles = true;
            var copyOfTiles = player.tilesInHand;
            for (var count = 0; count <= StandardLibrary.Functions.length(word) - 1; count = count + 1)
            {
                if (StandardLibrary.Functions.contains(copyOfTiles, word[count]))
                {
                    copyOfTiles = StandardLibrary.Functions.remove(copyOfTiles, StandardLibrary.Functions.indexOf(copyOfTiles, word[count]), 1);
                }
                else if (StandardLibrary.Functions.contains(copyOfTiles, '*'))
                {
                    copyOfTiles = StandardLibrary.Functions.remove(copyOfTiles, StandardLibrary.Functions.indexOf(copyOfTiles, '*'), 1);
                }
                else
                {
                    inTiles = false;
                }
            }
            return inTiles;
        }
        public virtual int getScoreForWord(string word)
        {
            var score = 0;
            for (var count = 0; count <= StandardLibrary.Functions.length(word) - 1; count = count + 1)
            {
                score = score + tileDictionary[word[count]];
            }
            if (StandardLibrary.Functions.length(word) > 7)
            {
                score = score + 20;
            }
            else if (StandardLibrary.Functions.length(word) > 5)
            {
                score = score + 5;
            }
            return score;
        }
        public virtual string listTileValues(Player player)
        {
            var values = @$"                  ";
            foreach (var c in player.tilesInHand)
            {
                values = values + tileDictionary[c];
            }
            return values;
        }
        public virtual string showTileQueue()
        {

            return tileQueue.show();
        }
        public virtual string tileValuesAsString()
        {
            var s = @$"
TILE VALUES
";
            foreach (var k in StandardLibrary.Functions.keys(tileDictionary))
            {
                var v = tileDictionary[k];
                s = s + @$"Point for {k}: {v}";
            }
            s = s + newline;
            return s;
        }
        public virtual string asString()
        {

            return @$"A Game";
        }
        public virtual void initialiseForRandomStart()
        {
            setStartingHand(ref player1);
            setStartingHand(ref player2);
            tileQueue.initialise();
        }
        public virtual void initialiseStandard()
        {
            var _setTilesInHand_4_0 = @$"EDKUQMCTIK";
            player1.setTilesInHand(ref _setTilesInHand_4_0);
            var _setTilesInHand_5_0 = @$"BSA* HT*EPR";
            player2.setTilesInHand(ref _setTilesInHand_5_0);
            tileQueue.initialise();
        }
        public virtual void setStartingHand(ref Player player)
        {
            var hand = @$"";
            for (var count = 0; count <= startHandSize - 1; count = count + 1)
            {
                var item = @$"";
                StandardLibrary.Functions.remove(tileQueue, item);
                hand = hand + item;
                tileQueue.add();
            }
            player.setTilesInHand(ref hand);
        }
        public virtual void updateScoresWithPenalty()
        {
            player1.deductAnyPenalty();
            player2.deductAnyPenalty();
        }
        public virtual void updateAfterAllowedWord(ref Player player, ref string word)
        {
            var wordAsPlayed = player.wordAsWouldBePlayed(ref word);
            player.removeLetters(ref wordAsPLayed);
            var _addToScore_13_0 = getScoreForWord(wordAsPlayed);
            player.addToScore(ref _addToScore_13_0);
        }
        public virtual void addEndOfTurnTiles(ref Player player, ref string newTileChoice, ref string word)
        {
            var noOfEndOfTurnTiles = 0;
            if (newTileChoice == @$"1")
            {
                noOfEndOfTurnTiles = StandardLibrary.Functions.length(word);
            }
            else if (newTileChoice == @$"2")
            {
                noOfEndOfTurnTiles = 3;
            }
            else
            {
                noOfEndOfTurnTiles = StandardLibrary.Functions.length(word) + 3;
            }
            for (var count = 0; count <= noOfEndOfTurnTiles - 1; count = count + 1)
            {
                letter = @$"";
                tileQueue.withdrawNextLetter(ref letter);
                player.addToLetters(ref letter);
                tileQueue.add();
            }
        }
        public virtual void fillHandWithTiles(ref Player player)
        {
            while (StandardLibrary.Functions.length(player.tilesInHand) <= maxHandSize)
            {
                var letter = @$"";
                tileQueue.withdrawNextLetter(ref letter);
                player.addToLetters(ref letter);
                tileQueue.add();
            }
        }
        private record class _DefaultGame : Game
        {
            public _DefaultGame() { }
            public override StandardLibrary.ElanList<string> allowedWords => StandardLibrary.ElanList<string>.DefaultInstance;
            public override Player player1 => Player.DefaultInstance;
            public override Player player2 => Player.DefaultInstance;
            public override QueueOfTiles tileQueue => QueueOfTiles.DefaultInstance;
            public override void initialiseForRandomStart() { }
            public override void initialiseStandard() { }
            public override void setStartingHand(ref Player player) { }
            public override void updateScoresWithPenalty() { }
            public override void updateAfterAllowedWord(ref Player player, ref string word) { }
            public override void addEndOfTurnTiles(ref Player player, ref string newTileChoice, ref string word) { }
            public override void fillHandWithTiles(ref Player player) { }
            public override string asString() { return "default Game"; }
        }
    }
}

//public static class Program
//{
//    //private static void Main(string[] args)
//    //{
//    //}
//}


