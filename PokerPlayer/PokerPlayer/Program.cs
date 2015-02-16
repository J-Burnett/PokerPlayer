using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates deck to play with
            Deck currentDeck = new Deck();

            for (int i = 0; i < 5; i++)
            {
                //shuffles deck 5 times
                currentDeck.Shuffle();
            }
            //Players hand
            List<Card> testHand = new List<Card>();
            //testHand.Add(new Card((int)Suit.Club, (int)Rank.Two));
            //testHand.Add(new Card((int)Suit.Diamond, (int)Rank.Two));
            //testHand.Add(new Card((int)Suit.Club, (int)Rank.Five));
            //testHand.Add(new Card((int)Suit.Diamond, (int)Rank.Five));
            //testHand.Add(new Card((int)Suit.Heart, (int)Rank.Five));

            //Creates player
            PokerPlayer player = new PokerPlayer();
            //Draws 5 cards for game and adds them to the player's hand
            player.DrawHand(currentDeck.Deal(5));
            //Prints hand to console
            player.ShowHand();


            //Keeps console open
            Console.ReadLine();
        }
    }
    /// <summary>
    /// Poker player
    /// </summary>
    class PokerPlayer
    {
        //Highest card property
        public Card HighestCard { get; set; }

        //contains dealt hand
        List<Card> Hand = new List<Card>();

        //draws hand
        public void DrawHand(List<Card> cards)
        {
            //adds cards to hand
            this.Hand = cards;
        }

        /// <summary>
        /// Prints contents of Hand
        /// </summary>
        public void ShowHand()
        {
            //Prints each card
            Hand.ForEach(x => x.ShowCard());

            //Prints poker hand results
            switch (this.HandRank)
            {
                case HandType.HighCard:
                    Console.WriteLine("You have a high card.");
                    break;
                case HandType.OnePair:
                    Console.WriteLine("You have one pair.");
                    break;
                case HandType.TwoPair:
                    Console.WriteLine("You have two pair.");
                    break;
                case HandType.ThreeOfAKind:
                    Console.WriteLine("You have three of a kind");
                    break;
                case HandType.Straight:
                    Console.WriteLine("You have a straight");
                    break;
                case HandType.Flush:
                    Console.WriteLine("You have a flush");
                    break;
                case HandType.FullHouse:
                    Console.WriteLine("You have a fullhouse");
                    break;
                case HandType.FourOfAKind:
                    Console.WriteLine("You have four of a kind.");
                    break;
                case HandType.StraightFlush:
                    Console.WriteLine("You have a straight flush!");
                    break;
                case HandType.RoyalFlush:
                    Console.WriteLine("You have a royal flush!!");
                    break;
            }
        }

        // Enum of different hand types
        public enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }

        // Rank of hand that player holds
        private HandType _handRank;
        public HandType HandRank
        {
            get
            {
                if (HasRoyalFlush()) { return HandType.RoyalFlush; }
                if (HasStraightFlush()) { return HandType.StraightFlush; }
                if (HasFourOfAKind()) { return HandType.FourOfAKind; }
                if (HasFullHouse()) { return HandType.FullHouse; }
                if (HasFlush()) { return HandType.Flush; }
                if (HasStraight()) { return HandType.Straight; }
                if (HasThreeOfAKind()) { return HandType.ThreeOfAKind; }
                if (HasTwoPair()) { return HandType.TwoPair; }
                if (HasPair()) { return HandType.OnePair; }
                { return HandType.HighCard; }
            }
            set { _handRank = value; }

        }
        // Constructor that isn't used
        public PokerPlayer() { }

        /// <summary>
        /// Checks if player has one Pair
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasPair()
        {
            //checks for 1 group containing 2 of the same card rank
            return Hand.GroupBy(x => x.CardRank).Where(x => x.Count() == 2).Count() == 1 && Hand.GroupBy(x => x.CardSuit).Distinct().Count() >= 2;
        }

        /// <summary>
        /// Checks if player has two Pairs
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasTwoPair()
        {
            //checks for 2 groups containing 2 of the same card rank
            if (Hand.GroupBy(x => x.CardRank).Where(x => x.Count() == 2).Count() == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if player has Three of a Kind
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasThreeOfAKind()
        {
            //Checks three of same rank and that it occurs 1 time
            return this.Hand.GroupBy(x => x.CardRank).Where(x => x.Count() == 3).Count() == 1;
        }

        /// <summary>
        /// Checks if player has a Straight
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasStraight()
        {
            //Checks for repeat card ranks
            if (Hand.GroupBy(x => x.CardRank).Any(x => x.Count() != 1))
            {
                return false;
            }

            //Orders cards from highest to lowest rank
            Hand = Hand.OrderByDescending(x => x.CardRank).ToList();
            //Subtracts lowest rank from highest
            if (Hand[0].CardRank - Hand[Hand.Count() - 1].CardRank == 4)
            {
               //if the difference is four, 
               return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if player has a flush
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasFlush()
        {
            //Checks for one unique suit
            return Hand.GroupBy(x => x.CardSuit).Count() == 1 && Hand.GroupBy(x => x.CardRank).Distinct().Count() == 5;
        }

        /// <summary>
        /// Checks if player has a Full House
        /// </summary>
        /// <returns>True or false</returns>
        public bool HasFullHouse()
        {
            //Checks if player has a three of a kind and a pair
            if (HasThreeOfAKind() && HasPair())
            {
                this.HandRank = HandType.ThreeOfAKind;
                return true;
            }
            else
            {
                return false;
            }

            //return false;
        }

        /// <summary>
        /// Checks if player has Four of a Kind
        /// </summary>
        /// <returns></returns>
        public bool HasFourOfAKind()
        {
            //Checks for 4 of the same rank in a hand and for 4 distinct suits for cards of the same rank
            return Hand.GroupBy(x => x.CardRank).Any(x => x.Count() == 4) && Hand.GroupBy(x => x.CardSuit).Distinct().Count() >= 4;
           
        }

        /// <summary>
        /// Checks for Straight Flush
        /// </summary>
        /// <returns></returns>
        public bool HasStraightFlush()
        {
            //Checks if hand has a flush and a straight
            return HasFlush() && HasStraight();
        }

        /// <summary>
        /// Checks for Royal Flush
        /// </summary>
        /// <returns></returns>
        public bool HasRoyalFlush()
        {
            //Orders hand from lowest rank to highest
            Hand = this.Hand.OrderBy(x => x.CardRank).ToList();

            //Checks for flush
            if (HasFlush() == true)
            {
                //if so,
                for (int i = 0; i < 5; i++)
                {
                    //Checks the order of the hand 
                    if (this.Hand[0].CardRank == Rank.Ten &&
                    this.Hand[1].CardRank == Rank.Jack &&
                    this.Hand[2].CardRank == Rank.Queen &&
                    this.Hand[3].CardRank == Rank.King &&
                    this.Hand[4].CardRank == Rank.Ace)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    //Guides to pasting your Deck and Card class
    //  *****Deck Class Start*****
    class Deck
    {
        //contains cards in a deck
        public List<Card> DeckOfCards { get; set; }

        //Contains card used
        public List<Card> DiscardedCards { get; set; }

        /// <summary>
        /// Creates deck to play with
        /// </summary>
        public Deck()
        {
            //Deck list to store cards
            this.DeckOfCards = new List<Card>();
            //List to store discarded cards
            this.DiscardedCards = new List<Card>();

            //Chooses card suit
            for (int j = 1; j < 5; j++)
            
            {
                //chooses card rank
                for (int i = 2; i < 15; i++)
                {
                    //adds new card with suit & rank to deck
                    this.DeckOfCards.Add(new Card(i, j));
                }
            }
        }

        /// <summary>
        /// Deals certain number of cards for game
        /// </summary>
        /// <param name="numberOfCards">Number of cards to deal</param>
        /// <returns></returns>
        public List<Card> Deal(int numberOfCards)
        {
            //Stores cards in hand
            List<Card> hand = new List<Card>();
            
            for (int i = 0; i < numberOfCards; i++)
            {
                //adds cards from Deck to Hand
                hand.Add(DeckOfCards[i]);
                //removes the cards from the Deck
                DeckOfCards.Remove(DeckOfCards[i]);
            }
            //returns hand of player
            return hand;
        }

        /// <summary>
        /// Discards cards from hand
        /// </summary>
        /// <param name="hand">Stores cards in hand</param>
        public void Discard(List<Card> hand)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                //Adds cards in hand to discard pile
                DiscardedCards.Add(hand[i]);
            }
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        public void Shuffle()
        {
            //Creates random number
            Random rng = new Random();
            //Stores shuffled cards
            List<Card> shuffledDeck = new List<Card>();

            while (DeckOfCards.Count > 0)
            {
                //pulls random card from deck
                Card newCard = DeckOfCards[rng.Next(0, DeckOfCards.Count())];
                //adds to shuffled deck
                shuffledDeck.Add(newCard);
                //removes from unshuffled deck
                DeckOfCards.Remove(newCard);
            }
            //makes deck new shuffled deck
            DeckOfCards = shuffledDeck;
        }
    }
    //  *****Deck Class End*******

    //  *****Card Class Start*****

    //Card Suits
    public enum Suit
    {
        Spade = 1,
        Diamond,
        Club,
        Heart
    }

    //Card Ranks
    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }

    class Card
    {
        //Suit and Rank properties
        public Rank CardRank { get; set; }
        public Suit CardSuit { get; set; }
       

        //Card Constructor
        public Card(int rank, int suit)
        {
            this.CardRank = (Rank)rank;
            this.CardSuit = (Suit)suit;
          
        }

        /// <summary>
        /// Prints cards in hand
        /// </summary>
        public void ShowCard()
        {
            //prints cards to console
            Console.WriteLine("{0} of {1}s", this.CardRank, this.CardSuit);
        }
    }
    //  *****Card Class End*******
}
